using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class NetUtilTest
    {
        [Fact]
        public void TryGetValidPort_ValidPort_Test()
        {
            Assert.True(NetUtil.TryGetValidPort("8080", out int port));
            Assert.Equal(8080, port);
        }

        [Fact]
        public void TryGetValidPort_InvalidPort_Test()
        {
            Assert.False(NetUtil.TryGetValidPort("99999", out int port));
            Assert.Equal(0, port);

            Assert.False(NetUtil.TryGetValidPort("-1", out port));
            Assert.Equal(0, port);

            Assert.False(NetUtil.TryGetValidPort("invalid", out port));
            Assert.Equal(0, port);

            Assert.False(NetUtil.TryGetValidPort("", out port));
            Assert.Equal(0, port);
        }

        [Fact]
        public void ToIpAddress_ValidIp_Test()
        {
            var ip = NetUtil.ToIpAddress("192.168.1.1");
            Assert.Equal(IPAddress.Parse("192.168.1.1"), ip);
        }

        [Fact]
        public void ToIpAddress_InvalidIp_Test()
        {
            var ip = NetUtil.ToIpAddress("invalid-ip");
            Assert.Null(ip);
        }

        [Fact]
        public void ToIpEndPoint_ValidEndPoint_Test()
        {
            var endPoint = NetUtil.ToIpEndPoint("127.0.0.1:80");
            Assert.NotNull(endPoint);
            Assert.Equal(IPAddress.Parse("127.0.0.1"), endPoint.Address);
            Assert.Equal(80, endPoint.Port);
        }

        [Fact]
        public void ToIpEndPoint_InvalidEndPoint_Test()
        {
            Assert.Null(NetUtil.ToIpEndPoint("invalid-ip:80"));
            Assert.Null(NetUtil.ToIpEndPoint("127.0.0.1:invalid-port"));
            Assert.Null(NetUtil.ToIpEndPoint("127.0.0.1"));
            Assert.Null(NetUtil.ToIpEndPoint(""));
        }

        [Fact]
        public void ToIpEndPoints_ValidEndPoints_Test()
        {
            var endPoints = NetUtil.ToIpEndPoints("192.168.1.1:80,192.168.1.2:8080");
            Assert.Equal(2, endPoints.Count);
            Assert.Equal(IPAddress.Parse("192.168.1.1"), endPoints[0].Address);
            Assert.Equal(80, endPoints[0].Port);
            Assert.Equal(IPAddress.Parse("192.168.1.2"), endPoints[1].Address);
            Assert.Equal(8080, endPoints[1].Port);
        }

        [Fact]
        public void ToIpEndPoints_MixedEndPoints_Test()
        {
            var endPoints = NetUtil.ToIpEndPoints("192.168.1.1:80,invalid,192.168.1.2:8080");
            Assert.Equal(2, endPoints.Count);
            Assert.Equal(IPAddress.Parse("192.168.1.1"), endPoints[0].Address);
            Assert.Equal(80, endPoints[0].Port);
            Assert.Equal(IPAddress.Parse("192.168.1.2"), endPoints[1].Address);
            Assert.Equal(8080, endPoints[1].Port);
        }

        [Fact]
        public void GetLocalHostName_Test()
        {
            Assert.NotNull(NetUtil.GetLocalHostName());
            Assert.False(string.IsNullOrWhiteSpace(NetUtil.GetLocalHostName()));
        }

        [Fact]
        public void GetLocalMachineIpv6Addresses_Test()
        {
            var ipv6Addresses = NetUtil.GetLocalMachineIpv6Addresses();
            Assert.NotNull(ipv6Addresses);
            // Assert.True(ipv6Addresses.Any()); // This might fail on systems without IPv6 configured
            Assert.All(ipv6Addresses, ip => Assert.Equal(AddressFamily.InterNetworkV6, ip.AddressFamily));
        }

        [Fact]
        public void GetLocalMachineIpv4Addresses_Test()
        {
            var ipv4Addresses = NetUtil.GetLocalMachineIpv4Addresses();
            Assert.NotNull(ipv4Addresses);
            Assert.True(ipv4Addresses.Any()); // Most systems should have at least one IPv4 address
            Assert.All(ipv4Addresses, ip => Assert.Equal(AddressFamily.InterNetwork, ip.AddressFamily));
        }

        [Fact]
        public void GetLocalNonLoopbackIpv4Address_Test()
        {
            var nonLoopbackIp = NetUtil.GetLocalNonLoopbackIpv4Address();
            Assert.NotNull(nonLoopbackIp);
            Assert.Equal(AddressFamily.InterNetwork, nonLoopbackIp.AddressFamily);
            Assert.False(IPAddress.IsLoopback(nonLoopbackIp));
        }
    }
}