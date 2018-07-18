namespace DotCommon.Http
{
    /// <summary>参数类型
    /// </summary>
    public enum ParameterType
    {
        Cookie,
        GetOrPost,
        UrlSegment,
        HttpHeader,
        RequestBody,
        QueryString
    }

    /// <summary>数据格式
    /// </summary>
    public enum DataFormat
    {
        Json,
        Xml
    }

    /// <summary>HTTP请求谓词
    /// </summary>
    public enum Method
    {
        GET,
        POST,
        PUT,
        DELETE,
        HEAD,
        OPTIONS,
        PATCH,
        MERGE,
        COPY
    }

    /// <summary>响应状态
    /// </summary>
    public enum ResponseStatus
    {
        None,
        Completed,
        Error,
        TimedOut,
        Aborted
    }
}
