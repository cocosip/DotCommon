using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using DotCommon.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace DotCommon.Test.TextJson
{
    public class TextJsonSerializerTest
    {
 
        private readonly IServiceProvider _provider;
        public TextJsonSerializerTest()
        {

           

            IServiceCollection services = new ServiceCollection();
            services
                .AddLogging()
                .AddDotCommon();
            _provider = services.BuildServiceProvider();
        }

        [Fact]
        public void Serialize_Deserialize_Test()
        {

            var textJsonSerializer = _provider.GetRequiredService<IJsonSerializer>();
            // Assert.Null(textJsonSerializer.Serialize(s1));
            Assert.Equal("null", textJsonSerializer.Serialize(null));

            var o1 = new TextJsonSerializerClass1()
            {
                Age = 3,
                Id = 1,
                Name = "HaHa"
            };

            var json1 = textJsonSerializer.Serialize(o1);
            var deserializeO1 = textJsonSerializer.Deserialize<TextJsonSerializerClass1>(json1);

            Assert.Equal(typeof(TextJsonSerializerClass1), deserializeO1.GetType());

            var jsonSerializer = _provider.GetService<IJsonSerializer>();
            var json2 = jsonSerializer.Serialize(o1);
            Assert.Equal(json1, json2);

            var deserializeO2 = textJsonSerializer.Deserialize<TextJsonSerializerClass1>(json1);
            Assert.Equal(o1.Id, deserializeO2.Id);
            Assert.Equal(o1.Name, deserializeO2.Name);
            Assert.Equal(o1.Age, deserializeO2.Age);
        }

        [Fact]
        public void Generics_Serialize_Deserialize_Test()
        {
            var textJsonSerializer = _provider.GetRequiredService<IJsonSerializer>();

            var o = new JsonTestResult<JsonTestResultItem>()
            {
                Success = true,
                Data = new JsonTestResultItem()
                {
                    Id = "1",
                    Name = "virtual"
                }
            };

            var json = textJsonSerializer.Serialize(o);

            var o2 = textJsonSerializer.Deserialize<JsonTestResult<JsonTestResultItem>>(json);

            Assert.Equal(o.Success, o2.Success);
            Assert.Equal(o.Data.Id, o2.Data.Id);
            Assert.Equal(o.Data.Name, o2.Data.Name);
        }

        /// <summary>
        /// 依赖注入获取对象
        /// </summary>
        [Fact]
        public void DependencyInjection_Serialize_Deserialize_Test()
        {
            var serializer = _provider.GetService<IJsonSerializer>();

            var o = new JsonTestResult<JsonTestResultItem>()
            {
                Success = true,
                Data = new JsonTestResultItem()
                {
                    Id = "1",
                    Name = "virtual"
                }
            };

            var json = serializer.Serialize(o);

            var o2 = serializer.Deserialize<JsonTestResult<JsonTestResultItem>>(json);

            Assert.Equal(o.Success, o2.Success);
            Assert.Equal(o.Data.Id, o2.Data.Id);
            Assert.Equal(o.Data.Name, o2.Data.Name);
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

    public class JsonTestResult<T>
    {
        public bool Success { get; set; }

        public T Data { get; set; }
    }

    public class JsonTestResultItem
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }


}
