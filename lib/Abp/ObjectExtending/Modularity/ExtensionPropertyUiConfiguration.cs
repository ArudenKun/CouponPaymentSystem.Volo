using JetBrains.Annotations;

namespace Abp.ObjectExtending.Modularity;

public class ExtensionPropertyUiConfiguration
{
    public const int DefaultOrder = 1000;

    public ExtensionPropertyUiTableConfiguration OnTable { get; }

    public ExtensionPropertyUiFormConfiguration OnCreateForm { get; }

    public ExtensionPropertyUiFormConfiguration OnEditForm { get; }

    public ExtensionPropertyLookupConfiguration Lookup { get; set; }

    public int Order { get; set; }

    public ExtensionPropertyUiConfiguration()
    {
        OnTable = new ExtensionPropertyUiTableConfiguration();
        OnCreateForm = new ExtensionPropertyUiFormConfiguration();
        OnEditForm = new ExtensionPropertyUiFormConfiguration();
        Lookup = new ExtensionPropertyLookupConfiguration();
        Order = DefaultOrder;
    }
}
