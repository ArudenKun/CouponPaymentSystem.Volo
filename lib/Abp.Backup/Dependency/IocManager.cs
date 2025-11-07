using System.Reflection;
using Castle.DynamicProxy;
using Stashbox;
using Stashbox.Lifetime;
using Stashbox.Registration.Fluent;
using Stashbox.Resolution;

namespace Abp.Dependency;

/// <summary>
/// This class is used to directly perform dependency injection tasks.
/// </summary>
public class IocManager : IIocManager
{
    /// <summary>
    /// The Singleton instance.
    /// </summary>
    public static IocManager Instance { get; private set; }

    /// <summary>
    /// Singletone instance for Castle ProxyGenerator.
    /// From Castle.Core documentation it is highly recommended to use single instance of ProxyGenerator to avoid memoryleaks and performance issues
    /// Follow next links for more details:
    /// <a href="https://github.com/castleproject/Core/blob/master/docs/dynamicproxy.md">Castle.Core documentation</a>,
    /// <a href="http://kozmic.net/2009/07/05/castle-dynamic-proxy-tutorial-part-xii-caching/">Article</a>
    /// </summary>
    private static readonly ProxyGenerator ProxyGeneratorInstance = new();

    /// <summary>
    /// Reference to the Castle Windsor Container.
    /// </summary>
    public IStashboxContainer IocContainer { get; }

    /// <summary>
    /// List of all registered conventional registrars.
    /// </summary>
    private readonly List<IConventionalDependencyRegistrar> _conventionalRegistrars;

    static IocManager()
    {
        Instance = new IocManager();
    }

    /// <summary>
    /// Creates a new <see cref="IocManager"/> object.
    /// Normally, you don't directly instantiate an <see cref="IocManager"/>.
    /// This may be useful for test purposes.
    /// </summary>
    internal IocManager()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        IocContainer = CreateContainer();
        _conventionalRegistrars = [];

