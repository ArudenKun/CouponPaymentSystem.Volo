namespace Volo.Abp.DependencyInjection;

public interface IDisposableServiceWrapper : IDisposableServiceWrapper<object>;

public interface IDisposableServiceWrapper<out T> : IDisposable
{
    T Service { get; }
}
