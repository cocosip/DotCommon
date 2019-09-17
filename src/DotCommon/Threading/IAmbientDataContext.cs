namespace DotCommon.Threading
{
    /// <summary>环境数据上下文
    /// </summary>
    public interface IAmbientDataContext
    {
        /// <summary>赋值
        /// </summary>
        /// <param name="key">Key值</param>
        /// <param name="value">值</param>
        void SetData(string key, object value);

        /// <summary>取值
        /// </summary>
        /// <param name="key">Key值</param>
        /// <returns></returns>
        object GetData(string key);
    }
}
