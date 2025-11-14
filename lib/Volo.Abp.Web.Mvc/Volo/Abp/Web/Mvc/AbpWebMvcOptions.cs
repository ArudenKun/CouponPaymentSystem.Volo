using Volo.Abp.Uow;
using Volo.Abp.Web.Models;

namespace Volo.Abp.Web.Mvc;

public class AbpWebMvcOptions
{
    public AbpWebMvcOptions()
    {
        DefaultUnitOfWorkAttribute = new UnitOfWorkAttribute();
        DefaultWrapResultAttribute = new WrapResultAttribute();
        IsValidationEnabledForControllers = true;
        IsAutomaticAntiForgeryValidationEnabled = true;
        IsAuditingEnabled = true;
    }

    public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

    public WrapResultAttribute DefaultWrapResultAttribute { get; }

    public bool IsValidationEnabledForControllers { get; set; }

    public bool IsAutomaticAntiForgeryValidationEnabled { get; set; }

    public bool IsAuditingEnabled { get; set; }

    public bool IsAuditingEnabledForChildActions { get; set; }
}
