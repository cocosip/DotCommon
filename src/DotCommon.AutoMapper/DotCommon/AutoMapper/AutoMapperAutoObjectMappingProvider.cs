using DotCommon.ObjectMapping;

namespace DotCommon.AutoMapper
{
    /// <summary>
    /// AutoMapper implementation of <see cref="IAutoObjectMappingProvider{TContext}"/> interface for a specific context
    /// </summary>
    /// <typeparam name="TContext">Mapping context type</typeparam>
    public class AutoMapperAutoObjectMappingProvider<TContext> : AutoMapperAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapperAccessor">Mapper accessor instance</param>
        public AutoMapperAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
            : base(mapperAccessor)
        {
        }
    }

    /// <summary>
    /// AutoMapper implementation of <see cref="IAutoObjectMappingProvider"/> interface
    /// </summary>
    public class AutoMapperAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        /// <summary>
        /// Mapper accessor
        /// </summary>
        public IMapperAccessor MapperAccessor { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapperAccessor">Mapper accessor instance</param>
        public AutoMapperAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
        {
            MapperAccessor = mapperAccessor;
        }

        /// <summary>
        /// Maps the source object to an instance of the destination type
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Destination object</returns>
        public virtual TDestination Map<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Maps the source object to an existing destination object
        /// </summary>
        /// <typeparam name="TSource">Source object type</typeparam>
        /// <typeparam name="TDestination">Destination object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Existing destination object</param>
        /// <returns>Destination object</returns>
        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return MapperAccessor.Mapper.Map(source, destination);
        }
    }
}