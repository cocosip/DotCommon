using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotCommon;
using JetBrains.Annotations;

namespace System
{

    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the assembly of the specified type.
        /// </summary>
        /// <param name="type">The type to get assembly from.</param>
        /// <returns>The assembly of the specified type.</returns>
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        /// <summary>
        /// Gets a method from the type by method name, parameter count and generic argument count.
        /// </summary>
        /// <param name="type">The type to search for the method.</param>
        /// <param name="methodName">The name of the method to get.</param>
        /// <param name="pParametersCount">The number of parameters of the method, default is 0.</param>
        /// <param name="pGenericArgumentsCount">The number of generic arguments of the method, default is 0.</param>
        /// <returns>The MethodInfo that matches the criteria.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no matching method is found.</exception>
        public static MethodInfo GetMethod(this Type type, string methodName, int pParametersCount = 0, int pGenericArgumentsCount = 0)
        {
            // Filter methods by name, parameter count and generic argument count
            return type
                .GetMethods()
                .Where(m => m.Name == methodName).ToList()
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments()
                })
                .Where(x => x.Params.Length == pParametersCount
                            && x.Args.Length == pGenericArgumentsCount
                ).Select(x => x.Method)
                .First();
        }


        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <typeparamref name="TTarget"></typeparamref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>([NotNull] this Type type)
        {
            Check.NotNull(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <paramref name="targetType"></paramref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/> (as reverse).
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo([NotNull] this Type type, [NotNull] Type targetType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(targetType, nameof(targetType));

            return targetType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses([NotNull] this Type type, bool includeObject = true)
        {
            Check.NotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="stoppingType">A type to stop going to the deeper base classes. This type will be be included in the returned array</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses([NotNull] this Type type, Type stoppingType, bool includeObject = true)
        {
            Check.NotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            return types.ToArray();
        }

        private static void AddTypeAndBaseTypesRecursively(
            [NotNull] List<Type> types,
            Type? type,
            bool includeObject,
            Type? stoppingType = null)
        {
            if (type == null || type == stoppingType)
            {
                return;
            }

            if (!includeObject && type == typeof(object))
            {
                return;
            }

            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            types.Add(type);
        }
    }
}
