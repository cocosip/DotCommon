using System;
using DotCommon.ObjectMapping;
using Xunit;

namespace DotCommon.Test.ObjectMapping
{
    public class NotImplementedAutoObjectMappingProviderTest
    {
        [Fact]
        public void Map_WithSource_ShouldThrowNotImplemented()
        {
            var provider = new NotImplementedAutoObjectMappingProvider();
            Assert.Throws<NotImplementedException>(() => provider.Map<Source, Destination>(new Source()));
        }

        [Fact]
        public void Map_WithSourceAndDestination_ShouldThrowNotImplemented()
        {
            var provider = new NotImplementedAutoObjectMappingProvider();
            Assert.Throws<NotImplementedException>(() => provider.Map(new Source(), new Destination()));
        }

        private class Source { }
        private class Destination { }
    }
}