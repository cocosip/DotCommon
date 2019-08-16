using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class HttpMethodUtilTest
    {
        [Fact]
        public void ConventionalPrefixes_Test()
        {
            var prefixes = HttpMethodUtil.ConventionalPrefixes;
            Assert.Equal(5, prefixes.Count);
            var expected = new string[] { "GetList", "GetAll", "Get" };
            Assert.Equal(expected, prefixes["GET"]);

            var acutalMethod = HttpMethodUtil.RemoveHttpMethodPrefix("InsertIntoDataBase", "POST");
            Assert.Equal("IntoDataBase", acutalMethod);

            var acutalMethod2 = HttpMethodUtil.RemoveHttpMethodPrefix("CreateUser", "Get");
            Assert.Equal("CreateUser", acutalMethod2);
        }

        [Fact]
        public void GetConventionalVerbForMethodName_Test()
        {
            Assert.Equal("GET", HttpMethodUtil.GetConventionalVerbForMethodName("GetList"));
            Assert.Equal("GET", HttpMethodUtil.GetConventionalVerbForMethodName("gEt"));
            Assert.Equal("POST", HttpMethodUtil.GetConventionalVerbForMethodName("Curl"));
            Assert.Equal("POST", HttpMethodUtil.GetConventionalVerbForMethodName("Insert"));
        }

        [Fact]
        public void ConvertToHttpMethod_Test()
        {
            Assert.Equal(HttpMethod.Get, HttpMethodUtil.ConvertToHttpMethod("get"));
            Assert.Equal(HttpMethod.Post, HttpMethodUtil.ConvertToHttpMethod("post"));
            Assert.Equal(HttpMethod.Put, HttpMethodUtil.ConvertToHttpMethod("Put"));
            Assert.Equal(HttpMethod.Delete, HttpMethodUtil.ConvertToHttpMethod("Delete"));
            Assert.Equal(HttpMethod.Options, HttpMethodUtil.ConvertToHttpMethod("Options"));
            Assert.Equal(HttpMethod.Trace, HttpMethodUtil.ConvertToHttpMethod("Trace"));
            Assert.Equal(HttpMethod.Head, HttpMethodUtil.ConvertToHttpMethod("Head"));
            Assert.Equal(HttpMethod.Patch, HttpMethodUtil.ConvertToHttpMethod("PATCH"));
            Assert.Throws<ArgumentException>(() =>
            {
                HttpMethodUtil.ConvertToHttpMethod("Gud");
            });


        }


    }
}
