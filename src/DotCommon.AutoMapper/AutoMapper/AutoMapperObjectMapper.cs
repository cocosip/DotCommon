using AutoMapper;
using IObjectMapper = DotCommon.ObjectMapping.IObjectMapper;

namespace DotCommon.AutoMapper
{
    /// <summary>AutoMapper对象映射
    /// </summary>
    public class AutoMapperObjectMapper : IObjectMapper
    {
        private readonly IMapper _mapper;

        /// <summary>Ctor
        /// </summary>
        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>将对象转为为目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>映射后的对象</returns>
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        /// <summary>执行从源对象映射到现有的目标对象
        /// </summary>
        /// <typeparam name="TSource">源对象类型</typeparam>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns>映射后的对象</returns>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
