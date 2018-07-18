using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Http
{
    /// <summary>Request请求数据
    /// </summary>
    public class Parameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ParameterType Type { get; set; }

        public string ContentType { get; set; }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
