using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
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


    }

    public class SingletonTestClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
