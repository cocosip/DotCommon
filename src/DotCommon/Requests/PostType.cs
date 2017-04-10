namespace DotCommon.Requests
{
    /// <summary>Post请求的四种头部类型枚举
    /// </summary>
    public enum PostType
    {
        /// <summary>form表单的形式
        /// </summary>
        FormUrlEncoded = 1,

        /// <summary>多部件
        /// </summary>
        Multipart = 2,

        /// <summary>Json头
        /// </summary>
        Json = 3,

        /// <summary>Xml头
        /// </summary>
        Xml = 4
    }
}
