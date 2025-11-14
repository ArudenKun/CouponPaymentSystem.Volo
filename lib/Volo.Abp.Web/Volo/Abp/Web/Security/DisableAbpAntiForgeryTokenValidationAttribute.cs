namespace Volo.Abp.Web.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
public class DisableAbpAntiForgeryTokenValidationAttribute : Attribute;
