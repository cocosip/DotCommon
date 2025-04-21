using System.Text.Json;

namespace DotCommon.Json.SystemTextJson
{
    public class DotCommonSystemTextJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }

        public DotCommonSystemTextJsonSerializerOptions()
        {
            JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };
        }
    }
}
