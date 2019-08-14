using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class UnilayerXmlTest
    {
        [Fact]
        public void UnilayerXml_Test()
        {
            var unilayerXml = new UnilayerXml();
            unilayerXml.SetValue("Name", "ZhangSan");
            unilayerXml.SetValue("Age", 19);

            var xml = @"<xml><Age>19</Age><Name><![CDATA[ZhangSan]]></Name></xml>";

            Assert.True(unilayerXml.HasValue("Age"));
            Assert.False(unilayerXml.HasValue("Id"));
            Assert.Equal(19, unilayerXml.GetValue("Age"));
            Assert.Equal(2, unilayerXml.GetValues().Count);
            Assert.Equal(xml, unilayerXml.ToXml().Trim());

            var unilayerXml2 = new UnilayerXml(xml);
            Assert.True(unilayerXml2.HasValue("Age"));
            Assert.False(unilayerXml2.HasValue("Id"));
            Assert.Equal(19, Convert.ToInt32(unilayerXml2.GetValue("Age")));
            Assert.Equal(2, unilayerXml2.GetValues().Count);



        }

    }
}
