using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using DotCommon.Json.SystemTextJson.JsonConverters;
using DotCommon.Reflection;
using DotCommon.Timing;

namespace DotCommon.Json.SystemTextJson.Modifiers
{
    /// <summary>
    /// A modifier class for customizing DateTime serialization behavior in JSON
    /// </summary>
    public class DotCommonDateTimeConverterModifier
    {
        /// <summary>
        /// Non-nullable DateTime converter instance
        /// </summary>
        private readonly DotCommonDateTimeConverter _dotCommonDateTimeConverter;

        /// <summary>
        /// Nullable DateTime converter instance
        /// </summary>
        private readonly DotCommonNullableDateTimeConverter _abpNullableDateTimeConverter;

        /// <summary>
        /// Initializes a new instance of the DotCommonDateTimeConverterModifier class
        /// </summary>
        /// <param name="dotCommonDateTimeConverter">Non-nullable DateTime converter</param>
        /// <param name="dotCommonNullableDateTimeConverter">Nullable DateTime converter</param>
        public DotCommonDateTimeConverterModifier(DotCommonDateTimeConverter dotCommonDateTimeConverter, DotCommonNullableDateTimeConverter dotCommonNullableDateTimeConverter)
        {
            _dotCommonDateTimeConverter = dotCommonDateTimeConverter;
            _abpNullableDateTimeConverter = dotCommonNullableDateTimeConverter;
        }

        /// <summary>
        /// Creates a modify action delegate for JsonTypeInfo modification
        /// </summary>
        /// <returns>Returns an Action delegate for modifying JsonTypeInfo</returns>
        public Action<JsonTypeInfo> CreateModifyAction()
        {
            return Modify;
        }

        /// <summary>
        /// Modifies DateTime property converters in JsonTypeInfo
        /// </summary>
        /// <param name="jsonTypeInfo">The JsonTypeInfo object to modify</param>
        private void Modify(JsonTypeInfo jsonTypeInfo)
        {
            // Check if the type has DisableDateTimeNormalizationAttribute, skip processing if marked
            if (ReflectionHelper.GetAttributesOfMemberOrDeclaringType<DisableDateTimeNormalizationAttribute>(jsonTypeInfo.Type).Any())
            {
                return;
            }

            // Iterate through all DateTime properties and set custom converters
            foreach (var property in jsonTypeInfo.Properties.Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?)))
            {
                // Check if property has DisableDateTimeNormalizationAttribute, set custom converter if not marked
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
