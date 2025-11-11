namespace Abp.DependencyInjection;

public interface IServiceProviderAccessor
{
    IServiceProvider ServiceProvider { get; }
}
