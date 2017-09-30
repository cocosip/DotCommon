using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotCommon.Serializing
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public JsonSerializerSettings Settings { get; }

        public NewtonsoftJsonSerializer()
        {
            Settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter() },
                ContractResolver = new CustomContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        public string Serialize(object obj)
        {
            return obj == null ? null : JsonConvert.SerializeObject(obj, Settings);
        }


        public object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, Settings);
        }

        public T Deserialize<T>(string value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(JObject.Parse(value).ToString(), Settings);
        }

        class CustomContractResolver : DefaultContractResolver
        {
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
