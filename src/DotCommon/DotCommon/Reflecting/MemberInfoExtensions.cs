using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>
    /// Extension methods for <see cref="MemberInfo"/> and related reflection types.
    /// </summary>
    public static class MemberInfoExtensions
    {
        #region Attribute Extensions

        /// <summary>
        /// Gets the first custom attribute of the specified type from a member, or null if not found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo">The member to get the attribute from</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attribute; otherwise, false</param>
        /// <returns>The first attribute of the specified type, or null if not found</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static TAttribute? GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
            return attrs.Length > 0 ? (TAttribute)attrs[0] : default;
        }

        /// <summary>
        /// Gets all custom attributes of the specified type from a member.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="memberInfo">The member to get attributes from</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attributes; otherwise, false</param>
        /// <returns>An array of attributes of the specified type</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static TAttribute[] GetCustomAttributes<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().ToArray();
        }

        /// <summary>
        /// Determines whether the member has the specified custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to check for</typeparam>
        /// <param name="memberInfo">The member to check</param>
        /// <param name="inherit">True to search the member's inheritance chain to find the attribute; otherwise, false</param>
        /// <returns>True if the member has the specified attribute; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when memberInfo is null</exception>
        public static bool HasCustomAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo.IsDefined(typeof(TAttribute), inherit);
        }

        /// <summary>
        /// Gets the first custom attribute of the specified type from a type or its base types, or null if not found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to retrieve</typeparam>
        /// <param name="type">The type to get the attribute from</param>
        /// <param name="inherit">True to search the type's inheritance chain to find the attribute; otherwise, false</param>
        /// <returns>The first attribute of the specified type, or null if not found</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static TAttribute? GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
            if (attr != null)
                return attr;

            if (type.GetTypeInfo().BaseType == null)
                return null;

            return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
        }

        #endregion

        #region Type Extensions

        /// <summary>
        /// Determines whether the type is a nullable value type.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is a nullable value type; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static bool IsNullableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the underlying type of a nullable type, or the type itself if it's not nullable.
        /// </summary>
        /// <param name="type">The type to get the underlying type from</param>
        /// <returns>The underlying type if nullable; otherwise, the original type</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static Type GetNonNullableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsNullableType() ? type.GetGenericArguments()[0] : type;
        }

        /// <summary>
        /// Determines whether the type is a numeric type.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is a numeric type; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static bool IsNumericType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            type = type.GetNonNullableType();

            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        /// <summary>
        /// Determines whether the type is a primitive type or string.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is a primitive type or string; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static bool IsPrimitiveOrString(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            type = type.GetNonNullableType();
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        /// <summary>
        /// Gets all interfaces implemented by the type, including inherited interfaces.
        /// </summary>
        /// <param name="type">The type to get interfaces from</param>
        /// <returns>An array of interface types</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null</exception>
        public static Type[] GetAllInterfaces(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetInterfaces();
        }

        /// <summary>
        /// Determines whether the type implements the specified interface.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="interfaceType">The interface type to check for</param>
        /// <returns>True if the type implements the interface; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type or interfaceType is null</exception>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));

            return interfaceType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the type implements the specified generic interface.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="genericInterfaceType">The generic interface type to check for</param>
        /// <returns>True if the type implements the generic interface; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when type or genericInterfaceType is null</exception>
        public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (genericInterfaceType == null)
                throw new ArgumentNullException(nameof(genericInterfaceType));

            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType);
        }

        #endregion

        #region MethodInfo Extensions

        /// <summary>
        /// Determines whether the method has the AsyncStateMachine attribute (indicating it's an async method).
        /// </summary>
        /// <param name="method">The method to check</param>
        /// <returns>True if the method has AsyncStateMachine attribute; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when method is null</exception>
        public static bool HasAsyncStateMachine(this MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            return method.HasCustomAttribute<System.Runtime.CompilerServices.AsyncStateMachineAttribute>();
        }

        /// <summary>
        /// Determines whether the method returns a Task or Task&lt;T&gt;.
        /// </summary>
        /// <param name="method">The method to check</param>
        /// <returns>True if the method returns a Task or Task&lt;T&gt;; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when method is null</exception>
        public static bool ReturnsTask(this MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            return method.ReturnType == typeof(System.Threading.Tasks.Task) ||
                   (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(System.Threading.Tasks.Task<>));
        }

        /// <summary>
        /// Gets the parameter types of the method.
        /// </summary>
        /// <param name="method">The method to get parameter types from</param>
        /// <returns>An array of parameter types</returns>
        /// <exception cref="ArgumentNullException">Thrown when method is null</exception>
        public static Type[] GetParameterTypes(this MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            return method.GetParameters().Select(p => p.ParameterType).ToArray();
        }

        #endregion

        #region PropertyInfo Extensions

        /// <summary>
        /// Determines whether the property is an auto-implemented property.
        /// </summary>
        /// <param name="property">The property to check</param>
        /// <returns>True if the property is auto-implemented; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when property is null</exception>
        public static bool IsAutoProperty(this PropertyInfo property)
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
        /// Determines whether the property has both getter and setter.
        /// </summary>
        /// <param name="property">The property to check</param>
        /// <returns>True if the property has both getter and setter; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when property is null</exception>
        public static bool IsReadWrite(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property.CanRead && property.CanWrite;
        }

        #endregion

        #region FieldInfo Extensions

        /// <summary>
        /// Determines whether the field is a backing field for an auto-implemented property.
        /// </summary>
        /// <param name="field">The field to check</param>
        /// <returns>True if the field is a backing field; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">Thrown when field is null</exception>
        public static bool IsBackingField(this FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return field.Name.Contains(">k__BackingField") && 
                   field.HasCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>();
        }

        /// <summary>
        /// Gets the property name associated with a backing field.
        /// </summary>
        /// <param name="field">The backing field</param>
        /// <returns>The property name if it's a backing field; otherwise, null</returns>
        /// <exception cref="ArgumentNullException">Thrown when field is null</exception>
        public static string? GetPropertyNameFromBackingField(this FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (!field.IsBackingField())
                return null;

            var startIndex = field.Name.IndexOf('<') + 1;
            var endIndex = field.Name.IndexOf('>');
            
            if (startIndex > 0 && endIndex > startIndex)
                return field.Name.Substring(startIndex, endIndex - startIndex);

            return null;
        }

        #endregion

        #region Assembly Extensions

        /// <summary>
        /// Gets all types from the assembly that implement the specified interface.
        /// </summary>
        /// <param name="assembly">The assembly to search</param>
        /// <param name="interfaceType">The interface type to search for</param>
        /// <returns>An enumerable of types that implement the interface</returns>
        /// <exception cref="ArgumentNullException">Thrown when assembly or interfaceType is null</exception>
        public static IEnumerable<Type> GetTypesImplementing(this Assembly assembly, Type interfaceType)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));

            return assembly.GetTypes().Where(t => t.ImplementsInterface(interfaceType) && !t.IsInterface && !t.IsAbstract);
        }

        /// <summary>
        /// Gets all types from the assembly that are decorated with the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to search for</typeparam>
        /// <param name="assembly">The assembly to search</param>
        /// <param name="inherit">True to search the type's inheritance chain; otherwise, false</param>
        /// <returns>An enumerable of types that have the specified attribute</returns>
        /// <exception cref="ArgumentNullException">Thrown when assembly is null</exception>
        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>(this Assembly assembly, bool inherit = true)
            where TAttribute : Attribute
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return assembly.GetTypes().Where(t => t.HasCustomAttribute<TAttribute>(inherit));
        }

        #endregion
    }
}