namespace Abp.ObjectExtending.Modularity;

public class ModuleExtensionConfiguration
{
    public EntityExtensionConfigurationDictionary Entities { get; }

    public Dictionary<string, object> Configuration { get; }

    public ModuleExtensionConfiguration()
    {
        Entities = new EntityExtensionConfigurationDictionary();
        Configuration = new Dictionary<string, object>();
    }
}
