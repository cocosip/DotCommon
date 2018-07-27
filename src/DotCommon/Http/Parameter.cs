namespace DotCommon.Http
{
    /// <summary>HttpRequest中使用的参数
    /// </summary>
    public class Parameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ParameterType Type { get; set; }

        /// <summary>MIME
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }
    }
}
