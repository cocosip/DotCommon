using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;

namespace DotCommon.Json.SystemTextJson
{

    public class DotCommonSystemTextJsonSerializerModifiersOptions
    {
        public List<Action<JsonTypeInfo>> Modifiers { get; }

        public DotCommonSystemTextJsonSerializerModifiersOptions()
        {
            Modifiers = [];
        }
    }
}
