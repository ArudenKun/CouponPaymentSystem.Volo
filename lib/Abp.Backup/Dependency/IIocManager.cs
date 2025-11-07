using Stashbox;

namespace Abp.Dependency;

/// <summary>
/// This interface is used to directly perform dependency injection tasks.
/// </summary>
public interface IIocManager : IIocRegistrar, IIocResolver
{
    /// <summary>
    /// Reference to the Castle Windsor Container.
    /// </summary>
    IStashboxContainer IocContainer { get; }

    /// <summary>
    /// Checks whether given type is registered before.
    /// </summary>
    /// <param name="type">Type to check</param>
    new bool IsRegistered(Type type);

    /// <summary>
    /// Checks whether given type is registered before.
    /// </summary>
    /// <typeparam name="T">Type to check</typeparam>
    new bool IsRegistered<T>();
}
