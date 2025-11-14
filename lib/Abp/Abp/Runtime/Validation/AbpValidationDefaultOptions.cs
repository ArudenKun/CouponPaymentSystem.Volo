using Abp.Application.Services;

namespace Abp.Runtime.Validation;

public class AbpValidationDefaultOptions : IAbpValidationDefaultOptions
{
    public static List<Func<Type, bool>> ConventionalValidationSelectorList = new()
    {
        type => typeof(IApplicationService).IsAssignableFrom(type),
    };

    public List<Func<Type, bool>> ConventionalValidationSelectors { get; }

    public AbpValidationDefaultOptions()
    {
        ConventionalValidationSelectors = ConventionalValidationSelectorList.ToList();
    }
}
