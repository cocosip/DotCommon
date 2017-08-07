using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.AutoMapper
{
    public class AutoAttributeAutoMapperHelper
    {
        private static readonly object SyncObj = new object();
        //已经被映射过的程序集
        private static List<Assembly> MappedAssemblies = new List<Assembly>();


        //private void Test()
        //{
        //    Mapper.Initialize(cfg =>
        //    {
        //        //自动映射
        //        AutoAttributeAutoMapperHelper.CreateAutoAttributeMappings(new List<Assembly>(), cfg);
        //        AutoAttributeAutoMapperHelper.CreateMappings(cfg, x =>
        //        {
        //        });
        //    });
        //}

        /// <summary>创建映射
        /// </summary>
        public static void CreateMappings(IMapperConfigurationExpression configuration, Action<IMapperConfigurationExpression> action)
        {
            action?.Invoke(configuration);
        }


        /// <summary>添加自动属性的映射
        /// </summary>
        public static void CreateAutoAttributeMappings(List<Assembly> assemblies, IMapperConfigurationExpression configuration)
        {
            lock (SyncObj)
            {
                //未被映射过的程序集
                var notMappedAssemblies = assemblies.Where(x => !MappedAssemblies.Contains(x));
                //创建映射
                FindAndAutoMapTypes(assemblies, configuration);
            }
        }

        /// <summary>找到自动映射的类型,并且生成Mapper
        /// </summary>
        private static void FindAndAutoMapTypes(List<Assembly> assemblies, IMapperConfigurationExpression configuration)
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
                configuration.CreateAutoAttributeMaps(type);
            }
        }
    }
}
