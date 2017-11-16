using Autofac;
using DotCommon.Configurations;
using DotCommon.Dependency;
using DotCommon.Runtime.Caching;
using System.Threading;
using Xunit;

namespace DotCommon.Test.Caching
{
    public class CachingTest: TestBase
    {
        [Fact]
        public void Caching_Test()
        {
            var cacheManager = IocManager.GetContainer().Resolve<ICacheManager>();
            //获取具体某个cache值
            var user = cacheManager.GetCache("user").Get<long, AutoMapper.TestUser>(1, x => null);
            var order = cacheManager.GetCache("order").Get("100", x => "200");
            Assert.Null(user);
            Assert.Equal("200", order);
            cacheManager.GetCache("order").Set("100", "100");
            Assert.Equal("100", cacheManager.GetCache("order").Get("100", x => null));


        }

    }
}
