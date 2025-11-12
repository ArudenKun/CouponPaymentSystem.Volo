using System.Globalization;

namespace Abp.Localization;

public static class CultureInfoHelper
{
    public static IDisposable Use(string culture, string? uiCulture = null)
    {
        Check.NotNull(culture, nameof(culture));

        return Use(
            CultureInfo.GetCultureInfo(culture),
            uiCulture == null ? null : CultureInfo.GetCultureInfo(uiCulture)
        );
    }

    public static IDisposable Use(CultureInfo culture, CultureInfo? uiCulture = null)
    {
        Check.NotNull(culture, nameof(culture));

        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = uiCulture ?? culture;

        return new DisposeAction(() =>
        {
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentUiCulture;
        });
    }
}
