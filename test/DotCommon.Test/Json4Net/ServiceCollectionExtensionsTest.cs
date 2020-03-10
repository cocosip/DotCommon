using DotCommon.DependencyInjection;
using DotCommon.Json4Net;
using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.Json4Net
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddJson4Net_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddDotCommon()
                .AddJson4Net();

            var provider = services.BuildServiceProvider();
            var jsonSerializer = provider.GetService<IJsonSerializer>();
            Assert.Equal(typeof(NewtonsoftJsonSerializer), jsonSerializer.GetType());


        }
    }
}
