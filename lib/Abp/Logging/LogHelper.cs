using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Runtime.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Abp.Logging
{
    /// <summary>
    /// This class can be used to write logs from somewhere where it's a hard to get a reference to the <see cref="ILogger"/>.
    /// Normally, use <see cref="ILogger"/> with property injection wherever it's possible.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// A reference to the logger.
        /// </summary>
        public static ILogger Logger { get; private set; }

        static LogHelper()
        {
            Logger = IocManager.Instance.IsRegistered(typeof(ILoggerFactory))
                ? IocManager.Instance.Resolve<ILoggerFactory>().CreateLogger(typeof(LogHelper))
                : NullLogger.Instance;
        }

        public static void LogException(Exception ex)
        {
            LogException(Logger, ex);
        }

        public static void LogException(ILogger logger, Exception ex)
        {
            var severity = (ex as IHasLogSeverity)?.Severity ?? LogLevel.Error;

            logger.Log(severity, ex, ex.Message);

            LogValidationErrors(logger, ex);
        }

        private static void LogValidationErrors(ILogger logger, Exception exception)
        {
            //Try to find inner validation exception
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException?.InnerException is AbpValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (exception is not AbpValidationException)
            {
                return;
            }

            if (
                exception is not AbpValidationException validationException
                || validationException.ValidationErrors.IsNullOrEmpty()
            )
            {
                return;
            }

            logger.Log(
                validationException.Severity,
                "There are {Count} validation errors:",
                validationException.ValidationErrors.Count
            );
            foreach (var validationResult in validationException.ValidationErrors)
            {
                var memberNames = "";
                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                }

                logger.Log(
                    validationException.Severity,
                    "{Message}{Names}",
                    validationResult.ErrorMessage,
                    memberNames
                );
            }
        }
    }
}
