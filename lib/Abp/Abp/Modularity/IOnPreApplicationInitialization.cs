namespace Abp.Modularity;

public interface IOnPreApplicationInitialization
{
    Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context);

    void OnPreApplicationInitialization(ApplicationInitializationContext context);
}