        //Register self!
        IocContainer.Register<IocManager>(c =>
            c.WithInstance(this)
                .AsServiceAlso<IIocManager>()
                .AsServiceAlso<IIocRegistrar>()
                .AsServiceAlso<IIocResolver>()
        );
    }

    protected virtual IStashboxContainer CreateContainer()
    {
        return new StashboxContainer(c => c.WithDefaultLifetime(Lifetimes.Singleton));
    }

    /// <summary>
    /// Adds a dependency registrar for conventional registration.
    /// </summary>
    /// <param name="registrar">dependency registrar</param>
    public void AddConventionalRegistrar(IConventionalDependencyRegistrar registrar)
    {
        _conventionalRegistrars.Add(registrar);
    }

    /// <summary>
    /// Registers types of given assembly by all conventional registrars. See <see cref="AddConventionalRegistrar"/> method.
    /// </summary>
    /// <param name="assembly">Assembly to register</param>
    public void RegisterAssemblyByConvention(Assembly assembly)
    {
        RegisterAssemblyByConvention(assembly, new ConventionalRegistrationConfig());
    }

    /// <summary>
    /// Registers types of given assembly by all conventional registrars. See <see cref="AddConventionalRegistrar"/> method.
    /// </summary>
    /// <param name="assembly">Assembly to register</param>
    /// <param name="config">Additional configuration</param>
    public void RegisterAssemblyByConvention(
        Assembly assembly,
        ConventionalRegistrationConfig config
    )
    {
        var context = new ConventionalRegistrationContext(assembly, this, config);

        foreach (var registerer in _conventionalRegistrars)
        {
            registerer.RegisterAssembly(context);
        }

        if (config.InstallInstallers)
        {
            IocContainer.Install(assembly);
        }
    }

    /// <inheritdoc />
    public void Register<TType>(LifetimeDescriptor? lifeStyle = null)
        where TType : class
    {
        IocContainer.Register<TType>();
    }

    /// <inheritdoc />
    public void Register(Type type, LifetimeDescriptor? lifeStyle = null)
    {
        lifeStyle ??= Lifetimes.Singleton;
        IocContainer.Register(type, c => c.WithLifetime(lifeStyle));
    }

    /// <inheritdoc />
    public void Register<TType, TImpl>(LifetimeDescriptor? lifeStyle = null)
        where TType : class
        where TImpl : class, TType
    {
        lifeStyle ??= Lifetimes.Singleton;
        IocContainer.Register<TType, TImpl>(c => c.WithLifetime(lifeStyle));
    }

    /// <inheritdoc />
    public void Register(Type type, Type impl, LifetimeDescriptor? lifeStyle = null)
    {
        lifeStyle ??= Lifetimes.Singleton;
        IocContainer.Register(type, impl, c => c.WithLifetime(lifeStyle));
    }

    public void Release(object obj)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool IsRegistered(Type type)
    {
        return IocContainer.IsRegistered(type);
    }

    /// <inheritdoc />
    public bool IsRegistered<TType>()
    {
        return IocContainer.IsRegistered<TType>();
    }

    /// <inheritdoc />
    public T Resolve<T>()
    {
        return IocContainer.Resolve<T>();
    }

    /// <inheritdoc />
    public T Resolve<T>(Type type)
    {
        return (T)IocContainer.Resolve(type);
    }

    public T Resolve<T>(object argumentsAsAnonymousType)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public T Resolve<T>(params object[] argumentsAsAnonymousType)
    {
        return IocContainer.Resolve<T>(argumentsAsAnonymousType);
    }

    /// <inheritdoc />
    public object Resolve(Type type)
    {
        return IocContainer.Resolve(type);
    }

    /// <inheritdoc />
    public object Resolve(Type type, object argumentsAsAnonymousType)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object Resolve(
        Type typeFrom,
        object? name,
        object[]? dependencyOverrides,
        ResolutionBehavior resolutionBehavior = ResolutionBehavior.Default
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object? ResolveOrDefault(Type typeFrom)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object? ResolveOrDefault(
        Type typeFrom,
        object? name,
        object[]? dependencyOverrides,
        ResolutionBehavior resolutionBehavior = ResolutionBehavior.Default
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Delegate ResolveFactory(
        Type typeFrom,
        object? name = null,
        ResolutionBehavior resolutionBehavior = ResolutionBehavior.Default,
        params Type[] parameterTypes
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Delegate? ResolveFactoryOrDefault(
        Type typeFrom,
        object? name = null,
        ResolutionBehavior resolutionBehavior = ResolutionBehavior.Default,
        params Type[] parameterTypes
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IDependencyResolver BeginScope(object? name = null, bool attachToParent = false)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void PutInstanceInScope(
        Type typeFrom,
        object instance,
        bool withoutDisposalTracking = false,
        object? name = null
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public TTo BuildUp<TTo>(
        TTo instance,
        ResolutionBehavior resolutionBehavior = ResolutionBehavior.Default
    )
        where TTo : class
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object Activate(
        Type type,
        ResolutionBehavior resolutionBehavior,
        params object[] arguments
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public ValueTask InvokeAsyncInitializers(CancellationToken token = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool CanResolve(
        Type typeFrom,
        object? name = null,
        ResolutionBehavior resolutionBehavior = ResolutionBehavior.Default
    )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IEnumerable<DelegateCacheEntry> GetDelegateCacheEntries()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object Resolve(Type type, params object[] argumentsAsAnonymousType)
    {
        return IocContainer.Resolve(type, argumentsAsAnonymousType);
    }

    ///<inheritdoc/>
    public T[] ResolveAll<T>()
    {
        return IocContainer.ResolveAll<T>().ToArray();
    }

    /// <inheritdoc />
    public T[] ResolveAll<T>(object argumentsAsAnonymousType)
    {
        throw new NotImplementedException();
    }

    ///<inheritdoc/>
    public T[] ResolveAll<T>(params object[] argumentsAsAnonymousType)
    {
        return IocContainer.ResolveAll<T>(null, argumentsAsAnonymousType).ToArray();
    }

    ///<inheritdoc/>
    public object[] ResolveAll(Type type)
    {
        return IocContainer.ResolveAll(type).ToArray();
    }

    /// <inheritdoc />
    public object[] ResolveAll(Type type, object argumentsAsAnonymousType)
    {
        throw new NotImplementedException();
    }

    ///<inheritdoc/>
    public object[] ResolveAll(Type type, params object[] argumentsAsAnonymousType)
    {
        return IocContainer.ResolveAll(type, argumentsAsAnonymousType).ToArray();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        IocContainer.Dispose();
    }

    /// <inheritdoc />
    public IStashboxContainer Register<TFrom, TTo>(
        Action<RegistrationConfigurator<TFrom, TTo>> configurator
    )
        where TFrom : class
        where TTo : class, TFrom
    {
        return IocContainer.Register(configurator);
    }

    /// <inheritdoc />
    public IStashboxContainer Register<TFrom, TTo>(object? name = null)
        where TFrom : class
        where TTo : class, TFrom
    {
        return IocContainer.Register<TFrom, TTo>(name);
    }

    /// <inheritdoc />
    public IStashboxContainer Register<TFrom>(
        Type typeTo,
        Action<RegistrationConfigurator<TFrom, TFrom>>? configurator = null
    )
        where TFrom : class
    {
        return IocContainer.Register(typeTo, configurator);
    }

    /// <inheritdoc />
    public IStashboxContainer Register(
        Type typeFrom,
        Type typeTo,
        Action<RegistrationConfigurator>? configurator = null
    )
    {
        return IocContainer.Register(typeFrom, typeTo, configurator);
    }

    /// <inheritdoc />
    public IStashboxContainer Register<TTo>(Action<RegistrationConfigurator<TTo, TTo>> configurator)
        where TTo : class
    {
        return IocContainer.Register<TTo>(configurator);
    }

    /// <inheritdoc />
    public IStashboxContainer Register<TTo>(object? name = null)
        where TTo : class
    {
        return IocContainer.Register<TTo>(name);
    }

    /// <inheritdoc />
    public IStashboxContainer Register(
        Type typeTo,
        Action<RegistrationConfigurator>? configurator = null
    )
    {
        return IocContainer.Register(typeTo, configurator);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterSingleton<TFrom, TTo>(object? name = null)
        where TFrom : class
        where TTo : class, TFrom
    {
        return IocContainer.RegisterSingleton<TFrom, TTo>(name);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterSingleton<TTo>(object? name = null)
        where TTo : class
    {
        return IocContainer.RegisterSingleton<TTo>(name);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterSingleton(Type typeFrom, Type typeTo, object? name = null)
    {
        return IocContainer.RegisterSingleton(typeFrom, typeTo, name);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterScoped<TFrom, TTo>(object? name = null)
        where TFrom : class
        where TTo : class, TFrom
    {
        return IocContainer.RegisterScoped<TFrom, TTo>(name);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterScoped(Type typeFrom, Type typeTo, object? name = null)
    {
        return IocContainer.RegisterScoped(typeFrom, typeTo, name);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterScoped<TTo>(object? name = null)
        where TTo : class
    {
        return IocContainer.RegisterScoped<TTo>(name);
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterInstance<TInstance>(
        TInstance instance,
        object? name = null,
        bool withoutDisposalTracking = false,
        Action<TInstance>? finalizerDelegate = null
    )
        where TInstance : class
    {
        return IocContainer.RegisterInstance(
            instance,
            name,
            withoutDisposalTracking,
            finalizerDelegate
        );
    }

    /// <inheritdoc />
    public IStashboxContainer RegisterInstance(
        object instance,
        Type serviceType,
        object? name = null,
        bool withoutDisposalTracking = false
    )
    {
        return IocContainer.RegisterInstance(instance, serviceType, name, withoutDisposalTracking);
    }

    /// <inheritdoc />
    public IStashboxContainer WireUp<TInstance>(
        TInstance instance,
        object? name = null,
        bool withoutDisposalTracking = false,
        Action<TInstance>? finalizerDelegate = null
    )
        where TInstance : class
    {
        return IocContainer.WireUp(instance, name, withoutDisposalTracking, finalizerDelegate);
    }

    /// <inheritdoc />
    public IStashboxContainer WireUp(
        object instance,
        Type serviceType,
        object? name = null,
        bool withoutDisposalTracking = false
    )
    {
        return IocContainer.WireUp(instance, serviceType, name, withoutDisposalTracking);
    }

    /// <inheritdoc />
    public object GetService(Type serviceType)
    {
        return IocContainer.GetService(serviceType);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        return IocContainer.DisposeAsync();
    }
}
