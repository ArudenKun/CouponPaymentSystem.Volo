namespace Abp.DependencyInjection;

public interface IExposedKeyedServiceTypesProvider
{
    ServiceIdentifier[] GetExposedServiceTypes(Type targetType);
}
