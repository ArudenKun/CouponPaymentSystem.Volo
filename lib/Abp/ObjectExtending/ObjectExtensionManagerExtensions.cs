using System.Collections.Immutable;
using Abp.ObjectExtending.Data;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.ObjectExtending;

public static class ObjectExtensionManagerExtensions
{
    public static ObjectExtensionManager AddOrUpdateProperty<TProperty>(
        this ObjectExtensionManager objectExtensionManager,
        Type[] objectTypes,
        string propertyName,
        Action<ObjectExtensionPropertyInfo>? configureAction = null
    )
    {
        return objectExtensionManager.AddOrUpdateProperty(
            objectTypes,
            typeof(TProperty),
            propertyName,
            configureAction
        );
    }

    public static ObjectExtensionManager AddOrUpdateProperty<TObject, TProperty>(
        this ObjectExtensionManager objectExtensionManager,
        string propertyName,
        Action<ObjectExtensionPropertyInfo>? configureAction = null
    )
        where TObject : IHasExtraProperties
    {
        return objectExtensionManager.AddOrUpdateProperty(
            typeof(TObject),
            typeof(TProperty),
            propertyName,
            configureAction
        );
    }

    public static ObjectExtensionManager AddOrUpdateProperty(
        this ObjectExtensionManager objectExtensionManager,
        Type[] objectTypes,
        Type propertyType,
        string propertyName,
        Action<ObjectExtensionPropertyInfo>? configureAction = null
    )
    {
        Check.NotNull(objectTypes, nameof(objectTypes));

        foreach (var objectType in objectTypes)
        {
            objectExtensionManager.AddOrUpdateProperty(
                objectType,
                propertyType,
                propertyName,
                configureAction
            );
        }

        return objectExtensionManager;
    }

    public static ObjectExtensionManager AddOrUpdateProperty(
        this ObjectExtensionManager objectExtensionManager,
        Type objectType,
        Type propertyType,
        string propertyName,
        Action<ObjectExtensionPropertyInfo>? configureAction = null
    )
    {
        Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

        return objectExtensionManager.AddOrUpdate(
            objectType,
            options =>
            {
                options.AddOrUpdateProperty(propertyType, propertyName, configureAction);
            }
        );
    }

    public static ObjectExtensionPropertyInfo? GetPropertyOrNull<TObject>(
        this ObjectExtensionManager objectExtensionManager,
        string propertyName
    )
    {
        return objectExtensionManager.GetPropertyOrNull(typeof(TObject), propertyName);
    }

    public static ObjectExtensionPropertyInfo? GetPropertyOrNull(
        this ObjectExtensionManager objectExtensionManager,
        Type objectType,
        string propertyName
    )
    {
        Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
        Check.NotNull(objectType, nameof(objectType));
        Check.NotNull(propertyName, nameof(propertyName));

        return objectExtensionManager.GetOrNull(objectType)?.GetPropertyOrNull(propertyName);
    }

    private static readonly ImmutableList<ObjectExtensionPropertyInfo> EmptyPropertyList =
        new List<ObjectExtensionPropertyInfo>().ToImmutableList();

    public static ImmutableList<ObjectExtensionPropertyInfo> GetProperties<TObject>(
        this ObjectExtensionManager objectExtensionManager
    )
    {
        return objectExtensionManager.GetProperties(typeof(TObject));
    }

    public static ImmutableList<ObjectExtensionPropertyInfo> GetProperties(
        this ObjectExtensionManager objectExtensionManager,
        Type objectType
    )
    {
        Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
        Check.NotNull(objectType, nameof(objectType));

        var extensionInfo = objectExtensionManager.GetOrNull(objectType);
        if (extensionInfo == null)
        {
            return EmptyPropertyList;
        }

        return extensionInfo.GetProperties();
    }

    public static Task<
        ImmutableList<ObjectExtensionPropertyInfo>
    > GetPropertiesAndCheckPolicyAsync<TObject>(
        this ObjectExtensionManager objectExtensionManager,
        IServiceProvider serviceProvider
    )
    {
        return objectExtensionManager.GetPropertiesAndCheckPolicyAsync(
            typeof(TObject),
            serviceProvider
        );
    }

    public static async Task<
        ImmutableList<ObjectExtensionPropertyInfo>
    > GetPropertiesAndCheckPolicyAsync(
        this ObjectExtensionManager objectExtensionManager,
        Type objectType,
        IServiceProvider serviceProvider
    )
    {
        Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
        Check.NotNull(objectType, nameof(objectType));
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        var extensionPropertyPolicyConfigurationChecker =
            serviceProvider.GetRequiredService<ExtensionPropertyPolicyChecker>();
        var properties = new List<ObjectExtensionPropertyInfo>();
        foreach (var propertyInfo in objectExtensionManager.GetProperties(objectType))
        {
            if (
                await extensionPropertyPolicyConfigurationChecker.CheckPolicyAsync(
                    propertyInfo.Policy
                )
            )
            {
                properties.Add(propertyInfo);
            }
        }

        return properties.ToImmutableList();
    }
}
