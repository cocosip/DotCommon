using System.Net.Http;
using Xunit;
using DotCommon.Utility;

namespace DotCommon.Test.Utility
{
    public class HttpMethodUtilTest
    {
        [Theory]
        [InlineData("GetUser", "GET")]
        [InlineData("UpdateProduct", "PUT")]
        [InlineData("DeleteUser", "DELETE")]
        [InlineData("CreateOrder", "POST")]
        [InlineData("PatchSomething", "PATCH")]
        [InlineData("MyCustomMethod", "POST")] // Default case
        [InlineData("", "POST")]
        [InlineData(null, "POST")]
        public void GetConventionalVerbForMethodName_ShouldReturnCorrectVerb(string methodName, string expectedVerb)
        {
            Assert.Equal(expectedVerb, HttpMethodUtil.GetConventionalVerbForMethodName(methodName));
        }

        [Theory]
        [InlineData("GetUserDetails", "GET", "UserDetails")]
        [InlineData("UpdateProductInfo", "PUT", "ProductInfo")]
        [InlineData("RemoveItem", "DELETE", "Item")]
        [InlineData("PostComment", "POST", "Comment")]
        [InlineData("NonConventional", "GET", "NonConventional")]
        public void RemoveHttpMethodPrefix_ShouldRemovePrefixCorrectly(string methodName, string httpMethod, string expected)
        {
            Assert.Equal(expected, HttpMethodUtil.RemoveHttpMethodPrefix(methodName, httpMethod));
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("DELETE")]
        [InlineData("OPTIONS")]
        [InlineData("TRACE")]
        [InlineData("HEAD")]
        [InlineData("PATCH")]
        public void ConvertToHttpMethod_ShouldConvertKnownMethods(string method)
        {
            var httpMethod = HttpMethodUtil.ConvertToHttpMethod(method);
            Assert.Equal(method, httpMethod.Method);
        }

        [Fact]
        public void ConvertToHttpMethod_ShouldThrowForUnknownMethod()
        {
            Assert.Throws<System.ArgumentException>(() => HttpMethodUtil.ConvertToHttpMethod("UNKNOWN"));
        }
    }
}