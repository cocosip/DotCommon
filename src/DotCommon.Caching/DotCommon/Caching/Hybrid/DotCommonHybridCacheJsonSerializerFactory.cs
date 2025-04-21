using System.Diagnostics.CodeAnalysis;
using DotCommon.Json.SystemTextJson;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace DotCommon.Caching.Hybrid
{

    public class DotCommonHybridCacheJsonSerializerFactory : IHybridCacheSerializerFactory
    {
        protected IOptions<DotCommonSystemTextJsonSerializerOptions> Options { get; }

        public DotCommonHybridCacheJsonSerializerFactory(IOptions<DotCommonSystemTextJsonSerializerOptions> options)
        {
            Options = options;
        }

#if NETSTANDARD2_0
        public bool TryCreateSerializer<T>(out IHybridCacheSerializer<T> serializer)
#else
    public bool TryCreateSerializer<T>([NotNullWhen(true)] out IHybridCacheSerializer<T> serializer)
#endif
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(byte[]))
            {
                // 返回 false 时，serializer 必须为 null
                serializer = null!;  // 使用 null! 抑制编译器警告
                return false;
            }

            // 返回 true 时，serializer 必须非 null
            serializer = new DotCommonHybridCacheJsonSerializer<T>(Options.Value.JsonSerializerOptions);
            return true;
        }
    }
}
