using System.Runtime.Serialization;
using Abp.Logging;
using Microsoft.Extensions.Logging;

namespace Abp.UI;

/// <summary>
/// This exception type is directly shown to the user.
/// </summary>
[Serializable]
public class UserFriendlyException : AbpException, IHasLogLevel, IHasErrorCode
{
    /// <summary>
    /// Default log severity
    /// </summary>
    public static LogLevel DefaultLogSeverity = LogLevel.Warning;

    /// <summary>
    /// Additional information about the exception.
    /// </summary>
    public string Details { get; private set; }

    /// <summary>
    /// An arbitrary error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Severity of the exception.
    /// Default: Warn.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public UserFriendlyException()
    {
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor for serializing.
    /// </summary>
    public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    public UserFriendlyException(string message)
        : base(message)
    {
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="logLevel">Exception severity</param>
    public UserFriendlyException(string message, LogLevel logLevel)
        : base(message)
    {
        LogLevel = logLevel;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Exception message</param>
    /// <param name="details">Additional information about the exception</param>
    public UserFriendlyException(string code, string message, string details)
        : base(message)
    {
        Code = code;
        Details = details;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public UserFriendlyException(string message, Exception innerException)
        : base(message, innerException)
    {
        LogLevel = DefaultLogSeverity;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="details">Additional information about the exception</param>
    /// <param name="innerException">Inner exception</param>
    public UserFriendlyException(string message, string details, Exception innerException)
        : this(message, innerException)
    {
        Details = details;
    }
}
