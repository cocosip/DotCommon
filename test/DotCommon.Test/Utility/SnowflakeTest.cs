using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
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
            var v3 = SnowflakeDistributeId.GenerateNextId();

            Assert.True(v1 > 0);
            Assert.True(v2 > 0);
            Assert.True(v3 > 0);

            var snowflake2 = new SnowflakeDistributeId(0L, 0L);
            var v2_1 = snowflake2.NextId();
            var v2_2 = snowflake2.NextId();

            Assert.True(v2_1 > 0);
            Assert.True(v2_2 > 0);

            Assert.Throws<ArgumentException>(() =>
            {
                var snowflake3 = new SnowflakeDistributeId(1000);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                var snowflake3 = new SnowflakeDistributeId(1L, 300);
            });

            Assert.Throws<InvalidTimeZoneException>(() =>
            {
                var snowflake3 = new SnowflakeDistributeId(0L, 0L);
                var lastTimestampValue = (long)(DateTime.Now.AddDays(10) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                BindingFlags flag = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
                FieldInfo f_lastTimestamp = typeof(SnowflakeDistributeId).GetField("lastTimestamp", flag);
                f_lastTimestamp.SetValue(snowflake3, lastTimestampValue);
                var r = snowflake3.NextId();

            });

            {
                BindingFlags flag = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
                FieldInfo f_lastTimestamp = typeof(SnowflakeDistributeId).GetField("lastTimestamp", flag);
                var lastTimestamp = f_lastTimestamp.GetValue(snowflake);

                MethodInfo method = snowflake.GetType().GetMethod("TilNextMillis", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Instance);
                var next = Convert.ToInt64(method.Invoke(snowflake, new object[] { lastTimestamp }));
                Assert.True(next > 0);
            }
        }



    }
}
