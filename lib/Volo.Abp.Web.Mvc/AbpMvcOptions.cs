using Volo.Abp.Uow;

namespace Volo.Abp.Web.Mvc;

public class AbpMvcOptions
{
    public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; } = new();

    public bool IsValidationEnabledForControllers { get; set; } = true;

    public bool IsAutomaticAntiForgeryValidationEnabled { get; set; } = true;

    public bool IsAuditingEnabled { get; set; } = true;

    public bool IsAuditingEnabledForChildActions { get; set; }
}
