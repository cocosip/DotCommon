namespace DotCommon.DependencyInjection
{
    /// <summary>对象存取器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectAccessor<out T>
    {
        /// <summary>对象值
        /// </summary>
        T Value { get; }
    }
}
