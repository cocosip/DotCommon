using Autofac;
using DotCommon.Configurations;
using DotCommon.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Autofac
{
    public static class ConfigurationExtensions
    {
        /// <summary>使用Autofac容器
        /// </summary>
        public static Configuration UseAutofac(this Configuration configuration, ContainerBuilder builder)
        {
            var container = new AutofacIocContainer(builder);
            IocManager.SetContainer(container);
            return configuration;
        }

        /// <summary>容器生效
        /// </summary>
        public static Configuration AutofacBuild(this Configuration configuration, IContainer container)
        {
            IocManager.GetContainer().UseEngine(container);
            return configuration;
        }
    }
}
