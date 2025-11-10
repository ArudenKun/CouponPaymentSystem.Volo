using System.Security.Claims;
using System.Web;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Web.Session;

internal class HttpContextPrincipalAccessor : CurrentPrincipalAccessorBase
{
    protected override ClaimsPrincipal GetClaimsPrincipal() =>
        HttpContext.Current?.User as ClaimsPrincipal ?? Principal;
}
