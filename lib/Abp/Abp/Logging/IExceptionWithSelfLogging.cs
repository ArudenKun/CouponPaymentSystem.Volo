using Microsoft.Extensions.Logging;

namespace Abp.Logging;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}
