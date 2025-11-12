using System.Collections.Immutable;
using Abp.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Abp.Localization;

internal class DefaultLanguageProvider : ILanguageProvider, ITransientDependency
{
    private readonly AbpLocalizationOptions _options;

    public DefaultLanguageProvider(IOptions<AbpLocalizationOptions> options)
    {
        _options = options.Value;
    }

    public IReadOnlyList<LanguageInfo> GetLanguages()
    {
        return _options.Languages.ToImmutableList();
    }

    public IReadOnlyList<LanguageInfo> GetActiveLanguages()
    {
        return _options.Languages.Where(l => !l.IsDisabled).ToImmutableList();
    }
}
