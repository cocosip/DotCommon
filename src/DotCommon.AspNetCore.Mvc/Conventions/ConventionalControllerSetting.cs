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
    public class ConventionalControllerSetting
    {
        public Assembly Assembly { get; }

        public HashSet<Type> ControllerTypes { get; }

        public string RootPath
        {
            get => _rootPath;
            set
            {
                _rootPath = value;
            }
        }
        private string _rootPath;

        public Func<Type, bool> TypePredicate { get; set; }

        public Action<ControllerModel> ControllerModelConfigurer { get; set; }

        public Func<UrlControllerNameNormalizerContext, string> UrlControllerNameNormalizer { get; set; }

        public Func<UrlActionNameNormalizerContext, string> UrlActionNameNormalizer { get; set; }

        public List<ApiVersion> ApiVersions { get; }

        public Action<ApiVersioningOptions> ApiVersionConfigurer { get; set; }

        public ConventionalControllerSetting(Assembly assembly, string rootPath)
        {
            //Check.NotNull(assembly, rootPath);

            Assembly = assembly;
            RootPath = rootPath;
            ControllerTypes = new HashSet<Type>();
            ApiVersions = new List<ApiVersion>();
        }

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