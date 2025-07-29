using System;
using DotCommon.AutoMapper;
using DotCommon.ObjectMapping;
using Microsoft.Extensions.DependencyInjection;
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
                .AddAutoMapperObjectMapper()
                .AddLogging()
                .Configure<DotCommonAutoMapperOptions>(options =>
                {
                    options.AddMaps<AutoMapperTest>();
                });
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
            var user = objectMapper.Map<TestOrder,TestUser>(testOrder);
            Assert.Equal(10, user.UserId);
            Assert.Equal("15868702111", user.PhoneNumber);
            Assert.Equal("张三", user.UserName);
        }

    }
}
