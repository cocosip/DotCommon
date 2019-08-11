namespace DotCommon.Http
{
    /// <summary>请求参数类型
    /// </summary>
    public enum ParameterType
    {
        /// <summary>Cookie
        /// </summary>
        Cookie,

        /// <summary>Get或者Post请求
        /// </summary>
        GetOrPost,

        /// <summary>Url片段
        /// </summary>
        UrlSegment,

        /// <summary>Http请求头部
        /// </summary>
        HttpHeader,

        /// <summary>请求Body
        /// </summary>
        RequestBody,

        /// <summary>请求参数
        /// </summary>
        QueryString,

        /// <summary>请求参数不进行Encode
        /// </summary>
        QueryStringWithoutEncode
    }

    /// <summary>数据格式
    /// </summary>
    public enum DataFormat
    {
        /// <summary>Json
        /// </summary>
        Json,
        /// <summary>Xml
        /// </summary>
        Xml,
        /// <summary>无
        /// </summary>
        None
    }

    /// <summary>请求方法
    /// </summary>
    public enum Method
    {
        /// <summary>GET
        /// </summary>
        GET,

        /// <summary>POST
        /// </summary>
        POST,

        /// <summary>PUT
        /// </summary>
        PUT,

        /// <summary>DELETE
        /// </summary>
        DELETE,

        /// <summary>HEAD
        /// </summary>
        HEAD,

        /// <summary>OPTIONS
        /// </summary>
        OPTIONS,

        /// <summary>PATCH
        /// </summary>
        PATCH,

        /// <summary>MERGE
        /// </summary>
        MERGE,

        /// <summary>COPY
        /// </summary>
        COPY
    }
}
