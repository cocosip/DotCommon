using AutoMapper;
using DotCommon.AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace DotCommon.Test.AutoMapper
{
    public class AutoMapperTest
    {
        static AutoMapperTest()
        {
            Mapper.Initialize(cfg =>
            {
                //自动映射
                AutoAttributeMapperHelper.CreateAutoAttributeMappings(new List<Assembly>()
                {
                    typeof(TestUser).Assembly
                }, cfg);
                //指定的映射
                AutoAttributeMapperHelper.CreateMappings(cfg, x =>
                {
                });
            });


        }

        [Fact]
        public void MapTest()
        {
            var testOrder = new TestOrder()
            {
                OrderId = 1,
                OrderNo = "101",
                UserName = "张三",
                PhoneNumber = "15868702111",
                UserId = 10
            };
            var user = testOrder.MapTo<TestUser>();
            Assert.Equal(10, user.UserId);
            Assert.Equal("15868702111", user.PhoneNumber);
            Assert.Equal("张三", user.UserName);
        }

    }
}
