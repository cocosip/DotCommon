using DotCommon.Json;
using DotCommon.Json.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.TextJson
{
    public class ServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddTextJson_Test()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddDotCommon();

            var provider = services.BuildServiceProvider();
            var jsonSerializer = provider.GetService<IJsonSerializer>();
            Assert.Equal(typeof(DotCommonSystemTextJsonSerializer), jsonSerializer.GetType());
        }
    }
}
