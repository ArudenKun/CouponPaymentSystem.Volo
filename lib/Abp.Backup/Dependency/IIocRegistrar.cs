using System.Reflection;
using Stashbox;
using Stashbox.Lifetime;

namespace Abp.Dependency;

/// <summary>
/// Define interface for classes those are used to register dependencies.
/// </summary>
public interface IIocRegistrar : IDependencyRegistrator
{
    /// <summary>
    /// Adds a dependency registrar for conventional registration.
    /// </summary>
    /// <param name="registrar">dependency registrar</param>
    void AddConventionalRegistrar(IConventionalDependencyRegistrar registrar);

    /// <summary>
    /// Registers types of given assembly by all conventional registrars. See <see cref="IocManager.AddConventionalRegistrar"/> method.
    /// </summary>
    /// <param name="assembly">Assembly to register</param>
    void RegisterAssemblyByConvention(Assembly assembly);

    /// <summary>
    /// Registers types of given assembly by all conventional registrars. See <see cref="IocManager.AddConventionalRegistrar"/> method.
    /// </summary>
    /// <param name="assembly">Assembly to register</param>
    /// <param name="config">Additional configuration</param>
    void RegisterAssemblyByConvention(Assembly assembly, ConventionalRegistrationConfig config);

    /// <summary>
    /// Registers a type as self registration.
    /// </summary>
    /// <typeparam name="T">Type of the class</typeparam>
    /// <param name="descriptor">lifetime of the objects of this type</param>
    void Register<T>(LifetimeDescriptor? descriptor = null)
        where T : class;

    /// <summary>
    /// Registers a type as self registration.
    /// </summary>
    /// <param name="type">Type of the class</param>
    /// <param name="descriptor">Lifestyle of the objects of this type</param>
    void Register(Type type, LifetimeDescriptor? descriptor = null);

    /// <summary>
    /// Registers a type with it's implementation.
    /// </summary>
    /// <typeparam name="TType">Registering type</typeparam>
    /// <typeparam name="TImpl">The type that implements <typeparamref name="TType"/></typeparam>
    /// <param name="descriptor">Lifestyle of the objects of this type</param>
    void Register<TType, TImpl>(LifetimeDescriptor? descriptor = null)
        where TType : class
        where TImpl : class, TType;

    /// <summary>
    /// Registers a type with it's implementation.
    /// </summary>
    /// <param name="type">Type of the class</param>
    /// <param name="impl">The type that implements <paramref name="type"/></param>
    /// <param name="descriptor">Lifestyle of the objects of this type</param>
    void Register(Type type, Type impl, LifetimeDescriptor? descriptor = null);

    /// <summary>
    /// Checks whether given type is registered before.
    /// </summary>
    /// <param name="type">Type to check</param>
    bool IsRegistered(Type type);

    /// <summary>
    /// Checks whether given type is registered before.
    /// </summary>
    /// <typeparam name="TType">Type to check</typeparam>
    bool IsRegistered<TType>();
}
