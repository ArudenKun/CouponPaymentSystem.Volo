using System.Net.Http;
using System.Reflection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Web.Security;

public static class AbpAntiForgeryManagerExtensions
{
    public static bool ShouldValidate(
        this IAbpAntiForgeryManager _,
        AbpWebAntiForgeryOptions abpWebAntiForgeryOptions,
        MethodInfo methodInfo,
        HttpMethod httpMethod,
        bool defaultValue
    )
    {
        if (!abpWebAntiForgeryOptions.IsEnabled)
        {
            return false;
        }

        if (methodInfo.IsDefined(typeof(ValidateAbpAntiForgeryTokenAttribute), true))
        {
            return true;
        }

        if (
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableAbpAntiForgeryTokenValidationAttribute>(
                methodInfo
            ) != null
        )
        {
            return false;
        }

        if (abpWebAntiForgeryOptions.IgnoredHttpMethods.Contains(httpMethod))
        {
            return false;
        }

        if (
            methodInfo.DeclaringType?.IsDefined(typeof(ValidateAbpAntiForgeryTokenAttribute), true)
            ?? false
        )
        {
            return true;
        }

        return defaultValue;
    }
}
