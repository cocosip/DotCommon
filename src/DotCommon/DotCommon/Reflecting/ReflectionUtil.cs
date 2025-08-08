using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Provides utility methods for reflection operations including type checking, attribute retrieval, and property path manipulation.
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// Checks whether the given type implements or inherits from the specified generic type.
        /// </summary>
        /// <param name="givenType">The type to check for assignability</param>
        /// <param name="genericType">The generic type definition to check against</param>
        /// <returns>True if the given type is assignable to the generic type; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when givenType or genericType is null</exception>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            if (givenType == null)
                throw new ArgumentNullException(nameof(givenType));
            if (genericType == null)
                throw new ArgumentNullException(nameof(genericType));

            var givenTypeInfo = givenType.GetTypeInfo();

            if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenTypeInfo.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and its declaring type including inherited attributes.
        /// </summary>
        /// <param name="memberInfo">The member to get attributes from</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>A list of all attributes found on the member and its declaring type</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static List<object> GetAttributesOfMemberAndDeclaringType(MemberInfo memberInfo, bool inherit = true)
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            var attributeList = new List<object>();

            attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));

            if (memberInfo.DeclaringType != null)
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(inherit));
            }

            return attributeList;
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and the specified type including inherited attributes.
        /// </summary>
        /// <param name="memberInfo">The member to get attributes from</param>
        /// <param name="type">The type to get attributes from</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>A list of all attributes found on the member and the specified type</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo or type is null</exception>
        public static List<object> GetAttributesOfMemberAndType(MemberInfo memberInfo, Type type, bool inherit = true)
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var attributeList = new List<object>();
            attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));
            attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(inherit));
            return attributeList;
        }

        /// <summary>
        /// Gets a list of attributes of the specified type defined for a class member and its declaring type including inherited attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo">The member to get attributes from</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>A list of attributes of the specified type found on the member and its declaring type</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            var attributeList = new List<TAttribute>();

            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// Gets a list of attributes of the specified type defined for a class member and the specified type including inherited attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo">The member to get attributes from</param>
        /// <param name="type">The type to get attributes from</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>A list of attributes of the specified type found on the member and the specified type</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo or type is null</exception>
        public static List<TAttribute> GetAttributesOfMemberAndType<TAttribute>(MemberInfo memberInfo, Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var attributeList = new List<TAttribute>();

            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            if (type.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// Tries to get a single attribute of the specified type defined for a class member or its declaring type.
        /// Returns the default value if the attribute is not declared at all.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo">The member to get the attribute from</param>
        /// <param name="defaultValue">The default value to return if the attribute is not found</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>The first attribute found, or the default value if none is found</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static TAttribute? GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute? defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : class
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo.GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
                   ?? memberInfo.ReflectedType?.GetTypeInfo().GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
                   ?? defaultValue;
        }

        /// <summary>
        /// Tries to get a single attribute of the specified type defined for a class member.
        /// Returns the default value if the attribute is not declared.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo">The member to get the attribute from</param>
        /// <param name="defaultValue">The default value to return if the attribute is not found</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>The first attribute found, or the default value if none is found</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static TAttribute? GetSingleAttributeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute? defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets a property by its full path from the given object type.
        /// </summary>
        /// <param name="obj">The object to get the property from</param>
        /// <param name="objectType">The type of the given object</param>
        /// <param name="propertyPath">The full path of the property (dot-separated)</param>
        /// <returns>The PropertyInfo object representing the property at the specified path</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj, objectType, or propertyPath is null</exception>
        /// <exception cref="ArgumentException">Thrown when the property path is invalid or property is not found</exception>
        public static PropertyInfo GetPropertyByPath(object obj, Type objectType, string propertyPath)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (string.IsNullOrEmpty(propertyPath))
                throw new ArgumentNullException(nameof(propertyPath));

            var currentType = objectType;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (objectPath != null && absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            PropertyInfo? property = null;
            foreach (var propertyName in absolutePropertyPath.Split('.'))
            {
                property = currentType.GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found in type '{currentType.FullName}'", nameof(propertyPath));
                }
                currentType = property.PropertyType;
            }

            return property!;
        }

        /// <summary>
        /// Gets the value of a property by its full path from the given object.
        /// </summary>
        /// <param name="obj">The object to get the value from</param>
        /// <param name="objectType">The type of the given object</param>
        /// <param name="propertyPath">The full path of the property (dot-separated)</param>
        /// <returns>The value of the property at the specified path</returns>
        /// <exception cref="ArgumentNullException">Thrown when obj, objectType, or propertyPath is null</exception>
        /// <exception cref="ArgumentException">Thrown when the property path is invalid or property is not found</exception>
        public static object? GetValueByPath(object obj, Type objectType, string propertyPath)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (string.IsNullOrEmpty(propertyPath))
                throw new ArgumentNullException(nameof(propertyPath));

            var value = obj;
            var currentType = objectType;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (objectPath != null && absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            foreach (var propertyName in absolutePropertyPath.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found in type '{currentType.FullName}'", nameof(propertyPath));
                }
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }

        /// <summary>
        /// Sets the value of a property by its full path on the given object.
        /// </summary>
        /// <param name="obj">The object to set the value on</param>
        /// <param name="objectType">The type of the given object</param>
        /// <param name="propertyPath">The full path of the property (dot-separated)</param>
        /// <param name="value">The value to set</param>
        /// <exception cref="ArgumentNullException">Thrown when obj, objectType, or propertyPath is null</exception>
        /// <exception cref="ArgumentException">Thrown when the property path is invalid or property is not found</exception>
        public static void SetValueByPath(object obj, Type objectType, string propertyPath, object? value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));
            if (string.IsNullOrEmpty(propertyPath))
                throw new ArgumentNullException(nameof(propertyPath));

            var currentType = objectType;
            PropertyInfo? property;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (objectPath != null && absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            var properties = absolutePropertyPath.Split('.');

            if (properties.Length == 1)
            {
                property = objectType.GetProperty(properties.First());
                if (property == null)
                {
                    throw new ArgumentException($"Property '{properties.First()}' not found in type '{objectType.FullName}'", nameof(propertyPath));
                }
                property.SetValue(obj, value);
                return;
            }

            for (int i = 0; i < properties.Length - 1; i++)
            {
                property = currentType.GetProperty(properties[i]);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{properties[i]}' not found in type '{currentType.FullName}'", nameof(propertyPath));
                }
                obj = property.GetValue(obj, null)!;
                currentType = property.PropertyType;
            }

            property = currentType.GetProperty(properties.Last());
            if (property == null)
            {
                throw new ArgumentException($"Property '{properties.Last()}' not found in type '{currentType.FullName}'", nameof(propertyPath));
            }
            property.SetValue(obj, value);
        }

        /// <summary>
        /// Determines whether the specified method is a property getter or setter method.
        /// </summary>
        /// <param name="method">The method to check</param>
        /// <param name="type">The type that contains the method</param>
        /// <returns>True if the method is a property getter or setter; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when method or type is null</exception>
        public static bool IsPropertyGetterSetterMethod(MethodInfo method, Type type)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!method.IsSpecialName)
            {
                return false;
            }

            if (method.Name.Length < 5)
            {
                return false;
            }

            return type.GetProperty(method.Name.Substring(4), BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public) != null;
        }
    }
}