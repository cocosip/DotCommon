using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DotCommon.Utility
{
    /// <summary>单层Xml,可用于微信xml
    /// </summary>
    public class UnilayerXml
    {
        private readonly SortedDictionary<string, object> _values = new SortedDictionary<string, object>();
        public UnilayerXml()
        {

        }

        public UnilayerXml(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var root = xmlDoc.DocumentElement;
            if (root != null)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    var xe = (XmlElement) node;
                    _values[xe.Name] = xe.InnerText; //获取xml的键值对到WxPayData内部的数据中
                }
            }
        }

        /// <summary>赋值
        /// </summary>
        public void SetValue(string key, object value)
        {
            _values[key] = value;
        }

        /// <summary>取值
        /// </summary>
        public object GetValue(string key)
        {
            object o;
            _values.TryGetValue(key, out o);
            return o;
        }

        /// <summary>判断某个key是否已经赋值
        /// </summary>
        public bool HasValue(string key)
        {
            object o;
            _values.TryGetValue(key, out o);
            return null != o;
        }

        public SortedDictionary<string, object> GetValues()
        {
            return _values;
        }

        /// <summary>转换为xml字符串
        /// </summary>
        public string ToXml()
        {
            var sb = new StringBuilder();
            sb.Append($"<xml>");
            foreach (var kv in _values)
            {
                if (kv.Value is int)
                {
                    sb.Append($"<{kv.Key}>{kv.Value}</{kv.Value}>");
                }
                if (kv.Value is string)
                {
                    sb.Append($"<{kv.Key}><![CDATA[{kv.Value}]]></{kv.Value}>");
                }
            }
            sb.Append($"</xml>");
            return sb.ToString();
        }

    }

}
