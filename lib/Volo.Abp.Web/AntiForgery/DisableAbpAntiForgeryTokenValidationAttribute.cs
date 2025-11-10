namespace Volo.Abp.Web.AntiForgery;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
public class DisableAbpAntiForgeryTokenValidationAttribute : Attribute;
