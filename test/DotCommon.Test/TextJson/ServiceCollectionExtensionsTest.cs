using DotCommon.DependencyInjection;
using DotCommon.Serializing;
using DotCommon.TextJson;
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
                .AddDotCommon()
                .AddTextJson();

            var provider = services.BuildServiceProvider();
            var jsonSerializer = provider.GetService<IJsonSerializer>();
            Assert.Equal(typeof(TextJsonSerializer), jsonSerializer.GetType());
        }
    }
}
