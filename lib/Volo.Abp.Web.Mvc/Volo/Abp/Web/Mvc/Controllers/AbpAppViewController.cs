using System.Web.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp.Web.Mvc.Controllers;

public class AbpAppViewController : AbpController
{
    [DisableAuditing]
    [DisableValidation]
    [UnitOfWork(IsDisabled = true)]
    public ActionResult Load(string viewUrl)
    {
        if (viewUrl.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(viewUrl));
        }

        return View(viewUrl.EnsureStartsWith('~'));
    }
}
