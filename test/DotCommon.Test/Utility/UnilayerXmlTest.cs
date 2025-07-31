using Xunit;
using DotCommon.Utility;

namespace DotCommon.Test.Utility
{
    public class UnilayerXmlTest
    {
        [Fact]
        public void Constructor_ShouldParseXmlCorrectly()
        {
            string xml = "<xml><key1>value1</key1><key2><![CDATA[value2]]></key2></xml>";
            var unilayerXml = new UnilayerXml(xml);

            Assert.Equal("value1", unilayerXml.GetValue("key1"));
            Assert.Equal("value2", unilayerXml.GetValue("key2"));
        }

        [Fact]
        public void SetAndGetValue_ShouldWorkCorrectly()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetValue("stringKey", "hello");
            unilayerXml.SetValue("intKey", 123);

            Assert.Equal("hello", unilayerXml.GetValue("stringKey"));
            Assert.Equal(123, unilayerXml.GetValue("intKey"));
            Assert.Null(unilayerXml.GetValue("nonExistentKey"));
        }

        [Fact]
        public void GetValue_Generic_ShouldReturnTypedValue()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetValue("myString", "world");
            unilayerXml.SetValue("myInt", 456);

            Assert.Equal("world", unilayerXml.GetValue<string>("myString"));
            Assert.Equal(456, unilayerXml.GetValue<int>("myInt"));
            Assert.Equal(default(string), unilayerXml.GetValue<string>("wrongType"));
        }

        [Fact]
        public void HasValue_ShouldReturnCorrectStatus()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetValue("a", 1);

            Assert.True(unilayerXml.HasValue("a"));
            Assert.False(unilayerXml.HasValue("b"));
        }

        [Fact]
        public void ToXml_ShouldGenerateCorrectXml()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetValue("amount", 100);
            unilayerXml.SetValue("description", "A test product");

            string expectedXml = "<xml><amount>100</amount><description><![CDATA[A test product]]></description></xml>";
            Assert.Equal(expectedXml, unilayerXml.ToXml());
        }

        [Fact]
        public void ToXml_WithCustomRoot_ShouldGenerateCorrectXml()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetRootNodeName("request");
            unilayerXml.SetValue("id", "xyz");

            string expectedXml = "<request><id><![CDATA[xyz]]></id></request>";
            Assert.Equal(expectedXml, unilayerXml.ToXml());
        }

        [Fact]
        public void RemoveAndClear_ShouldModifyDictionary()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetValue("a", 1);
            unilayerXml.SetValue("b", 2);

            unilayerXml.RemoveValue("a");
            Assert.False(unilayerXml.HasValue("a"));
            Assert.True(unilayerXml.HasValue("b"));

            unilayerXml.Clear();
            Assert.False(unilayerXml.HasValue("b"));
            Assert.Empty(unilayerXml.GetValues());
        }
    }
}
