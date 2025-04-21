using System;
using System.Linq;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>MemberInfo扩展 <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// 获取成员的单个属性
        /// </summary>
        /// <typeparam name="TAttribute">属性类型</typeparam>
        /// <param name="memberInfo">获取属性的成员</param>
        /// <param name="inherit">是否包含继承的属性</param>
        /// <returns>属性返回的对象,如果未找到则返回null.</returns>
        public static TAttribute? GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
            if (attrs.Length > 0)
            {
                return (TAttribute)attrs[0];
            }

            return default;
        }

        /// <summary>
        /// 获取成员的单个属性或者基础类型
        /// </summary>
        /// <typeparam name="TAttribute">属性类型</typeparam>
        /// <param name="type">获取属性的类型</param>
        /// <param name="inherit">是否包含继承的属性</param>
        /// <returns></returns>
        public static TAttribute? GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
            if (attr != null)
            {
                return attr;
            }

            if (type.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
        }
    }
}