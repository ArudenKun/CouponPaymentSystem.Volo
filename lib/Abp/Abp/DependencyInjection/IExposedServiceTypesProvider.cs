namespace Abp.DependencyInjection;

public interface IExposedServiceTypesProvider
{
    Type[] GetExposedServiceTypes(Type targetType);
}
