using System.Security.Claims;
using System.Web;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Web.Session;

public class HttpContextPrincipalAccessor : ThreadCurrentPrincipalAccessor
{
    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        if (HttpContext.Current.User is ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal;
        }

        return base.GetClaimsPrincipal();
    }
}
