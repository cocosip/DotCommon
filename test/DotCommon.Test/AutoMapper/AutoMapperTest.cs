using AutoMapper;
using DotCommon.AutoMapper;
using DotCommon.DependencyInjection;
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
                .AddDotCommonAutoMapper();
            var config = new MapperConfiguration(cfg =>
            {
                AutoAttributeMapperHelper.CreateAutoAttributeMappings(new List<Assembly>()
                {
                   typeof(TestUser).Assembly
                }, cfg);
            });

            services.AddSingleton<IConfigurationProvider>(config);
            services.AddSingleton<IMapper>(config.CreateMapper());
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
        }

    }
}
