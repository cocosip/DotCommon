using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Provides high-performance object-dictionary mapping using Expression Trees
    /// </summary>
    public static class ExpressionMapper
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
        /// Converts a Dictionary&lt;string, object&gt; to the specified object type
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <param name="dictionary">The source dictionary</param>
        /// <returns>The converted object instance, or null if conversion fails</returns>
        /// <exception cref="ArgumentNullException">Thrown when dictionary is null</exception>
        /// <exception cref="NotSupportedException">Thrown when T implements ICollection</exception>
        public static T? DictionaryToObject<T>(Dictionary<string, object> dictionary) where T : class, new()
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
        public static Func<Dictionary<string, object>, T> GetDictionaryToObjectConverter<T>() where T : class, new()
        {
            var type = typeof(T);
            ValidateType(type);

            return (Func<Dictionary<string, object>, T>)DictionaryToObjectCache.GetOrAdd(type,
                _ => CreateDictionaryToObjectConverter<T>());
        }

        #endregion

        #region Public Methods - Object to Dictionary Conversion

        /// <summary>
        /// Converts an object to a Dictionary&lt;string, object&gt;
        /// </summary>
        /// <typeparam name="T">The source object type</typeparam>
        /// <param name="obj">The source object</param>
        /// <returns>The converted dictionary, or null if conversion fails</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj is null</exception>
        /// <exception cref="NotSupportedException">Thrown when T implements ICollection</exception>
        public static Dictionary<string, object>? ObjectToDictionary<T>(T obj) where T : class, new()
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
        public static Func<T, Dictionary<string, object>> GetObjectToDictionaryConverter<T>() where T : class, new()
        {
            var type = typeof(T);
            ValidateType(type);

            return (Func<T, Dictionary<string, object>>)ObjectToDictionaryCache.GetOrAdd(type,
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
        /// Creates a new dictionary to object converter using Expression Trees
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <returns>A converter function</returns>
        private static Func<Dictionary<string, object>, T> CreateDictionaryToObjectConverter<T>() where T : class, new()
        {
            var targetType = typeof(T);
            var sourceType = typeof(Dictionary<string, object>);
            var properties = GetConvertibleProperties(targetType, requireRead: false, requireWrite: true);

            // Define parameters and variables
            var parameterExpr = Expression.Parameter(sourceType, "dictionary");
            var objectVar = Expression.Variable(targetType, "obj");
            var bodyExpressions = new List<Expression>();

            // Create new instance: var obj = new T();
            var newObjectExpr = Expression.New(targetType);
            var assignObjectExpr = Expression.Assign(objectVar, newObjectExpr);
            bodyExpressions.Add(assignObjectExpr);

            // Process each property
            foreach (var property in properties)
            {
                var propertyAssignExpr = CreateDictionaryToObjectPropertyExpression(
                    parameterExpr, objectVar, property);
                bodyExpressions.Add(propertyAssignExpr);
            }

            // Return the object
            bodyExpressions.Add(objectVar);

            // Create method body
            var methodBodyExpr = Expression.Block(targetType, new[] { objectVar }, bodyExpressions);
            var lambdaExpr = Expression.Lambda<Func<Dictionary<string, object>, T>>(methodBodyExpr, parameterExpr);

            try
            {
                return lambdaExpr.Compile();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create dictionary to object converter for type '{targetType.Name}'", ex);
            }
        }

        /// <summary>
        /// Creates an expression for converting a single property from dictionary to object
        /// </summary>
        /// <param name="dictionaryParam">The dictionary parameter expression</param>
        /// <param name="objectVar">The object variable expression</param>
        /// <param name="property">The property to convert</param>
        /// <returns>An expression for property assignment</returns>
        private static Expression CreateDictionaryToObjectPropertyExpression(
            ParameterExpression dictionaryParam, ParameterExpression objectVar, PropertyInfo property)
        {
            var propertyNameExpr = Expression.Constant(property.Name);
            var propertyExpr = Expression.Property(objectVar, property);

            // Check if dictionary contains the key
            var containsKeyExpr = Expression.Call(dictionaryParam, ReflectionInfo.DictionaryContainsKey, propertyNameExpr);

            // Get value from dictionary
            var indexerExpr = Expression.Property(dictionaryParam, ReflectionInfo.DictionaryIndexer, propertyNameExpr);

            // Convert value to property type
            var convertedValueExpr = CreateValueConversionExpression(indexerExpr, property.PropertyType);

            // Assign property value
            var assignExpr = Expression.Assign(propertyExpr, convertedValueExpr);

            // Check for null value
            var notNullExpr = Expression.IfThen(
                Expression.NotEqual(indexerExpr, Expression.Constant(null)),
                assignExpr);

            // Combine conditions: if (dictionary.ContainsKey(key) && value != null) assign value
            return Expression.IfThen(containsKeyExpr, notNullExpr);
        }

        /// <summary>
        /// Creates an expression for converting object value to target type
        /// </summary>
        /// <param name="valueExpr">The value expression</param>
        /// <param name="targetType">The target type</param>
        /// <returns>A conversion expression</returns>
        private static Expression CreateValueConversionExpression(Expression valueExpr, Type targetType)
        {
            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                // For nullable types, convert to underlying type first, then to nullable
                var convertToUnderlyingExpr = Expression.Convert(valueExpr, underlyingType);
                return Expression.Convert(convertToUnderlyingExpr, targetType);
            }

            // Direct conversion for non-nullable types
            return Expression.Convert(valueExpr, targetType);
        }

        #endregion

        #region Private Methods - Object to Dictionary Conversion

        /// <summary>
        /// Creates a new object to dictionary converter using Expression Trees
        /// </summary>
        /// <typeparam name="T">The source object type</typeparam>
        /// <returns>A converter function</returns>
        private static Func<T, Dictionary<string, object>> CreateObjectToDictionaryConverter<T>() where T : class, new()
        {
            var sourceType = typeof(T);
            var targetType = typeof(Dictionary<string, object>);
            var properties = GetConvertibleProperties(sourceType, requireRead: true, requireWrite: false);

            // Define parameters and variables
            var parameterExpr = Expression.Parameter(sourceType, "obj");
            var dictionaryVar = Expression.Variable(targetType, "dictionary");
            var bodyExpressions = new List<Expression>();

            // Create new dictionary: var dictionary = new Dictionary<string, object>();
            var newDictionaryExpr = Expression.New(targetType);
            var assignDictionaryExpr = Expression.Assign(dictionaryVar, newDictionaryExpr);
            bodyExpressions.Add(assignDictionaryExpr);

            // Process each property
            foreach (var property in properties)
            {
                var propertyAddExpr = CreateObjectToDictionaryPropertyExpression(
                    parameterExpr, dictionaryVar, property);
                bodyExpressions.Add(propertyAddExpr);
            }

            // Return the dictionary
            bodyExpressions.Add(dictionaryVar);

            // Create method body
            var methodBodyExpr = Expression.Block(targetType, new[] { dictionaryVar }, bodyExpressions);
            var lambdaExpr = Expression.Lambda<Func<T, Dictionary<string, object>>>(methodBodyExpr, parameterExpr);

            try
            {
                return lambdaExpr.Compile();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create object to dictionary converter for type '{sourceType.Name}'", ex);
            }
        }

        /// <summary>
        /// Creates an expression for converting a single property from object to dictionary
        /// </summary>
        /// <param name="objectParam">The object parameter expression</param>
        /// <param name="dictionaryVar">The dictionary variable expression</param>
        /// <param name="property">The property to convert</param>
        /// <returns>An expression for adding property to dictionary</returns>
        private static Expression CreateObjectToDictionaryPropertyExpression(
            ParameterExpression objectParam, ParameterExpression dictionaryVar, PropertyInfo property)
        {
            var propertyNameExpr = Expression.Constant(property.Name);
            var propertyValueExpr = Expression.Property(objectParam, property);
            var boxedValueExpr = Expression.Convert(propertyValueExpr, typeof(object));

            // Add to dictionary: dictionary.Add(propertyName, propertyValue)
            var addMethodExpr = Expression.Call(dictionaryVar, ReflectionInfo.DictionaryAdd, 
                propertyNameExpr, boxedValueExpr);

            // Add null value: dictionary.Add(propertyName, null)
            var addNullMethodExpr = Expression.Call(dictionaryVar, ReflectionInfo.DictionaryAdd,
                propertyNameExpr, Expression.Constant(null, typeof(object)));

            // Handle null values for reference types and nullable value types
            if (!property.PropertyType.IsValueType || Nullable.GetUnderlyingType(property.PropertyType) != null)
            {
                return Expression.IfThenElse(
                    Expression.NotEqual(boxedValueExpr, Expression.Constant(null)),
                    addMethodExpr,
                    addNullMethodExpr);
            }

            // For non-nullable value types, always add the value
            return addMethodExpr;
        }

        #endregion

        #region Reflection Cache

        /// <summary>
        /// Caches frequently used reflection information for better performance
        /// </summary>
        private sealed class ReflectionCache
        {
            public readonly MethodInfo DictionaryContainsKey;
            public readonly PropertyInfo DictionaryIndexer;
            public readonly MethodInfo DictionaryAdd;

            public ReflectionCache()
            {
                var dictionaryType = typeof(Dictionary<string, object>);
                DictionaryContainsKey = dictionaryType.GetMethod("ContainsKey", new[] { typeof(string) })!;
                DictionaryIndexer = dictionaryType.GetProperty("Item", new[] { typeof(string) })!;
                DictionaryAdd = dictionaryType.GetMethod("Add", new[] { typeof(string), typeof(object) })!;
            }
        }

        #endregion
    }
}
