using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class SnowflakeTest
    {
        [Fact]
        public void Snowflake_Test()
        {
            var snowflake = new SnowflakeDistributeId(1);

            var v1 = snowflake.NextId();
            var v2 = snowflake.NextId();

            Assert.True(v1 > 0);
            Assert.True(v2 > 0);


        }
    }
}
