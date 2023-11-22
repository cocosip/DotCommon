//using Microsoft.Extensions.DependencyInjection;
//using System;

//namespace DotCommon.Serializing
//{
//    /// <summary>
//    /// 对象序列化
//    /// </summary>
//    public class DefaultObjectSerializer : IObjectSerializer
//    {
//        private readonly IServiceProvider _provider;
//        private readonly IJsonSerializer _jsonSerializer;

//        /// <summary>
//        /// Ctor
//        /// </summary>
//        /// <param name="provider"></param>
//        /// <param name="jsonSerializer"></param>
//        public DefaultObjectSerializer(IServiceProvider provider, IJsonSerializer jsonSerializer)
//        {
//            _provider = provider;
//            _jsonSerializer = jsonSerializer;
//        }

//        /// <summary>
//        /// 对象序列化
//        /// </summary>
//        public virtual byte[] Serialize<T>(T o)
//        {
//            if (o == null)
//            {
//                return null;
//            }

//            using (var scope = _provider.CreateScope())
//            {
//                var specificSerializer = scope.ServiceProvider.GetService<IObjectSerializer<T>>();
//                if (specificSerializer != null)
//                {
//                    return specificSerializer.Serialize(o);
//                }
//            }
//            return AutoSerialize(o);
//        }

//        /// <summary>
//        /// 对象反序列化
//        /// </summary>
//        public virtual T Deserialize<T>(byte[] bytes)
//        {
//            if (bytes == null)
//            {
//                return default;
//            }

//            using (var scope = _provider.CreateScope())
//            {
//                var specificSerializer = scope.ServiceProvider.GetService<IObjectSerializer<T>>();
//                if (specificSerializer != null)
//                {
//                    return specificSerializer.Deserialize(bytes);
//                }
//            }

//            return AutoDeserialize<T>(bytes);
//        }

//        /// <summary>自动序列化
//        /// </summary>
//        protected virtual byte[] AutoSerialize<T>(T o)
//        {
//            return _jsonSerializer.Serialize(o);
//        }

//        /// <summary>
//        /// 自动反序列化
//        /// </summary>
//        protected virtual T AutoDeserialize<T>(byte[] bytes)
//        {
//            return _jsonSerializer.Deserialize<T>(bytes);
//        }
//    }
//}
