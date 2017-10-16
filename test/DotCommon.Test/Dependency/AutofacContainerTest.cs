using Autofac;
using DotCommon.Configurations;
using DotCommon.Dependency;
using DotCommon.Test.Dependency.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Dependency
{
    public class AutofacContainerTest
    {
        [Fact]
        public void AutofacContainer_Test()
        {
            var builder = new ContainerBuilder();
            Configurations.Configuration.Create()
                .UseAutofac(builder);

            var container = IocManager.GetContainer();
            container.Register<InterfaceContainerTest, ContainerTestImpl1>(DependencyLifeStyle.Transient, "impl1");
            container.Register<InterfaceContainerTest, ContainerTestImpl2>(DependencyLifeStyle.Transient, "impl2");
            Configuration.Instance.AutofacBuild();

            var impl1 = container.ResolveNamed<InterfaceContainerTest>("impl1");
            var impl2 = container.ResolveNamed<InterfaceContainerTest>("impl2");

            Assert.Equal("ContainerTestImpl1", impl1.GetName());
            Assert.Equal("ContainerTestImpl2", impl2.GetName());

        }

    }
}
