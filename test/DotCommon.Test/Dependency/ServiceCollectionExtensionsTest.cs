using DotCommon.DependencyInjection;
using DotCommon.Test.Dependency.Dto;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
namespace DotCommon.Test.Dependency
{
    public class ServiceCollectionExtensionsTest
    {

        /// <summary>判断是否注册的测试
        /// </summary>
        [Fact]
        public void AddServiceIfNotRegister_Test()
        {
            IServiceCollection services1 = new ServiceCollection();
            services1.AddServiceWhenNull<IDependencyTestService>(ServiceLifetime.Transient, s =>
            {
                s.AddTransient<IDependencyTest2Service, DependencyTest2Service>();
            });
            var provider1 = services1.BuildServiceProvider();
            var dependencyTest2Service1 = provider1.GetService<IDependencyTest2Service>();
            Assert.Equal("1000", dependencyTest2Service1.GetId());

            IServiceCollection services2 = new ServiceCollection();
            services2.AddTransient<IDependencyTestService, DependencyTestService>();
            services2.AddServiceWhenNull(d =>
            {
                return d.ServiceType == typeof(IDependencyTestService)&&d.ImplementationType==typeof(DependencyTestService);
            }, s =>
             {
                 s.AddTransient<IDependencyTestService, DependencyTestService>();
             });
            var provider2 = services2.BuildServiceProvider();
            var dependencyTestService2 = provider2.GetService<IDependencyTestService>();
            Assert.Equal("123", dependencyTestService2.GetName());

            IServiceCollection services3 = new ServiceCollection();
            services3.AddTransient<IDependencyTestService, DependencyTestService>();
            services3.AddServiceWhenNull<IDependencyTestService>(ServiceLifetime.Transient, s =>
            {
                s.AddTransient<IDependencyTest2Service, DependencyTest2Service>();
            });
            var provider3 = services3.BuildServiceProvider();
            var dependencyTest2Service3 = provider3.GetService<IDependencyTest2Service>();
            Assert.Null(dependencyTest2Service3);
        }
    }
}
