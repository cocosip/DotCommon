using DotCommon.Alg;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Alg
{
    public class Sha1AlgTest
    {
        [Fact]
        public void GetStringMd5HashTest()
        {
            var str = "abcdefg&!@#12233";
            Assert.Equal("4dc660c2cf9dbed0488139de346e26de62f9fb38", Sha1Alg.GetStringSha1Hash(str).ToLower());
        }
    }
}
