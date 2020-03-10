using DotCommon.Json4Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using Xunit;
using static DotCommon.Json4Net.NewtonsoftJsonSerializer;

namespace DotCommon.Test.Json4Net
{
    public class NewtonsoftJsonSerializerTest
    {
        [Fact]
        public void Ctor_Test()
        {
            var jsonSerializer = new NewtonsoftJsonSerializer();
            var containIsoDateTimeConverter = jsonSerializer.Settings.Converters.Any(x => x.GetType() == typeof(IsoDateTimeConverter));
            Assert.True(containIsoDateTimeConverter);
            Assert.Equal(typeof(CustomContractResolver), jsonSerializer.Settings.ContractResolver.GetType());
            Assert.Equal(ConstructorHandling.AllowNonPublicDefaultConstructor, jsonSerializer.Settings.ConstructorHandling);
        }

        [Fact]
        public void Serialize_Deserialize_Test()
        {

            var newtonsoftJsonSerializer = new NewtonsoftJsonSerializer();
            Assert.Null(newtonsoftJsonSerializer.Serialize(null));

            var o1 = new NewtonsoftJsonSerializerClass1(3)
            {
                Id = 1,
                Name = "HaHa"
            };

            var json1 = newtonsoftJsonSerializer.Serialize(o1);
            var deserializeO1 = newtonsoftJsonSerializer.Deserialize(json1, typeof(NewtonsoftJsonSerializerClass1));

            Assert.Equal(typeof(NewtonsoftJsonSerializerClass1), deserializeO1.GetType());

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
