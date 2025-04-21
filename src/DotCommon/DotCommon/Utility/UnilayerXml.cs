using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DotCommon.Utility
{
    /// <summary>
    /// 单层Xml,可用于微信xml
    /// </summary>
    public class UnilayerXml
    {
        private string _rootNode = "xml";
        private readonly SortedDictionary<string, object> _values = new SortedDictionary<string, object>();
        
        /// <summary>
        /// ctor
        /// </summary>
        public UnilayerXml()
        {

        }

        /// <summary>ctor
        /// </summary>
        /// <param name="xml">xml字符串</param>
        public UnilayerXml(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var root = xmlDoc.DocumentElement;
            if (root != null)
            {
                //根节点
                _rootNode = root.Name;
                foreach (XmlNode node in root.ChildNodes)
                {
                    var xe = (XmlElement)node;
                    _values[xe.Name] = xe.InnerText;
                }
            }
        }

        /// <summary>
        /// 设置跟节点,默认为xml
        /// </summary>
        public void SetRootNode(string rootNode)
        {
            _rootNode = rootNode;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        public void SetValue(string key, object value)
        {
            _values[key] = value;
        }

        /// <summary>
        /// 取值
        /// </summary>
        public object GetValue(string key)
        {
            _values.TryGetValue(key, out object o);
            return o;
        }

        /// <summary>
        /// 判断某个key是否已经赋值
        /// </summary>
        public bool HasValue(string key)
        {
            _values.TryGetValue(key, out object o);
            return null != o;
        }

        /// <summary>
        /// 获取根节点下全部的值
        /// </summary>
        public SortedDictionary<string, object> GetValues()
        {
            return _values;
        }

        /// <summary>
        /// 转换为xml字符串
        /// </summary>
        public string ToXml()
        {
            var sb = new StringBuilder();
            sb.Append($"<{_rootNode}>");
            foreach (var kv in _values)
            {
                if (kv.Value is int)
                {
                    sb.Append($"<{kv.Key}>{kv.Value}</{kv.Key}>");
                }
                if (kv.Value is string)
                {
                    sb.Append($"<{kv.Key}><![CDATA[{kv.Value}]]></{kv.Key}>");
                }
            }
            sb.Append($"</{_rootNode}>");
            return sb.ToString();
        }

    }

}
