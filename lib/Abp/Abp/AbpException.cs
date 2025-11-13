using System.Runtime.Serialization;

namespace Abp;

/// <summary>
/// Base exception type for those are thrown by Abp system for Abp specific exceptions.
/// </summary>
public class AbpException : Exception
{
    public AbpException() { }

    public AbpException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context) { }

    public AbpException(string? message)
        : base(message) { }

    public AbpException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
