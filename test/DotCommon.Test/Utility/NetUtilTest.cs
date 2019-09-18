using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class NetUtilTest
    {
        [Fact]
        public void GetValidPort_Test()
        {
            var p1 = NetUtil.GetValidPort("122");
            Assert.Equal(122, p1);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                NetUtil.GetValidPort("99999");
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                NetUtil.GetValidPort("-1");
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                NetUtil.GetValidPort("");
            });

        }

        [Fact]
        public void GetEndPoint_Test()
        {
            var endPoint1 = NetUtil.GetEndPoint("127.0.1.1:9999");
            Assert.Equal(9999, endPoint1.Port);

            var ip1 = NetUtil.ToIpAddress("192.168.0.1");
            Assert.Equal(IPAddress.Parse("192.168.0.1"), ip1);

            var endPoints = NetUtil.GetEndPoints("192.168.0.111:8080,192.168.0.111:8081,192.168.0.111:8082");
            Assert.Equal(3, endPoints.Count);
            Assert.Equal(3, endPoints.Count(x => x.Address.Equals(IPAddress.Parse("192.168.0.111"))));
            Assert.Contains(endPoints, x => x.Port == 8080);
        }

        [Fact]
        public void LocalHostName_Test()
        {
            Assert.NotNull(NetUtil.LocalHostName());
        }

        [Fact]
        public void GetMatchineIps_Test()
        {
            Assert.True(NetUtil.GetMatchineIpv6s().Count() > 0);
            Assert.True(NetUtil.GetMachineIpv4s().Count() > 0);

        }


        [Fact]
        public void GetWlan_Test()
        {
            var iPAddress = NetUtil.GetWlan();
            Assert.True(iPAddress != null);
        }
    }
}
