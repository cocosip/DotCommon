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
    /// <summary>�������������
    /// </summary>
    public class ConventionalControllerSetting
    {
        /// <summary>����
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>����������
        /// </summary>
        public HashSet<Type> ControllerTypes { get; }

        /// <summary>��·��
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>���ͱ��ʽ
        /// </summary>
        public Func<Type, bool> TypePredicate { get; set; }

        /// <summary>������ʵ������ί��
        /// </summary>
        public Action<ControllerModel> ControllerModelConfigurer { get; set; }

        /// <summary>Url����������׼
        /// </summary>
        public Func<UrlControllerNameNormalizerContext, string> UrlControllerNameNormalizer { get; set; }

        /// <summary>UrlAction����׼
        /// </summary>
        public Func<UrlActionNameNormalizerContext, string> UrlActionNameNormalizer { get; set; }

        /// <summary>Api�汾
        /// </summary>
        public List<ApiVersion> ApiVersions { get; }

        /// <summary>Api�汾����
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

        /// <summary>��ʼ��
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
