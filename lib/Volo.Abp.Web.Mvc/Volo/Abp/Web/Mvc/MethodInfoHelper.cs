using System.Reflection;
using System.Web.Mvc;

namespace Volo.Abp.Web.Mvc;

internal static class MethodInfoHelper
{
    public static bool IsJsonResult(MethodInfo? method)
    {
        if (method is null)
            return false;

        return typeof(JsonResult).IsAssignableFrom(method.ReturnType)
            || typeof(Task<JsonResult>).IsAssignableFrom(method.ReturnType);
    }
}
