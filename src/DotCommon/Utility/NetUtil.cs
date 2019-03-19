using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DotCommon.Utility
{
    public class NetUtil
    {
        /***************检测判断*************/

        #region 检查设置的IP地址是否正确，返回正确的IP地址


        /// <summary>检查设置的端口号是否正确，并返回正确的端口号,无效端口号返回-1。
        /// </summary>
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

        #endregion

        /****************转换**************/

        #region 将字符串形式的IP地址转换成IPAddress对象

        /// <summary>将字符串形式的IP地址转换成IPAddress对象
        /// </summary>        
        public static IPAddress ToIpAddress(string ip)
        {
            return IPAddress.Parse(ip);
        }

        /// <summary>根据传入的IP地址详情,获取IP和端口号,IP地址的详情格式为 192.168.1.100:8080
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static IPEndPoint GetEndPoint(string ipValue)
        {
            var value = ipValue.Split(':');
            return new IPEndPoint(IPAddress.Parse(value[0]), int.Parse(value[1]));
        }

        /// <summary>获取IP地址详情集合
        /// </summary>
        public static List<IPEndPoint> GetEndPoints(string ipValues)
        {
            var values = ipValues.Split(',');
            return values.Select(item => item.Split(':'))
                .Select(value => new IPEndPoint(IPAddress.Parse(value[0]), int.Parse(value[1])))
                .ToList();
        }



        #endregion

        /**************获取本机的相关数据***************/

        #region 获取本机的计算机名

        /// <summary>获取本机的计算机名
        /// </summary>
        public static string LocalHostName()
        {
            return Dns.GetHostName();
        }

        #endregion

        #region 获取本机的局域网IPV6

        /// <summary>获取本机的局域网IPV6
        /// </summary>        
        public static Task<IEnumerable<IPAddress>> GetIpv6()
        {
            var task = new TaskCompletionSource<IEnumerable<IPAddress>>();
            //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
            Dns.GetHostEntryAsync(LocalHostName()).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    var addresses = t.Result.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetworkV6);
                    task.SetResult(addresses);
                }
            });
            return task.Task;
        }

        #endregion

        #region 获取本机的局域网IPV4

        /// <summary> 获取本机的局域网IPV4
        /// </summary>
        public static Task<IEnumerable<IPAddress>> GetIpv4()
        {
            var task = new TaskCompletionSource<IEnumerable<IPAddress>>();
            //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
            Dns.GetHostEntryAsync(LocalHostName()).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    var addresses = t.Result.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork);
                    task.SetResult(addresses);
                }
            });
            return task.Task;
        }

        #endregion

        #region 获取本机在Internet网络的广域网IP

        /// <summary>获取本机在Internet网络的广域网IP
        /// </summary>
        public static Task<IPAddress> GetWlan()
        {
            var task = new TaskCompletionSource<IPAddress>();
            //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
            Dns.GetHostEntryAsync(LocalHostName()).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    var addresses =
                        t.Result.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();
                    if (addresses.Count >= 2)
                    {
                        task.SetResult(addresses[1]);
                    }
                }
            });
            return task.Task;
        }

        #endregion

        #region 获取远程客户机的IP地址

        /// <summary> 获取远程客户机的IP地址
        /// </summary>   
        public static string GetClientIp(Socket clientSocket)
        {
            var client = (IPEndPoint)clientSocket.RemoteEndPoint;
            return client.Address.ToString();
        }

        #endregion

    }
}
