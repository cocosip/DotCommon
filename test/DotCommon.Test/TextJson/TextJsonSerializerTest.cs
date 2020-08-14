using DotCommon.DependencyInjection;
using DotCommon.Serializing;
using DotCommon.TextJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using Xunit;

namespace DotCommon.Test.TextJson
{
    public class TextJsonSerializerTest
    {
        private readonly Mock<IOptions<JsonSerializerOptions>> _mockJsonSerializerOptions;
        private readonly IServiceProvider _provider;
        public TextJsonSerializerTest()
        {

            _mockJsonSerializerOptions = new Mock<IOptions<JsonSerializerOptions>>();
            _mockJsonSerializerOptions.SetupGet<JsonSerializerOptions>(x => x.Value).Returns(new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging()
                .AddDotCommon()
                .AddTextJson();

            _provider = services.BuildServiceProvider();
            _provider.ConfigureDotCommon();
        }

        [Fact]
        public void Ctor_Test()
        {
            var jsonSerializer = new TextJsonSerializer(_mockJsonSerializerOptions.Object);
            Assert.False(jsonSerializer.Options.Converters.Any());
            Assert.Equal(JavaScriptEncoder.UnsafeRelaxedJsonEscaping, jsonSerializer.Options.Encoder);
        }

        [Fact]
        public void Serialize_Deserialize_Test()
        {

            var textJsonSerializer = new TextJsonSerializer(_mockJsonSerializerOptions.Object);
            // Assert.Null(textJsonSerializer.Serialize(s1));
            Assert.Equal("null", textJsonSerializer.Serialize(null));

            var o1 = new TextJsonSerializerClass1()
            {
                Age = 3,
                Id = 1,
                Name = "HaHa"
            };

            var json1 = textJsonSerializer.Serialize(o1);
            var deserializeO1 = textJsonSerializer.Deserialize(json1, typeof(TextJsonSerializerClass1));

            Assert.Equal(typeof(TextJsonSerializerClass1), deserializeO1.GetType());

            var jsonSerializer = _provider.GetService<IJsonSerializer>();
            var json2 = jsonSerializer.Serialize(o1);
            Assert.Equal(json1, json2);

            var deserializeO2 = textJsonSerializer.Deserialize<TextJsonSerializerClass1>(json1);
            Assert.Equal(o1.Id, deserializeO2.Id);
            Assert.Equal(o1.Name, deserializeO2.Name);
            Assert.Equal(o1.Age, deserializeO2.Age);
        }


    }

    public class TextJsonSerializerClass1
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public TextJsonSerializerClass1()
        {

        }

        //public TextJsonSerializerClass1(int age)
        //{
        //    Age = age;
        //}
    }
}
