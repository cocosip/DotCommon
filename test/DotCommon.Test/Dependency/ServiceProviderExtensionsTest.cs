using DotCommon.DependencyInjection;
using DotCommon.Test.Dependency.Dto;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
namespace DotCommon.Test.Dependency
{
    public class ServiceProviderExtensionsTest
    {
        [Fact]
        public void GetServiceByArgsTest()
        {
            IServiceCollection services = new ServiceCollection();

            var provider = services.BuildServiceProvider();
            var getServiceByArgsTestClass = provider.GetServiceByArgs<GetServiceByArgsTestClass>(1, "张三");
            Assert.Equal(1, getServiceByArgsTestClass.Id);
            Assert.Equal("张三", getServiceByArgsTestClass.Name);
        }

        [Fact]
        public void GetServiceByInjectArgsTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<GetServiceByInjectAndArgsTestInjectClass>();
            services.AddTransient<GetServiceByInjectAndArgsTestClass>();
            var provider = services.BuildServiceProvider();
            var getServiceByInjectAndArgsTestClass = provider.GetServiceByArgs<GetServiceByInjectAndArgsTestClass>(1, "张三");
            Assert.Equal("Hello,Id:1,Name:张三", getServiceByInjectAndArgsTestClass.GetInfo());
        }

    }
}
