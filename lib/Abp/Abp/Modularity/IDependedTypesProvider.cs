namespace Abp.Modularity;

public interface IDependedTypesProvider
{
    Type[] GetDependedTypes();
}
