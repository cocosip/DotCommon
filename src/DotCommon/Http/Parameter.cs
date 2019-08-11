namespace DotCommon.Http
{
    /// <summary>请求参数
    /// </summary>
    public class Parameter
    {
        /// <summary>Ctor
        /// </summary>
        public Parameter()
        {

        }

        /// <summary>Ctor
        /// </summary>
        public Parameter(string name, object value, ParameterType type)
        {

            Name = name;
            Value = value;
            Type = type;
        }

        /// <summary>Ctor
        /// </summary>
        public Parameter(string name, object value, string contentType, ParameterType type) : this(name, value, type)
        {
            ContentType = contentType;
        }

        /// <summary>参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>参数类型
        /// </summary>
        public ParameterType Type { get; set; }

        /// <summary>格式化
        /// </summary>
        public DataFormat DataFormat { get; set; } = DataFormat.None;

        /// <summary>MIME content type of the parameter
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Return a human-readable representation of this parameter
        /// </summary>
        /// <returns>String</returns>
        public override string ToString() => $"{Name}={Value}";
    }
}
