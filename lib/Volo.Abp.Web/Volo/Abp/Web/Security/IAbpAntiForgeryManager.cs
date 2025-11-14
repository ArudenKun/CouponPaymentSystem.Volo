namespace Volo.Abp.Web.Security;

public interface IAbpAntiForgeryManager
{
    AbpAntiForgeryOptions Options { get; }

    string GenerateToken();
}
