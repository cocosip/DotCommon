using DotCommon.Serializing;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace DotCommon.TextJson
{
    /// <summary>
    /// System.Text.Json序列化与反序列化
    /// </summary>
    public class TextJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// System.Text.Json序列化配置
        /// </summary>
        public JsonSerializerOptions Options { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="options"></param>
        public TextJsonSerializer(IOptions<JsonSerializerOptions> options)
        {
            Options = options.Value;
        }


        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string Serialize(object o)
        {
            return JsonSerializer.Serialize(o, Options);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Deserialize(string value, Type type)
        {
            return JsonSerializer.Deserialize(value, type, Options);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Deserialize<T>(string value) where T : class
        {
            return JsonSerializer.Deserialize<T>(value, Options);
        }


    }

}
