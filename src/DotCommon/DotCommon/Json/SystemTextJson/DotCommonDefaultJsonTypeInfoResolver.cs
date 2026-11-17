using Microsoft.Extensions.Options;
using System.Text.Json.Serialization.Metadata;

namespace DotCommon.Json.SystemTextJson
{
    public class DotCommonDefaultJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        public DotCommonDefaultJsonTypeInfoResolver(IOptions<DotCommonSystemTextJsonSerializerModifiersOptions> options)
        {
            foreach (var modifier in options.Value.Modifiers)
            {
                Modifiers.Add(modifier);
            }
        }
    }
}
