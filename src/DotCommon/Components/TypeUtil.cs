using System;
using System.Linq;
using System.Reflection;
using DotCommon.Components;

namespace DotCommon.Components
{
    internal class TypeUtil
    {
        /// <summary>Check whether a type is a component type.
        /// </summary>
        public static bool IsComponent(Type type)
        {
            return type.GetTypeInfo().IsClass && !type.GetTypeInfo().IsAbstract &&
                   type.GetTypeInfo().GetCustomAttributes(typeof(ComponentAttribute), false).Any();
        }
    }
}
