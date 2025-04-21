using System;
using DotCommon.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class DefaultJsonSerializerTest
    {
        private IServiceProvider _provider;
        public DefaultJsonSerializerTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDotCommon();
            _provider = services.BuildServiceProvider();
        }

        [Fact]
        public void DefaultJsonSerializer_Test()
        {
            var o1 = new TestSerializeClass()
            {
                Id = 1,
                Name = "zhang",
                Age = 19
            };
            var jsonSerializer = _provider.GetRequiredService<IJsonSerializer>();
            var json1 = jsonSerializer.Serialize(o1);

            var o2 = jsonSerializer.Deserialize<TestSerializeClass>(json1);

            Assert.Equal(o1.Id, o2.Id);
            Assert.Equal(o1.Name, o2.Name);
            Assert.Equal(o1.Age, o2.Age);

            var o3 = jsonSerializer.Deserialize<TestSerializeClass>(json1);

            Assert.Equal(o1.Id, o3.Id);
            Assert.Equal(o1.Name, o3.Name);
            Assert.Equal(o1.Age, o3.Age);

        }
    }
}
