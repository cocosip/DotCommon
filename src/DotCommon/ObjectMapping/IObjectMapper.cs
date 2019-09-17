namespace DotCommon.ObjectMapping
{
    /// <summary>对象映射
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>将对象转为为目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>映射后的对象</returns>
        TDestination Map<TDestination>(object source);

        /// <summary>执行从源对象映射到现有的目标对象
        /// </summary>
        /// <typeparam name="TSource">源对象类型</typeparam>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns>映射后的对象</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
