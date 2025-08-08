using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Provides high-performance object-dictionary conversion using IL emit
    /// </summary>
    public static class EmitMapper
    {
        #region Private Fields

        /// <summary>
        /// Cache for object to dictionary conversion delegates
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Delegate> ObjectToDictionaryCache =
            new ConcurrentDictionary<Type, Delegate>();

        /// <summary>
        /// Cache for dictionary to object conversion delegates
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Delegate> DictionaryToObjectCache =
            new ConcurrentDictionary<Type, Delegate>();

        /// <summary>
        /// Cached reflection info for dictionary operations
        /// </summary>
        private static readonly ReflectionCache ReflectionInfo = new ReflectionCache();

        #endregion

        #region Public Methods - Dictionary to Object Conversion

        /// <summary>
        /// Converts a Dictionary&lt;string, string&gt; to the specified object type
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <param name="dictionary">The source dictionary</param>
        /// <returns>The converted object instance, or null if conversion fails</returns>
        /// <exception cref="ArgumentNullException">Thrown when dictionary is null</exception>
        /// <exception cref="NotSupportedException">Thrown when T implements ICollection</exception>
        public static T? DictionaryToObject<T>(Dictionary<string, string> dictionary) where T : class, new()
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            var converter = GetDictionaryToObjectConverter<T>();
            return converter(dictionary);
        }

        /// <summary>
        /// Gets or creates a cached converter function for dictionary to object conversion
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <returns>A converter function</returns>
        /// <exception cref="NotSupportedException">Thrown when T implements ICollection</exception>
        public static Func<Dictionary<string, string>, T> GetDictionaryToObjectConverter<T>() where T : class, new()
        {
            var type = typeof(T);
            ValidateType(type);

            return (Func<Dictionary<string, string>, T>)DictionaryToObjectCache.GetOrAdd(type, 
                _ => CreateDictionaryToObjectConverter<T>());
        }

        #endregion

        #region Public Methods - Object to Dictionary Conversion

        /// <summary>
        /// Converts an object to a Dictionary&lt;string, string&gt;
        /// </summary>
        /// <typeparam name="T">The source object type</typeparam>
        /// <param name="obj">The source object</param>
        /// <returns>The converted dictionary, or null if conversion fails</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj is null</exception>
        /// <exception cref="NotSupportedException">Thrown when T implements ICollection</exception>
        public static Dictionary<string, string>? ObjectToDictionary<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var converter = GetObjectToDictionaryConverter<T>();
            return converter(obj);
        }

        /// <summary>
        /// Gets or creates a cached converter function for object to dictionary conversion
        /// </summary>
        /// <typeparam name="T">The source object type</typeparam>
        /// <returns>A converter function</returns>
        /// <exception cref="NotSupportedException">Thrown when T implements ICollection</exception>
        public static Func<T, Dictionary<string, string>> GetObjectToDictionaryConverter<T>()
        {
            var type = typeof(T);
            ValidateType(type);

            return (Func<T, Dictionary<string, string>>)ObjectToDictionaryCache.GetOrAdd(type, 
                _ => CreateObjectToDictionaryConverter<T>());
        }

        #endregion

        #region Private Methods - Validation

        /// <summary>
        /// Validates that the type is supported for conversion
        /// </summary>
        /// <param name="type">The type to validate</param>
        /// <exception cref="NotSupportedException">Thrown when type implements ICollection</exception>
        private static void ValidateType(Type type)
        {
            if (typeof(ICollection).IsAssignableFrom(type))
            {
                throw new NotSupportedException($"Type '{type.Name}' implements ICollection and is not supported");
            }
        }

        /// <summary>
        /// Gets properties that are suitable for conversion
        /// </summary>
        /// <param name="type">The type to analyze</param>
        /// <param name="requireRead">Whether the property must be readable</param>
        /// <param name="requireWrite">Whether the property must be writable</param>
        /// <returns>List of suitable properties</returns>
        private static List<PropertyInfo> GetConvertibleProperties(Type type, bool requireRead, bool requireWrite)
        {
            return PropertyInfoUtil.GetProperties(type)
                .Where(p => (!requireRead || p.CanRead) && 
                           (!requireWrite || p.CanWrite) && 
                           TypeUtil.IsPrimitiveExtended(p.PropertyType, true, true))
                .ToList();
        }

        #endregion

        #region Private Methods - Dictionary to Object Conversion

        /// <summary>
        /// Creates a new dictionary to object converter using IL emit
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <returns>A converter function</returns>
        private static Func<Dictionary<string, string>, T> CreateDictionaryToObjectConverter<T>() where T : class, new()
        {
            var targetType = typeof(T);
            var properties = GetConvertibleProperties(targetType, requireRead: false, requireWrite: true);

            var dynamicMethod = new DynamicMethod(
                $"DictionaryToObject_{targetType.Name}",
                targetType,
                new[] { typeof(Dictionary<string, string>) },
                targetType,
                skipVisibility: true)
            {
                InitLocals = false
            };

            var il = dynamicMethod.GetILGenerator();
            var endLabel = il.DefineLabel();

            // Create new instance: var obj = new T();
            var objLocal = il.DeclareLocal(targetType);
            var valueLocal = il.DeclareLocal(typeof(string));

            il.Emit(OpCodes.Newobj, targetType.GetConstructor(Type.EmptyTypes)!);
            il.Emit(OpCodes.Stloc, objLocal);

            // Process each property
            foreach (var property in properties)
            {
                EmitDictionaryToObjectPropertyConversion(il, property, objLocal, valueLocal);
            }

            // Return the object
            il.Emit(OpCodes.Ldloc, objLocal);
            il.Emit(OpCodes.Ret);

            var converter = (Func<Dictionary<string, string>, T>)dynamicMethod.CreateDelegate(
                typeof(Func<Dictionary<string, string>, T>));

            return converter ?? throw new VerificationException("Failed to create dictionary to object converter");
        }

        /// <summary>
        /// Emits IL code for converting a single property from dictionary to object
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="property">The property to convert</param>
        /// <param name="objLocal">Local variable holding the target object</param>
        /// <param name="valueLocal">Local variable for string values</param>
        private static void EmitDictionaryToObjectPropertyConversion(ILGenerator il, PropertyInfo property, 
            LocalBuilder objLocal, LocalBuilder valueLocal)
        {
            var endIfLabel = il.DefineLabel();

            // Check if dictionary contains the key
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, property.Name);
            il.Emit(OpCodes.Callvirt, ReflectionInfo.DictionaryContainsKey);
            il.Emit(OpCodes.Brfalse_S, endIfLabel);

            // Get the value from dictionary
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, property.Name);
            il.Emit(OpCodes.Callvirt, ReflectionInfo.DictionaryGetItem);
            il.Emit(OpCodes.Stloc, valueLocal);

            // Check if value is not null, empty, or whitespace
            il.Emit(OpCodes.Ldloc, valueLocal);
            il.Emit(OpCodes.Call, ReflectionInfo.StringIsNullOrWhiteSpace);
            il.Emit(OpCodes.Brtrue_S, endIfLabel);

            // Set the property value
            il.Emit(OpCodes.Ldloc, objLocal);
            il.Emit(OpCodes.Ldloc, valueLocal);
            EmitStringToValueConversion(il, property.PropertyType);
            il.Emit(OpCodes.Callvirt, property.GetSetMethod()!);

            il.MarkLabel(endIfLabel);
        }

        /// <summary>
        /// Emits IL code to convert string to the target type
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="targetType">The target type</param>
        private static void EmitStringToValueConversion(ILGenerator il, Type targetType)
        {
            if (targetType == typeof(string))
            {
                // No conversion needed for string
                return;
            }

            if (targetType.IsValueType)
            {
                if (EmitUtil.IsNullable(targetType))
                {
                    // Nullable value type: Parse underlying type and create nullable
                    var underlyingType = EmitUtil.GetNullableUnderlyingType(targetType);
                    var parseMethod = GetParseMethod(underlyingType);
                    il.Emit(OpCodes.Call, parseMethod);
                    il.Emit(OpCodes.Newobj, targetType.GetConstructor(new[] { underlyingType })!);
                }
                else
                {
                    // Non-nullable value type: Parse directly
                    var parseMethod = GetParseMethod(targetType);
                    il.Emit(OpCodes.Call, parseMethod);
                }
            }
        }

        /// <summary>
        /// Gets the Parse method for the specified type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The Parse method</returns>
        private static MethodInfo GetParseMethod(Type type)
        {
            return type.GetMethod("Parse", new[] { typeof(string) }) 
                ?? throw new NotSupportedException($"Type '{type.Name}' does not have a Parse(string) method");
        }

        #endregion

        #region Private Methods - Object to Dictionary Conversion

        /// <summary>
        /// Creates a new object to dictionary converter using IL emit
        /// </summary>
        /// <typeparam name="T">The source object type</typeparam>
        /// <returns>A converter function</returns>
        private static Func<T, Dictionary<string, string>> CreateObjectToDictionaryConverter<T>()
        {
            var sourceType = typeof(T);
            var properties = GetConvertibleProperties(sourceType, requireRead: true, requireWrite: false);

            var dynamicMethod = new DynamicMethod(
                $"ObjectToDictionary_{sourceType.Name}",
                typeof(Dictionary<string, string>),
                new[] { sourceType },
                typeof(object),
                skipVisibility: true)
            {
                InitLocals = true
            };

            var il = dynamicMethod.GetILGenerator();

            // Create new dictionary: var dict = new Dictionary<string, string>();
            var dictLocal = il.DeclareLocal(typeof(Dictionary<string, string>));
            var localVariables = new Dictionary<Type, LocalBuilder>();

            il.Emit(OpCodes.Newobj, typeof(Dictionary<string, string>).GetConstructor(Type.EmptyTypes)!);
            il.Emit(OpCodes.Stloc, dictLocal);

            // Process each property
            foreach (var property in properties)
            {
                EmitObjectToDictionaryPropertyConversion(il, property, dictLocal, localVariables);
            }

            // Return the dictionary
            il.Emit(OpCodes.Ldloc, dictLocal);
            il.Emit(OpCodes.Ret);

            var converter = (Func<T, Dictionary<string, string>>)dynamicMethod.CreateDelegate(
                typeof(Func<T, Dictionary<string, string>>));

            return converter ?? throw new VerificationException("Failed to create object to dictionary converter");
        }

        /// <summary>
        /// Emits IL code for converting a single property from object to dictionary
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="property">The property to convert</param>
        /// <param name="dictLocal">Local variable holding the target dictionary</param>
        /// <param name="localVariables">Cache of local variables</param>
        private static void EmitObjectToDictionaryPropertyConversion(ILGenerator il, PropertyInfo property,
            LocalBuilder dictLocal, Dictionary<Type, LocalBuilder> localVariables)
        {
            var endIfLabel = il.DefineLabel();
            var propertyType = property.PropertyType;

            // Handle null checks for reference types and nullable value types
            if (!propertyType.IsValueType || EmitUtil.IsNullable(propertyType))
            {
                EmitNullCheck(il, property, endIfLabel, localVariables);
            }

            // Add to dictionary: dict.Add(propertyName, propertyValue.ToString())
            il.Emit(OpCodes.Ldloc, dictLocal);
            il.Emit(OpCodes.Ldstr, property.Name);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, property.GetGetMethod()!);
            EmitValueToStringConversion(il, propertyType, localVariables);
            il.Emit(OpCodes.Callvirt, ReflectionInfo.DictionaryAdd);

            il.MarkLabel(endIfLabel);
        }

        /// <summary>
        /// Emits IL code for null checking
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="property">The property to check</param>
        /// <param name="endIfLabel">Label to jump to if null</param>
        /// <param name="localVariables">Cache of local variables</param>
        private static void EmitNullCheck(ILGenerator il, PropertyInfo property, Label endIfLabel,
            Dictionary<Type, LocalBuilder> localVariables)
        {
            var propertyType = property.PropertyType;

            if (propertyType == typeof(string))
            {
                // String null or empty check
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, property.GetGetMethod()!);
                il.Emit(OpCodes.Call, ReflectionInfo.StringIsNullOrEmpty);
                il.Emit(OpCodes.Brtrue_S, endIfLabel);
            }
            else if (EmitUtil.IsNullable(propertyType))
            {
                // Nullable value type HasValue check
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, property.GetGetMethod()!);
                var local = GetOrCreateLocal(il, propertyType, localVariables);
                il.Emit(OpCodes.Stloc, local);
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Call, propertyType.GetProperty("HasValue")!.GetGetMethod()!);
                il.Emit(OpCodes.Brfalse_S, endIfLabel);
            }
        }

        /// <summary>
        /// Emits IL code to convert value to string
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="valueType">The value type</param>
        /// <param name="localVariables">Cache of local variables</param>
        private static void EmitValueToStringConversion(ILGenerator il, Type valueType,
            Dictionary<Type, LocalBuilder> localVariables)
        {
            if (valueType == typeof(string))
            {
                // No conversion needed for string
                return;
            }

            if (valueType.IsValueType)
            {
                var local = GetOrCreateLocal(il, valueType, localVariables);
                il.Emit(OpCodes.Stloc, local);
                il.Emit(OpCodes.Ldloca, local);

                if (valueType == typeof(DateTime) || valueType == typeof(DateTime?))
                {
                    EmitDateTimeToString(il, valueType, localVariables);
                }
                else
                {
                    EmitValueTypeToString(il, valueType, localVariables);
                }
            }
        }

        /// <summary>
        /// Emits IL code to convert DateTime to string with invariant culture
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="dateTimeType">The DateTime type (DateTime or DateTime?)</param>
        /// <param name="localVariables">Cache of local variables</param>
        private static void EmitDateTimeToString(ILGenerator il, Type dateTimeType,
            Dictionary<Type, LocalBuilder> localVariables)
        {
            if (dateTimeType == typeof(DateTime?))
            {
                // Get the value from nullable DateTime
                il.Emit(OpCodes.Call, dateTimeType.GetProperty("Value")!.GetGetMethod()!);
                var realLocal = GetOrCreateLocal(il, typeof(DateTime), localVariables);
                il.Emit(OpCodes.Stloc, realLocal);
                il.Emit(OpCodes.Ldloca, realLocal);
            }

            il.Emit(OpCodes.Call, ReflectionInfo.CultureInfoInvariantCulture);
            il.Emit(OpCodes.Call, ReflectionInfo.DateTimeToString);
        }

        /// <summary>
        /// Emits IL code to convert value type to string
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="valueType">The value type</param>
        /// <param name="localVariables">Cache of local variables</param>
        private static void EmitValueTypeToString(ILGenerator il, Type valueType,
            Dictionary<Type, LocalBuilder> localVariables)
        {
            if (EmitUtil.IsNullable(valueType))
            {
                // Get the value from nullable type
                il.Emit(OpCodes.Call, valueType.GetProperty("Value")!.GetGetMethod()!);
                var underlyingType = EmitUtil.GetNullableUnderlyingType(valueType);
                var realLocal = GetOrCreateLocal(il, underlyingType, localVariables);
                il.Emit(OpCodes.Stloc, realLocal);
                il.Emit(OpCodes.Ldloca, realLocal);
                il.Emit(OpCodes.Call, underlyingType.GetMethod("ToString", Type.EmptyTypes)!);
            }
            else
            {
                il.Emit(OpCodes.Call, valueType.GetMethod("ToString", Type.EmptyTypes)!);
            }
        }

        /// <summary>
        /// Gets or creates a local variable for the specified type
        /// </summary>
        /// <param name="il">The IL generator</param>
        /// <param name="type">The type</param>
        /// <param name="localVariables">Cache of local variables</param>
        /// <returns>The local variable</returns>
        private static LocalBuilder GetOrCreateLocal(ILGenerator il, Type type,
            Dictionary<Type, LocalBuilder> localVariables)
        {
            if (!localVariables.TryGetValue(type, out var local))
            {
                local = il.DeclareLocal(type);
                localVariables[type] = local;
            }
            return local;
        }

        #endregion

        #region Reflection Cache

        /// <summary>
        /// Caches frequently used reflection information for better performance
        /// </summary>
        private sealed class ReflectionCache
        {
            public readonly MethodInfo DictionaryContainsKey;
            public readonly MethodInfo DictionaryGetItem;
            public readonly MethodInfo DictionaryAdd;
            public readonly MethodInfo StringIsNullOrEmpty;
            public readonly MethodInfo StringIsNullOrWhiteSpace;
            public readonly MethodInfo CultureInfoInvariantCulture;
            public readonly MethodInfo DateTimeToString;

            public ReflectionCache()
            {
                var dictionaryType = typeof(Dictionary<string, string>);
                DictionaryContainsKey = dictionaryType.GetMethod("ContainsKey", new[] { typeof(string) })!;
                DictionaryGetItem = dictionaryType.GetMethod("get_Item", new[] { typeof(string) })!;
                DictionaryAdd = dictionaryType.GetMethod("Add", new[] { typeof(string), typeof(string) })!;

                StringIsNullOrEmpty = typeof(string).GetMethod("IsNullOrEmpty", new[] { typeof(string) })!;
                StringIsNullOrWhiteSpace = typeof(string).GetMethod("IsNullOrWhiteSpace", new[] { typeof(string) })!;
                CultureInfoInvariantCulture = typeof(CultureInfo).GetProperty("InvariantCulture")!.GetGetMethod()!;
                DateTimeToString = typeof(DateTime).GetMethod("ToString", new[] { typeof(IFormatProvider) })!;
            }
        }

        #endregion
    }
}
