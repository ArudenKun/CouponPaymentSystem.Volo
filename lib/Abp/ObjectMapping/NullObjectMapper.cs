using Abp.Dependency;

namespace Abp.ObjectMapping
{
    public sealed class NullObjectMapper : IObjectMapper, ISingletonDependency
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullObjectMapper Instance { get; } = new();

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            throw new AbpException(
                "Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects."
            );
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new AbpException(
                "Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects."
            );
        }

        public IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source)
        {
            throw new AbpException(
                "Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects."
            );
        }
    }
}
