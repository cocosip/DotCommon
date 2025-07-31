using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DotCommon.Utility
{
    /// <summary>
    /// Provides utility methods for network-related operations.
    /// </summary>
    public static class NetUtil
    {
        /// <summary>
        /// Attempts to parse a string as a valid port number (0-65535).
        /// </summary>
        /// <param name="portString">The string representation of the port number.</param>
        /// <param name="validPort">When this method returns, contains the parsed port number if the parsing succeeded, or 0 if the parsing failed.</param>
        /// <returns><c>true</c> if the portString was successfully parsed and is within the valid range; otherwise, <c>false</c>.</returns>
        public static bool TryGetValidPort(string portString, out int validPort)
        {
            validPort = 0;
            if (string.IsNullOrWhiteSpace(portString))
            {
                return false;
            }

            if (int.TryParse(portString, out int parsedPort))
            {
                if (parsedPort >= IPEndPoint.MinPort && parsedPort <= IPEndPoint.MaxPort)
                {
                    validPort = parsedPort;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Converts a string representation of an IP address to an <see cref="IPAddress"/> object.
        /// </summary>
        /// <param name="ipString">The string representation of the IP address.</param>
        /// <returns>An <see cref="IPAddress"/> object if the parsing succeeds; otherwise, <c>null</c>.</returns>
        public static IPAddress? ToIpAddress(string ipString)
        {
            if (IPAddress.TryParse(ipString, out IPAddress? ipAddress))
            {
                return ipAddress;
            }
            return null;
        }

        /// <summary>
        /// Converts a string representation of an IP endpoint (e.g., "192.168.1.100:8080") to an <see cref="IPEndPoint"/> object.
        /// </summary>
        /// <param name="ipEndPointString">The string representation of the IP endpoint.</param>
        /// <returns>An <see cref="IPEndPoint"/> object if the parsing succeeds; otherwise, <c>null</c>.</returns>
        public static IPEndPoint? ToIpEndPoint(string ipEndPointString)
        {
            var parts = ipEndPointString.Split(':');
            if (parts.Length == 2)
            {
                IPAddress? ipAddress = ToIpAddress(parts[0]);
                if (ipAddress != null && TryGetValidPort(parts[1], out int port))
                {
                    return new IPEndPoint(ipAddress, port);
                }
            }
            return null;
        }

        /// <summary>
        /// Converts a comma-separated string of IP endpoints (e.g., "192.168.1.1:80,192.168.1.2:8080") to a list of <see cref="IPEndPoint"/> objects.
        /// Invalid endpoints in the string will be skipped.
        /// </summary>
        /// <param name="ipEndPointsString">The comma-separated string of IP endpoints.</param>
        /// <returns>A <see cref="List{IPEndPoint}"/> containing the parsed IP endpoints.</returns>
        public static List<IPEndPoint> ToIpEndPoints(string ipEndPointsString)
        {
            var endPoints = new List<IPEndPoint>();
            var parts = ipEndPointsString.Split(',');
            foreach (var part in parts)
            {
                IPEndPoint? endPoint = ToIpEndPoint(part.Trim());
                if (endPoint != null)
                {
                    endPoints.Add(endPoint);
                }
            }
            return endPoints;
        }

        /// <summary>
        /// Gets the local machine's host name.
        /// </summary>
        /// <returns>The local machine's host name.</returns>
        public static string GetLocalHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// Gets a list of IPv6 addresses for the local machine.
        /// </summary>
        /// <returns>A <see cref="List{IPAddress}"/> containing the IPv6 addresses.</returns>
        public static List<IPAddress> GetLocalMachineIpv6Addresses()
        {
            return Dns.GetHostEntry(GetLocalHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetworkV6).ToList();
        }

        /// <summary>
        /// Gets a list of IPv4 addresses for the local machine.
        /// </summary>
        /// <returns>A <see cref="List{IPAddress}"/> containing the IPv4 addresses.</returns>
        public static List<IPAddress> GetLocalMachineIpv4Addresses()
        {
            return Dns.GetHostEntry(GetLocalHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();
        }

        /// <summary>
        /// Attempts to get a non-loopback IPv4 address for the local machine.
        /// This method does not guarantee a public IP address, as it only checks local network interfaces.
        /// </summary>
        /// <returns>A non-loopback <see cref="IPAddress"/> if found; otherwise, <c>null</c>.</returns>
        public static IPAddress? GetLocalNonLoopbackIpv4Address()
        {
            return Dns.GetHostEntry(GetLocalHostName()).AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));
        }
    }
}