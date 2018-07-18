namespace DotCommon.Http
{
    /// <summary>Http请求对象的参数
    /// </summary>
    public class HttpParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ContentType { get; set; }

        public HttpParameter()
        {

        }

        public HttpParameter(string name, string value, string contentType)
        {
            Name = name;
            Value = value;
            ContentType = contentType;
        }

        public HttpParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
