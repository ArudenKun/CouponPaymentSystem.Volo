using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Abp.Logging;
using Microsoft.Extensions.Logging;

namespace Abp.Runtime.Validation;

/// <summary>
/// This exception type is used to throws validation exceptions.
/// </summary>
[Serializable]
public class AbpValidationException : AbpException, IHasLogLevel
{
    /// <summary>
    /// Default log severity
    /// </summary>
    public static LogLevel DefaultLogSeverity = LogLevel.Warning;

    /// <summary>
    /// Detailed list of validation errors for this exception.
    /// </summary>
    public IList<ValidationResult> ValidationErrors { get; set; }

    /// <summary>
    /// Exception severity.
    /// Default: Warn.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AbpValidationException()
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor for serializing.
    /// </summary>
    public AbpValidationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    public AbpValidationException(string message)
        : base(message)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="validationErrors">Validation errors</param>
    public AbpValidationException(string message, IList<ValidationResult> validationErrors)
        : base(message)
    {
        ValidationErrors = validationErrors;
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public AbpValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = DefaultLogSeverity;
    }
}
