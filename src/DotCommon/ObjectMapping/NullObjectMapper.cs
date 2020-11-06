using System;

namespace DotCommon.ObjectMapping
{
    /// <summary>
    /// 对象映射
    /// </summary>
    public sealed class NullObjectMapper : IObjectMapper
    {
        /// <summary>
        /// 默认实例
        /// </summary>
        public static NullObjectMapper Instance { get; } = new NullObjectMapper();

        /// <summary>将对象转为为目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>映射后的对象</returns>
        public TDestination Map<TDestination>(object source)
        {
            throw new ArgumentException("ObjectMapping.IObjectMapper should be implemented in order to map objects.");
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
            throw new ArgumentException("ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }
    }
}
