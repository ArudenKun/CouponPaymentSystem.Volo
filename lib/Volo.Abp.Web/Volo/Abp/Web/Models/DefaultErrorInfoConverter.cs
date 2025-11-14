using System.Text;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Web.Resources;

namespace Volo.Abp.Web.Models;

//TODO@Halil: I did not like constructing ErrorInfo this way. It works wlll but I think we should change it later...
internal class DefaultErrorInfoConverter : IExceptionToErrorInfoConverter
{
    private readonly IStringLocalizer<AbpWebResource> _stringLocalizer;
    private readonly AbpWebOptions _abpWebOptions;

    public IExceptionToErrorInfoConverter? Next { private get; set; }

    private bool SendAllExceptionsToClients => _abpWebOptions.SendAllExceptionsToClients;

    public DefaultErrorInfoConverter(
        IStringLocalizer<AbpWebResource> stringLocalizer,
        IOptions<AbpWebOptions> abpWebOptions
    )
    {
        _stringLocalizer = stringLocalizer;
        _abpWebOptions = abpWebOptions.Value;
    }

    public ErrorInfo Convert(Exception exception)
    {
        var errorInfo = CreateErrorInfoWithoutCode(exception);

        if (exception is IHasErrorCode code)
        {
            errorInfo.Code = code.Code;
        }

        return errorInfo;
    }

    private ErrorInfo CreateErrorInfoWithoutCode(Exception exception)
    {
        if (SendAllExceptionsToClients)
        {
            return CreateDetailedErrorInfoFromException(exception);
        }

        if (exception is AggregateException { InnerException: not null } aggException)
        {
            if (aggException.InnerException is UserFriendlyException or AbpValidationException)
            {
                exception = aggException.InnerException;
            }
        }

        if (exception is UserFriendlyException userFriendlyException)
        {
            return new ErrorInfo(
                userFriendlyException.Message,
                userFriendlyException.Details ?? string.Empty
            );
        }

        if (exception is AbpValidationException validationException)
        {
            return new ErrorInfo(L("ValidationError"))
            {
                ValidationErrors = GetValidationErrorInfos(validationException),
                Details = GetValidationErrorNarrative(validationException),
            };
        }

        if (exception is EntityNotFoundException entityNotFoundException)
        {
            if (entityNotFoundException.EntityType != null)
            {
                return new ErrorInfo(
                    string.Format(
                        L("EntityNotFound"),
                        entityNotFoundException.EntityType.Name,
                        entityNotFoundException.Id
                    )
                );
            }

            return new ErrorInfo(entityNotFoundException.Message);
        }

        if (exception is AbpAuthorizationException authorizationException)
        {
            return new ErrorInfo(authorizationException.Message);
        }

        return new ErrorInfo(L("InternalServerError"));
    }

    private ErrorInfo CreateDetailedErrorInfoFromException(Exception exception)
    {
        var detailBuilder = new StringBuilder();

        AddExceptionToDetails(exception, detailBuilder);

        var errorInfo = new ErrorInfo(exception.Message, detailBuilder.ToString());

        if (exception is AbpValidationException validationException)
        {
            errorInfo.ValidationErrors = GetValidationErrorInfos(validationException);
        }

        return errorInfo;
    }

    private void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder)
    {
        //Exception Message
        detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

        //Additional info for UserFriendlyException
        if (exception is UserFriendlyException userFriendlyException)
        {
            if (!string.IsNullOrEmpty(userFriendlyException.Details))
            {
                detailBuilder.AppendLine(userFriendlyException.Details);
            }
        }

        //Additional info for AbpValidationException
        if (exception is AbpValidationException validationException)
        {
            if (validationException.ValidationErrors.Count > 0)
            {
                detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
            }
        }

        //Exception StackTrace
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
        }

        //Inner exception
        if (exception.InnerException != null)
        {
            AddExceptionToDetails(exception.InnerException, detailBuilder);
        }

        //Inner exceptions for AggregateException
        if (exception is AggregateException aggException)
        {
            if (aggException.InnerExceptions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var innerException in aggException.InnerExceptions)
            {
                AddExceptionToDetails(innerException, detailBuilder);
            }
        }
    }

    private ValidationErrorInfo[] GetValidationErrorInfos(
        AbpValidationException validationException
    )
    {
        var validationErrorInfos = new List<ValidationErrorInfo>();

        foreach (var validationResult in validationException.ValidationErrors)
        {
            var validationError = new ValidationErrorInfo(validationResult.ErrorMessage);

            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                validationError.Members = validationResult
                    .MemberNames.Select(m => m.ToCamelCase())
                    .ToArray();
            }

            validationErrorInfos.Add(validationError);
        }

        return validationErrorInfos.ToArray();
    }

    private string GetValidationErrorNarrative(AbpValidationException validationException)
    {
        var detailBuilder = new StringBuilder();
        detailBuilder.AppendLine(L("ValidationNarrativeTitle"));

        foreach (var validationResult in validationException.ValidationErrors)
        {
            detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
            detailBuilder.AppendLine();
        }

        return detailBuilder.ToString();
    }

    private string L(string name)
    {
        try
        {
            return _stringLocalizer.GetString(name);
        }
        catch (Exception)
        {
            return name;
        }
    }
}
