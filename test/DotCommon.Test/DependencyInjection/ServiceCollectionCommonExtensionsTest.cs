using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;


namespace DotCommon.Test.DependencyInjection
{
    public class ServiceCollectionCommonExtensionsTest
    {
        [Fact]
        public void GetSingletonInstanceOrNull_Test()
        {
            IServiceCollection services = new ServiceCollection();
            var o1 = services.GetSingletonInstanceOrNull<SingletonTestClass>();
            Assert.Null(o1);

            services.AddSingleton(new SingletonTestClass()
            {
                Id = 1,
                Name = "zhangsan"
            });

            var o2 = services.GetSingletonInstanceOrNull<SingletonTestClass>();
            Assert.Equal(1, o2.Id);
            Assert.Equal("zhangsan", o2.Name);

        }

        [Fact]
        public void GetSingletonInstance_Test()
        {
            IServiceCollection services = new ServiceCollection();
            Assert.Throws<InvalidOperationException>(() =>
            {
                var o1 = services.GetSingletonInstance<SingletonTestClass>();
            });
            services.AddSingleton(new SingletonTestClass()
            {
                Id = 2,
                Name = "lisi"
            });
            var o2 = services.GetSingletonInstance<SingletonTestClass>();
            Assert.Equal(2, o2.Id);
            Assert.Equal("lisi", o2.Name);
        }

        [Fact]
        public void IsAdded_Generic_ShouldReturnTrue_WhenServiceAdded()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<SingletonTestClass>();

            Assert.True(services.IsAdded<SingletonTestClass>());
        }

        [Fact]
        public void IsAdded_Generic_ShouldReturnFalse_WhenServiceNotAdded()
        {
            IServiceCollection services = new ServiceCollection();

            Assert.False(services.IsAdded<SingletonTestClass>());
        }

        [Fact]
        public void IsAdded_Type_ShouldReturnTrue_WhenServiceAdded()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(SingletonTestClass));

            Assert.True(services.IsAdded(typeof(SingletonTestClass)));
        }

        [Fact]
        public void IsAdded_Type_ShouldReturnFalse_WhenServiceNotAdded()
        {
            IServiceCollection services = new ServiceCollection();

            Assert.False(services.IsAdded(typeof(SingletonTestClass)));
        }

        [Fact]
        public void BuildServiceProviderFromFactory_WithoutFactory_ShouldBuildDefaultProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<SingletonTestClass>();

            var provider = services.BuildServiceProviderFromFactory();

            Assert.NotNull(provider);
            var service = provider.GetService<SingletonTestClass>();
            Assert.NotNull(service);
        }

        [Fact]
        public void BuildServiceProviderFromFactory_Generic_WithoutFactory_ShouldThrow()
        {
            IServiceCollection services = new ServiceCollection();

            Assert.Throws<DotCommon.DotCommonException>(() =>
                services.BuildServiceProviderFromFactory<TestContainerBuilder>());
        }
    }

    public class SingletonTestClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class TestContainerBuilder
    {
        public bool Configured { get; set; }
    }
}
