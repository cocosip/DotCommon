using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Utility class for PropertyInfo operations and property reflection.
    /// </summary>
    public static class PropertyInfoUtil
    {
        #region Get Properties

        /// <summary>
        /// Gets all properties of the specified type.
        /// </summary>
        /// <param name="type">The type to get properties from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>A list of PropertyInfo objects representing the properties</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static List<PropertyInfo> GetProperties(Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var properties = type.GetTypeInfo().GetProperties(bindingFlags).ToList();
            return properties;
        }

        /// <summary>
        /// Gets all properties of the specified object's type.
        /// </summary>
        /// <param name="obj">The object to get properties from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>An enumerable of PropertyInfo objects representing the properties</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj is null</exception>
        public static IEnumerable<PropertyInfo> GetProperties(object obj,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return GetProperties(obj.GetType(), bindingFlags);
        }

        /// <summary>
        /// Gets all readable properties of the specified type.
        /// </summary>
        /// <param name="type">The type to get readable properties from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>A list of readable PropertyInfo objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static List<PropertyInfo> GetReadableProperties(Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetProperties(type, bindingFlags).Where(p => p.CanRead).ToList();
        }

        /// <summary>
        /// Gets all writable properties of the specified type.
        /// </summary>
        /// <param name="type">The type to get writable properties from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>A list of writable PropertyInfo objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static List<PropertyInfo> GetWritableProperties(Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetProperties(type, bindingFlags).Where(p => p.CanWrite).ToList();
        }

        /// <summary>
        /// Gets all properties of the specified type that have both getter and setter.
        /// </summary>
        /// <param name="type">The type to get read-write properties from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>A list of read-write PropertyInfo objects</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static List<PropertyInfo> GetReadWriteProperties(Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetProperties(type, bindingFlags).Where(p => p.CanRead && p.CanWrite).ToList();
        }

        #endregion

        #region Find Property

        /// <summary>
        /// Finds a property by name in the specified type.
        /// </summary>
        /// <param name="type">The type to search for the property</param>
        /// <param name="propertyName">The name of the property to find</param>
        /// <param name="bindingFlags">The binding flags to use for property search</param>
        /// <returns>The PropertyInfo if found; otherwise, null</returns>
        /// <exception cref="ArgumentNullException">Thrown when type or propertyName is null</exception>
        public static PropertyInfo? FindProperty(Type type, string propertyName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return type.GetProperty(propertyName, bindingFlags);
        }

        /// <summary>
        /// Finds a property by name in the specified object's type.
        /// </summary>
        /// <param name="obj">The object to search for the property</param>
        /// <param name="propertyName">The name of the property to find</param>
        /// <param name="bindingFlags">The binding flags to use for property search</param>
        /// <returns>The PropertyInfo if found; otherwise, null</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj or propertyName is null</exception>
        public static PropertyInfo? FindProperty(object obj, string propertyName,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return FindProperty(obj.GetType(), propertyName, bindingFlags);
        }

        /// <summary>
        /// Finds properties by their property type in the specified type.
        /// </summary>
        /// <param name="type">The type to search for properties</param>
        /// <param name="propertyType">The property type to match</param>
        /// <param name="bindingFlags">The binding flags to use for property search</param>
        /// <returns>A list of PropertyInfo objects that match the specified property type</returns>
        /// <exception cref="ArgumentNullException">Thrown when type or propertyType is null</exception>
        public static List<PropertyInfo> FindPropertiesByType(Type type, Type propertyType,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return GetProperties(type, bindingFlags)
                .Where(p => p.PropertyType == propertyType)
                .ToList();
        }

        #endregion

        #region Property Values

        /// <summary>
        /// Gets the value of a property from the specified object.
        /// </summary>
        /// <param name="obj">The object to get the property value from</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj or propertyName is null</exception>
        /// <exception cref="ArgumentException">Thrown when the property is not found or cannot be read</exception>
        public static object? GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var property = FindProperty(obj, propertyName);
            if (property == null)
                throw new ArgumentException($"Property '{propertyName}' not found in type '{obj.GetType().FullName}'", nameof(propertyName));

            if (!property.CanRead)
                throw new ArgumentException($"Property '{propertyName}' cannot be read", nameof(propertyName));

            return property.GetValue(obj);
        }

        /// <summary>
        /// Gets the value of a property from the specified object with a generic return type.
        /// </summary>
        /// <typeparam name="T">The expected type of the property value</typeparam>
        /// <param name="obj">The object to get the property value from</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property cast to the specified type</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj or propertyName is null</exception>
        /// <exception cref="ArgumentException">Thrown when the property is not found or cannot be read</exception>
        /// <exception cref="InvalidCastException">Thrown when the property value cannot be cast to the specified type</exception>
        public static T? GetPropertyValue<T>(object obj, string propertyName)
        {
            var value = GetPropertyValue(obj, propertyName);
            return (T?)value;
        }

        /// <summary>
        /// Sets the value of a property on the specified object.
        /// </summary>
        /// <param name="obj">The object to set the property value on</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to set</param>
        /// <exception cref="ArgumentNullException">Thrown when obj or propertyName is null</exception>
        /// <exception cref="ArgumentException">Thrown when the property is not found or cannot be written</exception>
        public static void SetPropertyValue(object obj, string propertyName, object? value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var property = FindProperty(obj, propertyName);
            if (property == null)
                throw new ArgumentException($"Property '{propertyName}' not found in type '{obj.GetType().FullName}'", nameof(propertyName));

            if (!property.CanWrite)
                throw new ArgumentException($"Property '{propertyName}' cannot be written", nameof(propertyName));

            property.SetValue(obj, value);
        }

        #endregion

        #region Property Attributes

        /// <summary>
        /// Gets all custom attributes of the specified type from a property.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="property">The property to get attributes from</param>
        /// <param name="inherit">True to search the property's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>An array of attributes of the specified type</returns>
        /// <exception cref="ArgumentNullException">Thrown when property is null</exception>
        public static TAttribute[] GetCustomAttributes<TAttribute>(PropertyInfo property, bool inherit = true)
            where TAttribute : Attribute
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.GetCustomAttributes<TAttribute>(inherit).ToArray();
        }

        /// <summary>
        /// Gets the first custom attribute of the specified type from a property, or null if not found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="property">The property to get the attribute from</param>
        /// <param name="inherit">True to search the property's inheritance chain to find the attribute; otherwise, false</param>
        /// <returns>The first attribute of the specified type, or null if not found</returns>
        /// <exception cref="ArgumentNullException">Thrown when property is null</exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(PropertyInfo property, bool inherit = true)
            where TAttribute : Attribute
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.GetCustomAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Determines whether the property has the specified custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to check for</typeparam>
        /// <param name="property">The property to check</param>
        /// <param name="inherit">True to search the property's inheritance chain to find the attribute; otherwise, false</param>
        /// <returns>True if the property has the specified attribute; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when property is null</exception>
        public static bool HasCustomAttribute<TAttribute>(PropertyInfo property, bool inherit = true)
            where TAttribute : Attribute
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.IsDefined(typeof(TAttribute), inherit);
        }

        #endregion

        #region Property Information

        /// <summary>
        /// Determines whether the property is an auto-implemented property.
        /// </summary>
        /// <param name="property">The property to check</param>
        /// <returns>True if the property is auto-implemented; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when property is null</exception>
        public static bool IsAutoProperty(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var getter = property.GetGetMethod(true);
            if (getter == null) return false;

            // Auto-implemented properties have a backing field with CompilerGenerated attribute
            var backingField = property.DeclaringType?.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.Name.Contains($"<{property.Name}>k__BackingField"));

            return backingField != null;
        }

        /// <summary>
        /// Gets the property names of the specified type.
        /// </summary>
        /// <param name="type">The type to get property names from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>An array of property names</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static string[] GetPropertyNames(Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetProperties(type, bindingFlags).Select(p => p.Name).ToArray();
        }

        /// <summary>
        /// Gets the property names of the specified object's type.
        /// </summary>
        /// <param name="obj">The object to get property names from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>An array of property names</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj is null</exception>
        public static string[] GetPropertyNames(object obj,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return GetPropertyNames(obj.GetType(), bindingFlags);
        }

        /// <summary>
        /// Creates a dictionary of property names and their values from the specified object.
        /// </summary>
        /// <param name="obj">The object to create the dictionary from</param>
        /// <param name="bindingFlags">The binding flags to use for property retrieval</param>
        /// <returns>A dictionary containing property names as keys and their values</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj is null</exception>
        public static Dictionary<string, object?> ToPropertyDictionary(object obj,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var result = new Dictionary<string, object?>();
            var properties = GetReadableProperties(obj.GetType(), bindingFlags);

            foreach (var property in properties)
            {
                result[property.Name] = property.GetValue(obj);
            }

            return result;
        }

        #endregion
    }
}
