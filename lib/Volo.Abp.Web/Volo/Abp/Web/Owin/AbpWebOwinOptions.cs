namespace Volo.Abp.Web.Owin;

public class AbpWebOwinOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool UseEmbeddedFiles { get; set; }

    internal bool UseAbpSet { get; set; }

    public AbpWebOwinOptions()
    {
        UseEmbeddedFiles = true;
        UseAbpSet = false;
    }
}
