using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DotCommon.Extensions;

namespace DotCommon.Test.Extensions
{
    public class DictionaryExtensionsTest
    {
        [Fact]
        public void TryGetValue_Test()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("1", "hello");
            dict.Add("2", 2);

            var r1 = dict.TryGetValue<string>("1", out string v1);
            Assert.True(r1);
            Assert.Equal("hello", v1);

            var r2 = dict.TryGetValue<int>("3", out int v2);
            Assert.False(r2);
            Assert.Equal(0, v2);

        }


        [Fact]
        public void GetOrDefault_Test()
        {
            var dict1 = new Dictionary<string, int>();
            dict1.Add("1", 100);
            dict1.Add("2", 200);
            var v1 = dict1.GetOrDefault("1", true);
            Assert.Equal(100, v1);

            var dict2 = new Dictionary<int, string>();
            dict2.Add(10, "10");
            dict2.Add(22, "220");

            var v2 = dict2.GetOrDefault(10, true);
            var v3 = dict2.GetOrDefault(22, false);
            var v4 = dict2.GetOrDefault(50, true);
            Assert.Equal("10", v2);
            Assert.Equal("220", v3);
            Assert.Null(v4);
        }

        [Fact]
        public void GetOrAdd_Test()
        {
            var dict1 = new Dictionary<int, string>();
            var v1 = dict1.GetOrAdd(1, k => "111");
            Assert.Equal("111", v1);

            var v2 = dict1.GetOrAdd(1, k => "2222");
            Assert.Equal("111", v2);

            var v3 = dict1.GetOrAdd(2, () => "222");
            Assert.Equal("222", v3);

            var v4 = dict1.GetOrAdd(2, () => "333");
            Assert.Equal("222", v4);
        }

        [Fact]
        public void Remove_Test()
        {
            var dict1 = new Dictionary<int, string>();
            dict1.Add(1, "100");
            dict1.Add(2, "200");

            var r1 = dict1.Remove(1);
            Assert.True(r1);

            var r2 = dict1.Remove(1, out string v1);
            Assert.False(r2);
            Assert.Null(v1);

            var r3 = dict1.Remove(2, out string v2);
            Assert.True(r3);
            Assert.Equal("200", v2);

        }

    }
}
