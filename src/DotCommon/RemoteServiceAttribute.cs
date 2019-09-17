using DotCommon.Reflecting;
using System;
using System.Reflection;

namespace DotCommon
{
    /// <summary>Զ�̷������Ա�ǩ
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class RemoteServiceAttribute : Attribute
    {
        /// <summary>�Ƿ�����,Ĭ��Ϊ����(true)
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>�Ƿ�����Ԫ����,Ĭ��Ϊ����(true)
        /// </summary>
        public bool IsMetadataEnabled { get; set; }

        /// <summary>Ctor
        /// </summary>
        public RemoteServiceAttribute(bool isEnabled = true)
        {
            IsEnabled = isEnabled;
            IsMetadataEnabled = true;
        }

        /// <summary>�Ƿ�����ĳ����
        /// </summary>
        public virtual bool IsEnabledFor(Type type)
        {
            return IsEnabled;
        }


        /// <summary>�Ƿ�����ĳ����
        /// </summary>
        public virtual bool IsEnabledFor(MethodInfo method)
        {
            return IsEnabled;
        }

        /// <summary>Ԫ�����Ƿ�����ĳ����
        /// </summary>
        public virtual bool IsMetadataEnabledFor(Type type)
        {
            return IsMetadataEnabled;
        }

        /// <summary>Ԫ�����Ƿ�����ĳ����
        /// </summary>
        public virtual bool IsMetadataEnabledFor(MethodInfo method)
        {
            return IsMetadataEnabled;
        }

        /// <summary>�Ƿ���ʽ����ĳ����
        /// </summary>
        public static bool IsExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsEnabledFor(type);
        }

        /// <summary>�Ƿ���ʽ����ĳ����
        /// </summary>
        public static bool IsExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type);
        }

        /// <summary>Ԫ�����Ƿ���ʾ����ĳ����
        /// </summary>
        public static bool IsMetadataExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsMetadataEnabledFor(type);
        }

        /// <summary>Ԫ�����Ƿ���ʾ����ĳ����
        /// </summary>
        public static bool IsMetadataExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsMetadataEnabledFor(type);
        }

        /// <summary>Ԫ�����Ƿ���ʾ����ĳ����
        /// </summary>
        public static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsMetadataEnabledFor(method);
        }

        /// <summary>Ԫ�����Ƿ���ʾ����ĳ����
        /// </summary>
        public static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsMetadataEnabledFor(method);
        }
    }
}
