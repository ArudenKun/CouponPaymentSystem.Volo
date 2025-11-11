namespace Abp.Modularity;

public interface IPostConfigureServices
{
    Task PostConfigureServicesAsync(ServiceConfigurationContext context);

    void PostConfigureServices(ServiceConfigurationContext context);
}
