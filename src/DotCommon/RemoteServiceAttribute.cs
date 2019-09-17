using DotCommon.Reflecting;
using System;
using System.Reflection;

namespace DotCommon
{
    /// <summary>远程服务特性标签
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class RemoteServiceAttribute : Attribute
    {
        /// <summary>是否启用,默认为启用(true)
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>是否启用元数据,默认为启用(true)
        /// </summary>
        public bool IsMetadataEnabled { get; set; }

        /// <summary>Ctor
        /// </summary>
        public RemoteServiceAttribute(bool isEnabled = true)
        {
            IsEnabled = isEnabled;
            IsMetadataEnabled = true;
        }

        /// <summary>是否启用某类型
        /// </summary>
        public virtual bool IsEnabledFor(Type type)
        {
            return IsEnabled;
        }


        /// <summary>是否启用某方法
        /// </summary>
        public virtual bool IsEnabledFor(MethodInfo method)
        {
            return IsEnabled;
        }

        /// <summary>元数据是否启用某类型
        /// </summary>
        public virtual bool IsMetadataEnabledFor(Type type)
        {
            return IsMetadataEnabled;
        }

        /// <summary>元数据是否启用某方法
        /// </summary>
        public virtual bool IsMetadataEnabledFor(MethodInfo method)
        {
            return IsMetadataEnabled;
        }

        /// <summary>是否显式启用某类型
        /// </summary>
        public static bool IsExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsEnabledFor(type);
        }

        /// <summary>是否显式禁用某类型
        /// </summary>
        public static bool IsExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type);
        }

        /// <summary>元数据是否显示启用某类型
        /// </summary>
        public static bool IsMetadataExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsMetadataEnabledFor(type);
        }

        /// <summary>元数据是否显示禁用某类型
        /// </summary>
        public static bool IsMetadataExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsMetadataEnabledFor(type);
        }

        /// <summary>元数据是否显示启用某方法
        /// </summary>
        public static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && !remoteServiceAttr.IsMetadataEnabledFor(method);
        }

        /// <summary>元数据是否显示禁用某方法
        /// </summary>
        public static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
            return remoteServiceAttr != null && remoteServiceAttr.IsMetadataEnabledFor(method);
        }
    }
}
