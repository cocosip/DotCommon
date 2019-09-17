namespace DotCommon.DependencyInjection
{
    /// <summary>对象存取器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        /// <summary>对象值
        /// </summary>
        public T Value { get; set; }

        /// <summary>Ctor
        /// </summary>
        public ObjectAccessor()
        {

        }

        /// <summary>Ctor
        /// </summary>
        public ObjectAccessor(T value)
        {
            Value = value;
        }
    }
}
