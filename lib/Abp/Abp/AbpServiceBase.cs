using System.Globalization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using JetBrains.Annotations;

namespace Abp;

/// <summary>
/// This class can be used as a base class for services.
/// It has some useful objects property-injected and has some basic methods
/// most of services may need to.
/// </summary>
[PublicAPI]
public abstract class AbpServiceBase
{
    /// <summary>
    /// Reference to the setting manager.
    /// </summary>
    public required ISettingManager SettingManager { protected get; init; }

    /// <summary>
    /// Reference to <see cref="IUnitOfWorkManager"/>.
    /// </summary>
    public required IUnitOfWorkManager UnitOfWorkManager { protected get; init; }

    /// <summary>
    /// Gets current unit of work.
    /// </summary>
    public IActiveUnitOfWork? CurrentUnitOfWork => UnitOfWorkManager.Current;

    /// <summary>
    /// Reference to the localization manager.
    /// </summary>
    public required ILocalizationManager LocalizationManager { protected get; init; }

    /// <summary>
    /// Gets/sets name of the localization source that is used in this application service.
    /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
    /// </summary>
    public required string LocalizationSourceName { protected get; init; }

    /// <summary>
    /// Gets localization source.
    /// It's valid if <see cref="LocalizationSourceName"/> is set.
    /// </summary>
    public ILocalizationSource LocalizationSource
    {
        get
        {
            if (LocalizationSourceName == null)
            {
                throw new AbpException(
                    "Must set LocalizationSourceName before, in order to get LocalizationSource"
                );
            }

            if (field == null || field.Name != LocalizationSourceName)
            {
                field = LocalizationManager.GetSource(LocalizationSourceName);
            }

            return field;
        }
    }

    /// <summary>
    /// Reference to the object to object mapper.
    /// </summary>
    public required IObjectMapper ObjectMapper { protected get; init; }

    /// <summary>
    /// Gets localized string for given key name and current language.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name)
    {
        return LocalizationSource.GetString(name);
    }

    /// <summary>
    /// Gets localized string for given key name and current language with formatting strings.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <param name="args">Format arguments</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name, params object[] args)
    {
        return LocalizationSource.GetString(name, args);
    }

    /// <summary>
    /// Gets localized string for given key name and specified culture information.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <param name="culture">culture information</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name, CultureInfo culture)
    {
        return LocalizationSource.GetString(name, culture);
    }

    /// <summary>
    /// Gets localized string for given key name and current language with formatting strings.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <param name="culture">culture information</param>
    /// <param name="args">Format arguments</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name, CultureInfo culture, params object[] args)
    {
        return LocalizationSource.GetString(name, culture, args);
    }
}
