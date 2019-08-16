using DotCommon.Serializing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class DefaultBinarySerializerTest
    {
        [Fact]
        public void DefaultBinarySerializer_Test()
        {
            var o1 = new TestSerializeClass()
            {
                Id = 1,
                Name = "zhang",
                Age = 19
            };
            IBinarySerializer binarySerializer = new DefaultBinarySerializer();
            var binary1 = binarySerializer.Serialize(o1);

            var o2 = binarySerializer.Deserialize<TestSerializeClass>(binary1);

            Assert.Equal(o1.Id, o2.Id);
            Assert.Equal(o1.Name, o2.Name);
            Assert.Equal(o1.Age, o2.Age);

            var o3 = (TestSerializeClass)binarySerializer.Deserialize(binary1, typeof(TestSerializeClass));

            Assert.Equal(o1.Id, o3.Id);
            Assert.Equal(o1.Name, o3.Name);
            Assert.Equal(o1.Age, o3.Age);


        }

    }


}
