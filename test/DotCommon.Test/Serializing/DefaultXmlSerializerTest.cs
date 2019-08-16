using DotCommon.Serializing;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class DefaultXmlSerializerTest
    {
        [Fact]
        public void DefaultXmlSerializer_Test()
        {
            var o1 = new TestSerializeClass()
            {
                Id = 1,
                Name = "zhang",
                Age = 19
            };
            IXmlSerializer xmlSerializer = new DefaultXmlSerializer();
            var xml1 = xmlSerializer.Serialize(o1);

            var o2 = xmlSerializer.Deserialize<TestSerializeClass>(xml1);

            Assert.Equal(o1.Id, o2.Id);
            Assert.Equal(o1.Name, o2.Name);
            Assert.Equal(o1.Age, o2.Age);

            var o3 = (TestSerializeClass)xmlSerializer.Deserialize(xml1, typeof(TestSerializeClass));

            Assert.Equal(o1.Id, o3.Id);
            Assert.Equal(o1.Name, o3.Name);
            Assert.Equal(o1.Age, o3.Age);

        }
    }
}
