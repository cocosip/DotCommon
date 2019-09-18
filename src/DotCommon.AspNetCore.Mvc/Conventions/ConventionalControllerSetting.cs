using DotCommon.Extensions;
using DotCommon.Reflecting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.AspNetCore.Mvc.Conventions
{
    /// <summary>常规控制器设置
    /// </summary>
    public class ConventionalControllerSetting
    {
        /// <summary>程序集
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>控制器类型
        /// </summary>
        public HashSet<Type> ControllerTypes { get; }

        /// <summary>根路径
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>类型表达式
        /// </summary>
        public Func<Type, bool> TypePredicate { get; set; }

        /// <summary>控制器实体设置委托
        /// </summary>
        public Action<ControllerModel> ControllerModelConfigurer { get; set; }

        /// <summary>Url控制器名标准
        /// </summary>
        public Func<UrlControllerNameNormalizerContext, string> UrlControllerNameNormalizer { get; set; }

        /// <summary>UrlAction名标准
        /// </summary>
        public Func<UrlActionNameNormalizerContext, string> UrlActionNameNormalizer { get; set; }

        /// <summary>Api版本
        /// </summary>
        public List<ApiVersion> ApiVersions { get; }

        /// <summary>Api版本配置
        /// </summary>
        public Action<ApiVersioningOptions> ApiVersionConfigurer { get; set; }

        /// <summary>Ctor
        /// </summary>
        public ConventionalControllerSetting(Assembly assembly, string rootPath)
        {
            Assembly = assembly;
            RootPath = rootPath;
            ControllerTypes = new HashSet<Type>();
            ApiVersions = new List<ApiVersion>();
        }

        /// <summary>初始化
        /// </summary>
        public void Initialize()
        {
            var types = Assembly.GetTypes()
                .Where(IsRemoteService)
                .WhereIf(TypePredicate != null, TypePredicate);

            foreach (var type in types)
            {
                ControllerTypes.Add(type);
            }
        }

        private static bool IsRemoteService(Type type)
        {
            if (!type.IsPublic || type.IsAbstract || type.IsGenericType)
            {
                return false;
            }

            var remoteServiceAttr = ReflectionUtil.GetSingleAttributeOrDefault<RemoteServiceAttribute>(type);
            if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
            {
                return false;
            }

            if (typeof(IRemoteService).IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }
    }
}
