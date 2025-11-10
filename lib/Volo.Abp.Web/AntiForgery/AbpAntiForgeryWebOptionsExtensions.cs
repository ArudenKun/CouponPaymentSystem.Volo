using System.Reflection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Web.AntiForgery;

public static class AbpAntiForgeryWebOptionsExtensions
{
    public static bool ShouldValidate(
        this IAbpAntiForgeryManager _,
        AbpAntiForgeryWebOptions options,
        MethodInfo methodInfo,
        HttpVerb httpVerb,
        bool defaultValue
    )
    {
        if (!options.IsEnabled)
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

        if (options.IgnoredHttpVerbs.Contains(httpVerb))
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
