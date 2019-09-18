using DotCommon.Serializing;
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
        /// <summary>Json4Net序列化配置
        /// </summary>
        public JsonSerializerSettings Settings { get; }

        /// <summary>Ctor
        /// </summary>
        public NewtonsoftJsonSerializer()
        {
            Settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter() },
                ContractResolver = new CustomContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
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
}
