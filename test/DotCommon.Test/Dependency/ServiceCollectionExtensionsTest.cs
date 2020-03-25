using DotCommon.DependencyInjection;
using DotCommon.Test.Dependency.Dto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
            services1.WhenNull<IDependencyTestService>(s =>
           {
               s.AddTransient<IDependencyTest2Service, DependencyTest2Service>();
           });
            var provider1 = services1.BuildServiceProvider();
            var dependencyTest2Service1 = provider1.GetService<IDependencyTest2Service>();
            Assert.Equal("1000", dependencyTest2Service1.GetId());

            IServiceCollection services2 = new ServiceCollection();
            services2.AddTransient<IDependencyTestService, DependencyTestService>();
            services2.WhenNull(d =>
            {
                return d.ServiceType == typeof(IDependencyTestService) && d.ImplementationType == typeof(DependencyTestService);
            }, s =>
             {
                 s.AddTransient<IDependencyTestService, DependencyTestService>();
             });
            var provider2 = services2.BuildServiceProvider();
            var dependencyTestService2 = provider2.GetService<IDependencyTestService>();
            Assert.Equal("123", dependencyTestService2.GetName());

            IServiceCollection services3 = new ServiceCollection();
            services3.AddTransient<IDependencyTestService, DependencyTestService>();
            services3.WhenNull<IDependencyTestService>(s =>
           {
               s.AddTransient<IDependencyTest2Service, DependencyTest2Service>();
           });
            var provider3 = services3.BuildServiceProvider();
            var dependencyTest2Service3 = provider3.GetService<IDependencyTest2Service>();
            Assert.Null(dependencyTest2Service3);
        }

        [Fact]
        public void Replace_Test()
        {
            IServiceCollection services1 = new ServiceCollection();
            var d1 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface1));
            Assert.Null(d1);
            services1.Replace<IDependencyInterface1, DependencyImpl1>(ServiceLifetime.Transient);
            var d2 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface1));
            Assert.Equal(typeof(IDependencyInterface1), d2.ServiceType);
            Assert.Equal(typeof(DependencyImpl1), d2.ImplementationType);
            var count1 = services1.Where(x => x.ServiceType == typeof(IDependencyInterface1)).Count();
            Assert.Equal(1, count1);

            services1.Replace<IDependencyInterface1, DependencyImpl2>(ServiceLifetime.Singleton);
            var d3 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface1));
            Assert.Equal(typeof(DependencyImpl2), d3.ImplementationType);
            var count2 = services1.Where(x => x.ServiceType == typeof(IDependencyInterface1)).Count();
            Assert.Equal(1, count2);
        }

        [Fact]
        public void Replace_Type_Test()
        {
            IServiceCollection services1 = new ServiceCollection();
            var d1 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface1));
            Assert.Null(d1);
            services1.Replace(typeof(IDependencyInterface2<>), typeof(DependencyImpl3), ServiceLifetime.Transient);
            var d2 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface2<>));
            Assert.Equal(typeof(IDependencyInterface2<>), d2.ServiceType);
            Assert.Equal(typeof(DependencyImpl3), d2.ImplementationType);
            var count1 = services1.Where(x => x.ServiceType == typeof(IDependencyInterface2<>)).Count();
            Assert.Equal(1, count1);

            services1.Replace(typeof(IDependencyInterface2<>), typeof(DependencyImpl4), ServiceLifetime.Singleton);
            var d3 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface2<>));
            Assert.Equal(typeof(DependencyImpl4), d3.ImplementationType);
            var count2 = services1.Where(x => x.ServiceType == typeof(IDependencyInterface2<>)).Count();
            Assert.Equal(1, count2);


            services1.Replace<IDependencyInterface1, DependencyImpl1>(ServiceLifetime.Transient);
            var d4 = services1.FirstOrDefault(x => x.ServiceType == typeof(IDependencyInterface1));
            Assert.Equal(typeof(IDependencyInterface1), d4.ServiceType);
            Assert.Equal(typeof(DependencyImpl1), d4.ImplementationType);


            Assert.Throws<ArgumentException>(() =>
            {
                services1.Replace(typeof(IDependencyInterface2<>), typeof(DependencyImpl1), ServiceLifetime.Transient);
            });

        }
    }

    interface IDependencyInterface1
    {

    }

    interface IDependencyInterface2<T>
    {

    }

    class DependencyImpl1 : IDependencyInterface1
    {

    }

    class DependencyImpl2 : IDependencyInterface1
    {

    }

    class DependencyImpl3 : IDependencyInterface2<string>
    {

    }

    class DependencyImpl4 : IDependencyInterface2<int>
    {

    }
}
