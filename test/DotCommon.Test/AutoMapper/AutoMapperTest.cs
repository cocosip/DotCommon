using AutoMapper;
using DotCommon.AutoMapper;
using DotCommon.DependencyInjection;
using DotCommon.ObjectMapping;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace DotCommon.Test.AutoMapper
{
    public class AutoMapperTest
    {
        static IServiceProvider _provider;
        static AutoMapperTest()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddDotCommon()
                .AddDotCommonAutoMapper()
                .AddAssemblyAutoMaps(typeof(TestUser).Assembly)
                .AddAutoMapperConfigurator(c =>
                {
                    //自定义的一些AutoMapper配置
                    //c.ApplyAutoMapperConfiguration()
                })
                .BuildAutoMapper();
            _provider = services.BuildServiceProvider();
        }

        [Fact]
        public void MapTest()
        {
            var objectMapper = _provider.GetService<ObjectMapping.IObjectMapper>();

            var testOrder = new TestOrder()
            {
                OrderId = 1,
                OrderNo = "101",
                UserName = "张三",
                PhoneNumber = "15868702111",
                UserId = 10
            };
            var user = objectMapper.Map<TestUser>(testOrder);
            Assert.Equal(10, user.UserId);
            Assert.Equal("15868702111", user.PhoneNumber);
            Assert.Equal("张三", user.UserName);


            var nullMapper = NullObjectMapper.Instance;
            Assert.Throws<ArgumentException>(() =>
            {
                nullMapper.Map<TestUser>(testOrder);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                nullMapper.Map<TestOrder, TestUser>(testOrder, user);
            });
        }

    }
}
