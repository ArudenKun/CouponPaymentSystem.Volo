using System.Reflection;

namespace Abp.Domain.Uow;

internal static class UnitOfWorkDefaultOptionsExtensions
{
    public static UnitOfWorkAttribute? GetUnitOfWorkAttributeOrNull(
        this UnitOfWorkDefaultOptions unitOfWorkDefaultOptions,
        MethodInfo methodInfo
    )
    {
        var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
        if (attrs.Length > 0)
        {
            return attrs[0];
        }

        attrs = methodInfo
            .DeclaringType.GetTypeInfo()
            .GetCustomAttributes(true)
            .OfType<UnitOfWorkAttribute>()
            .ToArray();
        if (attrs.Length > 0)
        {
            return attrs[0];
        }

        if (unitOfWorkDefaultOptions.IsConventionalUowClass(methodInfo.DeclaringType))
        {
            return new UnitOfWorkAttribute(); //Default
        }

        return null;
    }

    public static bool IsConventionalUowClass(
        this UnitOfWorkDefaultOptions unitOfWorkDefaultOptions,
        Type? type
    )
    {
        if (type is null)
        {
            return false;
        }

        return unitOfWorkDefaultOptions.ConventionalUowSelectors.Any(selector => selector(type));
    }
}
