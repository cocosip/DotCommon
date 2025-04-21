using System;
using System.Linq;
using System.Reflection;

namespace DotCommon.Reflecting
{
    /// <summary>MemberInfo��չ <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// ��ȡ��Ա�ĵ�������
        /// </summary>
        /// <typeparam name="TAttribute">��������</typeparam>
        /// <param name="memberInfo">��ȡ���Եĳ�Ա</param>
        /// <param name="inherit">�Ƿ�����̳е�����</param>
        /// <returns>���Է��صĶ���,���δ�ҵ��򷵻�null.</returns>
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
        /// ��ȡ��Ա�ĵ������Ի��߻�������
        /// </summary>
        /// <typeparam name="TAttribute">��������</typeparam>
        /// <param name="type">��ȡ���Ե�����</param>
        /// <param name="inherit">�Ƿ�����̳е�����</param>
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