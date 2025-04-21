using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using DotCommon.Json.SystemTextJson.JsonConverters;
using DotCommon.Reflection;
using DotCommon.Timing;

namespace DotCommon.Json.SystemTextJson.Modifiers
{
    public class DotCommonDateTimeConverterModifier
    {
        private readonly DotCommonDateTimeConverter _dotCommonDateTimeConverter;
        private readonly DotCommonNullableDateTimeConverter _abpNullableDateTimeConverter;

        public DotCommonDateTimeConverterModifier(DotCommonDateTimeConverter dotCommonDateTimeConverter, DotCommonNullableDateTimeConverter dotCommonNullableDateTimeConverter)
        {
            _dotCommonDateTimeConverter = dotCommonDateTimeConverter;
            _abpNullableDateTimeConverter = dotCommonNullableDateTimeConverter;
        }

        public Action<JsonTypeInfo> CreateModifyAction()
        {
            return Modify;
        }

        private void Modify(JsonTypeInfo jsonTypeInfo)
        {
            if (ReflectionHelper.GetAttributesOfMemberOrDeclaringType<DisableDateTimeNormalizationAttribute>(jsonTypeInfo.Type).Any())
            {
                return;
            }

            foreach (var property in jsonTypeInfo.Properties.Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?)))
            {
                if (property.AttributeProvider == null ||
                    !property.AttributeProvider.GetCustomAttributes(typeof(DisableDateTimeNormalizationAttribute), false).Any())
                {
                    property.CustomConverter = property.PropertyType == typeof(DateTime)
                        ? _dotCommonDateTimeConverter
                        : _abpNullableDateTimeConverter;
                }
            }
        }
    }
}
