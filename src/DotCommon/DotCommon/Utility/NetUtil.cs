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

        /// <summary>
        /// Validates whether a string represents a valid IP network segment in CIDR notation (e.g., "192.168.1.0/24" or "2001:db8::/32").
        /// Supports both IPv4 and IPv6 network segments.
        /// </summary>
        /// <param name="cidrString">The string representation of the IP network segment in CIDR notation.</param>
        /// <returns><c>true</c> if the string represents a valid IP network segment; otherwise, <c>false</c>.</returns>
        public static bool IsValidIpNetworkSegment(string cidrString)
        {
            if (string.IsNullOrWhiteSpace(cidrString))
            {
                return false;
            }

            var parts = cidrString.Split('/');
            if (parts.Length != 2)
            {
                return false;
            }

            // Parse the IP address part
            if (!IPAddress.TryParse(parts[0], out IPAddress? ipAddress))
            {
                return false;
            }

            // Parse the prefix length part
            if (!int.TryParse(parts[1], out int prefixLength))
            {
                return false;
            }

            // Validate prefix length based on address family
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                // IPv4: prefix length should be between 0 and 32
                return prefixLength >= 0 && prefixLength <= 32;
            }
            else if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // IPv6: prefix length should be between 0 and 128
                return prefixLength >= 0 && prefixLength <= 128;
            }

            return false;
        }

        /// <summary>
        /// Validates whether a string represents a valid IPv4 network segment in CIDR notation (e.g., "192.168.1.0/24").
        /// </summary>
        /// <param name="cidrString">The string representation of the IPv4 network segment in CIDR notation.</param>
        /// <returns><c>true</c> if the string represents a valid IPv4 network segment; otherwise, <c>false</c>.</returns>
        public static bool IsValidIpv4NetworkSegment(string cidrString)
        {
            if (string.IsNullOrWhiteSpace(cidrString))
            {
                return false;
            }

            var parts = cidrString.Split('/');
            if (parts.Length != 2)
            {
                return false;
            }

            // Parse the IP address part and ensure it's IPv4
            if (!IPAddress.TryParse(parts[0], out IPAddress? ipAddress) || 
                ipAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                return false;
            }

            // Parse and validate the prefix length
            if (!int.TryParse(parts[1], out int prefixLength))
            {
                return false;
            }

            return prefixLength >= 0 && prefixLength <= 32;
        }

        /// <summary>
        /// Validates whether a string represents a valid IPv6 network segment in CIDR notation (e.g., "2001:db8::/32").
        /// </summary>
        /// <param name="cidrString">The string representation of the IPv6 network segment in CIDR notation.</param>
        /// <returns><c>true</c> if the string represents a valid IPv6 network segment; otherwise, <c>false</c>.</returns>
        public static bool IsValidIpv6NetworkSegment(string cidrString)
        {
            if (string.IsNullOrWhiteSpace(cidrString))
            {
                return false;
            }

            var parts = cidrString.Split('/');
            if (parts.Length != 2)
            {
                return false;
            }

            // Parse the IP address part and ensure it's IPv6
            if (!IPAddress.TryParse(parts[0], out IPAddress? ipAddress) || 
                ipAddress.AddressFamily != AddressFamily.InterNetworkV6)
            {
                return false;
            }

            // Parse and validate the prefix length
            if (!int.TryParse(parts[1], out int prefixLength))
            {
                return false;
            }

            return prefixLength >= 0 && prefixLength <= 128;
        }
    }
}