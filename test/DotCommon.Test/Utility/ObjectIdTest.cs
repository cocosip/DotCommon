using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ObjectIdTest
    {
        [Fact]
        public void GenerateNewStringIdTest()
        {
            var objectId = ObjectId.GenerateNewStringId();
            Assert.Equal(24, objectId.Length);
        }
    }
}
