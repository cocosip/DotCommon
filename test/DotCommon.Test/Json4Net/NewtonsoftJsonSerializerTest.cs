using DotCommon.DependencyInjection;
using DotCommon.Json4Net;
using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DotCommon.Test.Json4Net
{
    public class NewtonsoftJsonSerializerTest
    {
        private readonly Mock<IOptions<JsonSerializerSettings>> _mockJsonSerializerSettings;
        private readonly IServiceProvider _provider;

        public NewtonsoftJsonSerializerTest()
        {
            _mockJsonSerializerSettings = new Mock<IOptions<JsonSerializerSettings>>();
            _mockJsonSerializerSettings.SetupGet<JsonSerializerSettings>(x => x.Value).Returns(new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter> { new IsoDateTimeConverter() },
                ContractResolver = new CustomContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });

            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging()
                .AddDotCommon()
                .AddJson4Net();

            _provider = services.BuildServiceProvider();
            _provider.ConfigureDotCommon();
        }

        [Fact]
        public void Ctor_Test()
        {

            var jsonSerializer = new NewtonsoftJsonSerializer(_mockJsonSerializerSettings.Object);
            var containIsoDateTimeConverter = jsonSerializer.Settings.Converters.Any(x => x.GetType() == typeof(IsoDateTimeConverter));
            Assert.True(containIsoDateTimeConverter);
            Assert.Equal(typeof(CustomContractResolver), jsonSerializer.Settings.ContractResolver.GetType());
            Assert.Equal(ConstructorHandling.AllowNonPublicDefaultConstructor, jsonSerializer.Settings.ConstructorHandling);
        }

        [Fact]
        public void Serialize_Deserialize_Test()
        {

            var newtonsoftJsonSerializer = new NewtonsoftJsonSerializer(_mockJsonSerializerSettings.Object);
            Assert.Null(newtonsoftJsonSerializer.Serialize(null));

            var o1 = new NewtonsoftJsonSerializerClass1(3)
            {
                Id = 1,
                Name = "HaHa"
            };

            var json1 = newtonsoftJsonSerializer.Serialize(o1);
            var deserializeO1 = newtonsoftJsonSerializer.Deserialize(json1, typeof(NewtonsoftJsonSerializerClass1));

            Assert.Equal(typeof(NewtonsoftJsonSerializerClass1), deserializeO1.GetType());

            var jsonSerializer = _provider.GetService<IJsonSerializer>();
            var json2 = jsonSerializer.Serialize(o1);
            Assert.Equal(json1, json2);

            var deserializeO2 = newtonsoftJsonSerializer.Deserialize<NewtonsoftJsonSerializerClass1>(json1);
            Assert.Equal(o1.Id, deserializeO2.Id);
            Assert.Equal(o1.Name, deserializeO2.Name);
            Assert.Equal(o1.Age, deserializeO2.Age);
        }



        class NewtonsoftJsonSerializerClass1
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int Age { get; private set; }

            public NewtonsoftJsonSerializerClass1(int age)
            {
                Age = age;
            }

        }

    }
}
