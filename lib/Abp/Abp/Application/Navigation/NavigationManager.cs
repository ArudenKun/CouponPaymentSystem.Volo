using Abp.Configuration.Startup;
using Abp.DependencyInjection;
using Abp.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Application.Navigation;

internal class NavigationManager : INavigationManager, ISingletonDependency
{
    public IDictionary<string, MenuDefinition> Menus { get; }

    public MenuDefinition MainMenu => Menus["MainMenu"];

    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationConfiguration _configuration;

    public NavigationManager(
        IServiceProvider serviceProvider,
        INavigationConfiguration configuration
    )
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;

        Menus = new Dictionary<string, MenuDefinition>
        {
            {
                "MainMenu",
                new MenuDefinition(
                    "MainMenu",
                    new LocalizableString("MainMenu", AbpConsts.LocalizationSourceName)
                )
            },
        };
    }

    public void Initialize()
    {
        var context = new NavigationProviderContext(this);

        foreach (var providerType in _configuration.Providers)
        {
            using var provider =
                _serviceProvider.GetRequiredServiceAsDisposable<NavigationProvider>(providerType);
            provider.Service.SetNavigation(context);
        }
    }
}
