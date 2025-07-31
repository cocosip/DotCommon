using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DotCommon.Utility
{
    /// <summary>
    /// Represents a simple, single-level XML document, commonly used in APIs like WeChat Pay.
    /// </summary>
    public class UnilayerXml
    {
        private string _rootNodeName = "xml";
        private readonly SortedDictionary<string, object> _values = new SortedDictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnilayerXml"/> class.
        /// </summary>
        public UnilayerXml()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnilayerXml"/> class by parsing an XML string.
        /// </summary>
        /// <param name="xml">The XML string to parse.</param>
        public UnilayerXml(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var root = xmlDoc.DocumentElement;
            if (root != null)
            {
                _rootNodeName = root.Name;
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node is XmlElement xe)
                    {
                        _values[xe.Name] = xe.InnerText;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the root node name for the generated XML.
        /// </summary>
        /// <param name="rootNodeName">The name of the root node. Defaults to "xml".</param>
        public void SetRootNodeName(string rootNodeName)
        {
            _rootNodeName = rootNodeName;
        }

        /// <summary>
        /// Sets a key-value pair.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to associate with the key.</param>
        public void SetValue(string key, object value)
        {
            _values[key] = value;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the key, or null if the key is not found.</returns>
        public object? GetValue(string key)
        {
            if (_values.TryGetValue(key, out object? o))
            {
                return o;
            }
            return null;
        }

        /// <summary>
        /// Gets the value associated with the specified key, cast to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The typed value, or the default value for the type if the key is not found.</returns>
        public T? GetValue<T>(string key)
        {
            if (_values.TryGetValue(key, out object? o) && o is T val)
            {
                return val;
            }
            return default(T);
        }

        /// <summary>
        /// Checks if a value has been set for the specified key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if a value exists for the key; otherwise, false.</returns>
        public bool HasValue(string key) => _values.ContainsKey(key);

        /// <summary>
        /// Gets a read-only dictionary of all key-value pairs.
        /// </summary>
        public IReadOnlyDictionary<string, object> GetValues() => _values;

        /// <summary>
        /// Removes the value with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public void RemoveValue(string key) => _values.Remove(key);

        /// <summary>
        /// Removes all keys and values.
        /// </summary>
        public void Clear() => _values.Clear();

        /// <summary>
        /// Converts the stored key-value pairs into an XML string.
        /// </summary>
        /// <returns>An XML string representation.</returns>
        public string ToXml()
        {
            var sb = new StringBuilder();
            sb.Append($"<{_rootNodeName}>");
            foreach (var kvp in _values)
            {
                sb.Append($"<{kvp.Key}>");
                if (kvp.Value is string strValue)
                {
                    sb.Append($"<![CDATA[{strValue}]]>");
                }
                else
                {
                    // For non-string values, convert to string and then XML escape if necessary
                    string valueAsString = kvp.Value?.ToString() ?? string.Empty;
                    // Basic XML escaping for common characters
                    valueAsString = valueAsString.Replace("&", "&amp;")
                                                 .Replace("<", "&lt;")
                                                 .Replace(">", "&gt;")
                                                 .Replace("\"", "&quot;")
                                                 .Replace("'", "&apos;");
                    sb.Append(valueAsString);
                }
                sb.Append($"</{kvp.Key}>");
            }
            sb.Append($"</{_rootNodeName}>");
            return sb.ToString();
        }
    }
}
