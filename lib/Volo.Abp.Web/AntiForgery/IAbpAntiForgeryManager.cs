namespace Volo.Abp.Web.AntiForgery;

public interface IAbpAntiForgeryManager
{
    AbpAntiForgeryOptions Options { get; }

    string GenerateToken();
}
