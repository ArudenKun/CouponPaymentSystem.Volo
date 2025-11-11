namespace Abp.ObjectExtending.Modularity;

public class ExtensionPropertyApiConfiguration
{
    public ExtensionPropertyApiGetConfiguration OnGet { get; }

    public ExtensionPropertyApiCreateConfiguration OnCreate { get; }

    public ExtensionPropertyApiUpdateConfiguration OnUpdate { get; }

    public ExtensionPropertyApiConfiguration()
    {
        OnGet = new ExtensionPropertyApiGetConfiguration();
        OnCreate = new ExtensionPropertyApiCreateConfiguration();
        OnUpdate = new ExtensionPropertyApiUpdateConfiguration();
    }
}
