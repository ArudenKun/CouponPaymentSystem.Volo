namespace Abp.Logging;

public interface IInitLoggerFactory
{
    IInitLogger<T> Create<T>();
}
