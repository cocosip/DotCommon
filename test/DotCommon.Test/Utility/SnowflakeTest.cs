using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class SnowflakeTest
    {
        [Fact]
        public void Constructor_ValidWorkerAndDatacenterId_ShouldInitializeCorrectly()
        {
            var snowflake = new Snowflake(1, 1);
            Assert.NotNull(snowflake);
        }

        [Fact]
        public void Constructor_CustomParameters_ShouldInitializeCorrectly()
        {
            long customEpoch = 1500000000000L; // July 14, 2017
            int workerIdBits = 3;
            int datacenterIdBits = 2;
            int sequenceBits = 10;
            long workerId = 5; // Max for 3 bits is 7
            long datacenterId = 2; // Max for 2 bits is 3

            var snowflake = new Snowflake(customEpoch, workerIdBits, datacenterIdBits, sequenceBits, workerId, datacenterId);
            Assert.NotNull(snowflake);
        }

        [Fact]
        public void Constructor_InvalidWorkerId_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Snowflake(32, 0)); // Default 5 bits for workerId
            Assert.Throws<ArgumentException>(() => new Snowflake(0, 0, 0, 0, -1, 0)); // Custom bits, invalid workerId
            Assert.Throws<ArgumentException>(() => new Snowflake(0, 3, 2, 10, 8, 0)); // Custom bits, workerId too large (max 7 for 3 bits)
        }

        [Fact]
        public void Constructor_InvalidDatacenterId_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Snowflake(0, 32)); // Default 5 bits for datacenterId
            Assert.Throws<ArgumentException>(() => new Snowflake(0, 0, 0, 0, 0, -1)); // Custom bits, invalid datacenterId
            Assert.Throws<ArgumentException>(() => new Snowflake(0, 3, 2, 10, 0, 4)); // Custom bits, datacenterId too large (max 3 for 2 bits)
        }

        [Fact]
        public void NextId_ShouldGenerateUniqueIds()
        {
            var snowflake = new Snowflake(1, 1);
            var ids = new HashSet<long>();
            for (int i = 0; i < 10000; i++)
            {
                ids.Add(snowflake.NextId());
            }
            Assert.Equal(10000, ids.Count);
        }

        [Fact]
        public void NextId_ShouldGenerateMonotonicallyIncreasingIds()
        {
            var snowflake = new Snowflake(1, 1);
            long lastId = 0;
            for (int i = 0; i < 10000; i++)
            {
                long currentId = snowflake.NextId();
                Assert.True(currentId > lastId);
                lastId = currentId;
            }
        }

        [Fact]
        public async Task NextId_ShouldHandleConcurrency()
        {
            var snowflake = new Snowflake(1, 1);
            var ids = new System.Collections.Concurrent.ConcurrentBag<long>();
            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        ids.Add(snowflake.NextId());
                    }
                }));
            }

            await Task.WhenAll(tasks.ToArray());
            Assert.Equal(10000, ids.Distinct().Count());
        }


    }
}