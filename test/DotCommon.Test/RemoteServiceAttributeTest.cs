using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace DotCommon.Test
{
    public class RemoteServiceAttributeTest
    {
        [Fact]
        public void RemoteServiceAttribute_Tes()
        {
            var attribute = new RemoteServiceAttribute(true);

            Assert.True(attribute.IsEnabledFor(typeof(int)));

            var method = this.GetType().GetMethod("TestMethod");

            Assert.True(attribute.IsEnabledFor(method));

            Assert.True(attribute.IsMetadataEnabledFor(typeof(int)));
            Assert.True(attribute.IsMetadataEnabledFor(method));

        }

        private void TestMethod()
        {

        }
    }
}
