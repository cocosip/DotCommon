using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class MimeTypeNameUtilTest
    {
        [Fact]
        public void GetMimeName_Test()
        {
            var actual = MimeTypeNameUtil.GetMimeName("Xls");
            Assert.Equal("application/vnd.ms-excel", actual);
        }
    }
}
