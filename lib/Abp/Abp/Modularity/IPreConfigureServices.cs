namespace Abp.Modularity;

public interface IPreConfigureServices
{
    Task PreConfigureServicesAsync(ServiceConfigurationContext context);

    void PreConfigureServices(ServiceConfigurationContext context);
}
