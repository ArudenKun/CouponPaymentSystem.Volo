namespace Abp.DependencyInjection;

/// <summary>
/// This interface is used to wrap an object that is resolved from IOC container.
/// It inherits <see cref="IDisposable"/>, so resolved object can be easily released.
/// In <see cref="IDisposable.Dispose"/> method
/// This is non-generic version of <see cref="IDisposableDependencyServiceWrapper{T}"/> interface.
/// </summary>
public interface IDisposableDependencyServiceWrapper : IDisposableDependencyServiceWrapper<object>;

/// <summary>
/// This interface is used to wrap an object that is resolved from IOC container.
/// It inherits <see cref="IDisposable"/>, so resolved object can be easily released.
/// In <see cref="IDisposable.Dispose"/> method
/// </summary>
/// <typeparam name="T">Type of the object</typeparam>
public interface IDisposableDependencyServiceWrapper<out T> : IDisposable
{
    /// <summary>
    /// The resolved object.
    /// </summary>
    T Service { get; }
}
