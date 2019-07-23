using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.AutoMapper
{
    public static class AutoMapExtensions
    {
        private static readonly object SyncObject = new object();

        //已经被映射过的程序集
        private static readonly List<Assembly> MappedAssemblies = new List<Assembly>();

        /// <summary>根据Assembly注册AutoMapper自动映射
        /// </summary>
        public static void CreateAssemblyAutoMaps(this IMapperConfigurationExpression configurationExpression, params Assembly[] assemblies)
        {
            lock (SyncObject)
            {
                //未被映射过的程序集
                var notMappedAssemblies = assemblies.Where(x => !MappedAssemblies.Contains(x)).ToList();
                //创建映射
                FindAndAutoMapTypes(notMappedAssemblies, configurationExpression);
                //把这些映射添加到已经映射的程序集
                MappedAssemblies.AddRange(notMappedAssemblies);
            }
        }


        /// <summary>找到自动映射的类型,并且生成Mapper
        /// </summary>
        private static void FindAndAutoMapTypes(List<Assembly> assemblies, IMapperConfigurationExpression configurationExpression)
        {
            //全部类型
            var allTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                //获取程序集中自动映射的类型
                var autoAttributeTypies = assembly.GetTypes().Where(x =>
                {
                    var typeInfo = x.GetTypeInfo();
                    return typeInfo.IsDefined(typeof(AutoMapAttribute)) || typeInfo.IsDefined(typeof(AutoMapFromAttribute)) || typeInfo.IsDefined(typeof(AutoMapToAttribute));
                });
                allTypes.AddRange(autoAttributeTypies);
            }
            //遍历,并且把每个映射都添加进去
            foreach (var type in allTypes)
            {
                configurationExpression.CreateAutoAttributeMaps(type);
            }
        }

    }
}
