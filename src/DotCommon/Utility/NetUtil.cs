using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace DotCommon.Utility
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public static class NetUtil
    {

        /// <summary>
        /// 检查设置的端口号是否正确，并返回正确的端口号,无效端口号返回-1。
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static int GetValidPort(string port)
        {
            //声明返回的正确端口号
            //最小有效端口号
            const int minport = 0;
            //最大有效端口号
            const int maxport = 65535;

            //检测端口号
            //传入的端口号为空则抛出异常
            if (port == "")
            {
                throw new InvalidOperationException("port");
            }

            //检测端口范围
            if ((Convert.ToInt32(port) < minport) || (Convert.ToInt32(port) > maxport))
            {
                throw new ArgumentOutOfRangeException("port");
            }

            //为端口号赋值
            var validPort = Convert.ToInt32(port);
            return validPort;
        }

        /// <summary>
        /// 将字符串形式的IP地址转换成IPAddress对象
        /// </summary>
        /// <param name="ipString">string类型IP地址</param>
        /// <returns></returns>
        public static IPAddress ToIpAddress(string ipString)
        {
            return IPAddress.Parse(ipString);
        }

        /// <summary>
        /// 根据传入的IP地址详情,获取IP和端口号,IP地址的详情格式为 192.168.1.100:8080
        /// </summary>
        /// <param name="ipString">string类型IP地址</param>
        /// <returns></returns>
        public static IPEndPoint GetEndPoint(string ipString)
        {
            var value = ipString.Split(':');
            return new IPEndPoint(IPAddress.Parse(value[0]), int.Parse(value[1]));
        }

        /// <summary>
        /// 根据string类型ip集合获取IPEndPoint集合
        /// </summary>
        /// <param name="ips">string类型IP地址</param>
        /// <returns></returns>
        public static List<IPEndPoint> GetEndPoints(string ips)
        {
            var values = ips.Split(',');
            return values.Select(item => item.Split(':'))
                .Select(value => new IPEndPoint(IPAddress.Parse(value[0]), int.Parse(value[1])))
                .ToList();
        }

        /// <summary>
        /// 获取本机的计算机名
        /// </summary>
        public static string LocalHostName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// 获取本机的局域网IPV6
        /// </summary>        
        public static List<IPAddress> GetMatchineIpv6s()
        {
            return Dns.GetHostEntry(LocalHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetworkV6).ToList();
        }

        /// <summary>
        /// 获取本机的局域网IPV4
        /// </summary>
        public static List<IPAddress> GetMachineIpv4s()
        {
            return Dns.GetHostEntry(LocalHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();
        }

        /// <summary>
        /// 获取本机在Internet网络的广域网IP
        /// </summary>
        public static IPAddress GetWlan()
        {
            //计算机名
            var hostName = LocalHostName();
            var addressList = Dns.GetHostEntry(hostName).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();
            if (addressList.Count == 2)
            {
                return addressList[1];
            }
            return addressList.FirstOrDefault();
        }
    
    }
}
