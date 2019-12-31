using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DotCommon.Extensions;

namespace DotCommon.Test.Extensions
{
    public class CollectionExtensionsTest
    {
        [Fact]
        public void IsNullOrEmpty_Test()
        {
            ICollection<int> collections = null;
            Assert.True(collections.IsNullOrEmpty<int>());
        }

        [Fact]
        public void AddIfNotContains_Test()
        {
            ICollection<int> collections = new List<int>();

            Assert.True(collections.AddIfNotContains(1));
            Assert.False(collections.AddIfNotContains(1)); 
            Assert.True(collections.AddIfNotContains(2));

            collections = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                collections.AddIfNotContains(1);
            });

        }

    }
}
