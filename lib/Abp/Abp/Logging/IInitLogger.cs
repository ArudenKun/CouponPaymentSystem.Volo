using Microsoft.Extensions.Logging;

namespace Abp.Logging;

public interface IInitLogger<out T> : ILogger<T>
{
    public List<AbpInitLogEntry> Entries { get; }
}
