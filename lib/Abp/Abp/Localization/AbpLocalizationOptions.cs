namespace Abp.Localization;

public class AbpLocalizationOptions
{
    public IList<LanguageInfo> Languages { get; } = [];

    public ILocalizationSourceList Sources { get; } = new LocalizationSourceList();

    public bool IsEnabled { get; set; } = true;

    public bool ReturnGivenTextIfNotFound { get; set; } = true;

    public bool WrapGivenTextIfNotFound { get; set; } = true;

    public bool HumanizeTextIfNotFound { get; set; } = true;

    public bool LogWarnMessageIfNotFound { get; set; } = true;
}
