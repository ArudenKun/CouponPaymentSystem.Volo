using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Web.Resources;

namespace Volo.Abp.Web.Models;

public class ErrorInfoBuilder : IErrorInfoBuilder, ISingletonDependency
{
    private IExceptionToErrorInfoConverter Converter { get; set; }

    public ErrorInfoBuilder(
        IStringLocalizer<AbpWebResource> localizationManager,
        IOptions<AbpWebOptions> abpWebOptions
    )
    {
        Converter = new DefaultErrorInfoConverter(localizationManager, abpWebOptions);
    }

    /// <inheritdoc/>
    public ErrorInfo BuildForException(Exception exception)
    {
        return Converter.Convert(exception);
    }

    /// <summary>
    /// Adds an exception converter that is used by <see cref="BuildForException"/> method.
    /// </summary>
    /// <param name="converter">Converter object</param>
    public void AddExceptionConverter(IExceptionToErrorInfoConverter converter)
    {
        converter.Next = Converter;
        Converter = converter;
    }
}
