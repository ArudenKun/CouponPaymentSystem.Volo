using System.Globalization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Abp.BackgroundJobs;

public abstract class BackgroundJobBase<TArgs> : IBackgroundJobBase<TArgs>
{
    /// <summary>
    /// Reference to the setting manager.
    /// </summary>
    public ISettingManager SettingManager { protected get; set; }

    /// <summary>
    /// Reference to <see cref="IUnitOfWorkManager"/>.
    /// </summary>
    public IUnitOfWorkManager UnitOfWorkManager
    {
        get
        {
            if (field == null)
            {
                throw new AbpException("Must set UnitOfWorkManager before use it.");
            }

            return field;
        }
        set;
    }

    /// <summary>
    /// Gets current unit of work.
    /// </summary>
    protected IActiveUnitOfWork CurrentUnitOfWork
    {
        get { return UnitOfWorkManager.Current; }
    }

    /// <summary>
    /// Reference to the localization manager.
    /// </summary>
    public ILocalizationManager LocalizationManager { protected get; set; }

    /// <summary>
    /// Gets/sets name of the localization source that is used in this application service.
    /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
    /// </summary>
    protected string LocalizationSourceName { get; set; }

    /// <summary>
    /// Gets localization source.
    /// It's valid if <see cref="LocalizationSourceName"/> is set.
    /// </summary>
    protected ILocalizationSource LocalizationSource
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
    /// Reference to the logger to write logs.
    /// </summary>
    public ILogger<BackgroundJobBase<TArgs>> Logger { protected get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    protected BackgroundJobBase()
    {
        Logger = NullLogger<BackgroundJobBase<TArgs>>.Instance;
        LocalizationManager = NullLocalizationManager.Instance;
    }

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
