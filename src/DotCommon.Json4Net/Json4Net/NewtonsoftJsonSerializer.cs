using DotCommon.Serializing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotCommon.Json4Net
{
    /// <summary>Json4Net序列化
    /// </summary>
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        /// <summary>Json4Net配置信息
        /// </summary>
        public JsonSerializerSettings Settings { get; private set; }

        /// <summary>Ctor
        /// </summary>
        public NewtonsoftJsonSerializer(IOptions<JsonSerializerSettings> settings)
        {
            Settings = settings.Value;
        }

        /// <summary>序列化对象
        /// </summary>
        public string Serialize(object obj)
        {
            return obj == null ? null : JsonConvert.SerializeObject(obj, Settings);
        }


        /// <summary>反序列化对象
        /// </summary>
        public object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, Settings);
        }


        /// <summary>反序列化对象
        /// </summary>
        public T Deserialize<T>(string value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(JObject.Parse(value).ToString(), Settings);
        }
    }

    /// <summary>自定义解析器
    /// </summary>
    public class CustomContractResolver : DefaultContractResolver
    {
        /// <summary>创建属性
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty.Writable)
            {
                return jsonProperty;
            }
            var property = member as PropertyInfo;
            if (property == null)
            {
                return jsonProperty;
            }
            var hasPrivateSetter = property.GetSetMethod(true) != null;
            jsonProperty.Writable = hasPrivateSetter;
            return jsonProperty;
        }
    }
}
