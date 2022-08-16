using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace System.Net.NetworkInformation
{
    /// <summary>
    /// 查找IP，参考自 http://www.nullskull.com/q/10303342/how-to-get-all-ip-address-of-a-network.aspx
    /// </summary>
    public static class IPLookup
    {
        /// <summary>
        /// 获取网络内的所有ip
        /// 测试下面的方法并不能正确的获取一个网络内的所有ip。至少Android手机使用的ip未获取到
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static IPAddress[] GetAllUnicastAddresses_New(bool excldeIPV6 = true)
        {
            // This works on both Mono and .NET , but there is a difference: it also
            // includes the LocalLoopBack so we need to filter that one out
            List<IPAddress> Addresses = new List<IPAddress>();
            // Obtain a reference to all network interfaces in the machine
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                foreach (IPAddressInformation uniCast in properties.UnicastAddresses)
                {
                    // Ignore loop-back addresses & IPv6
                    if (!IPAddress.IsLoopback(uniCast.Address) &&(!excldeIPV6 || excldeIPV6&&uniCast.Address.AddressFamily != AddressFamily.InterNetworkV6))
                        Addresses.Add(uniCast.Address);
                }

            }
            return Addresses.ToArray();
        }
        /// <summary>
        /// 获取网络内所有Ip 和 GetAllUnicastAddresses_New 结果一致。并不能获取网络内所有的ip地址
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetAllUnicastAddresses2(bool excldeIPV6 = true)
        {
            var globalNet =IPGlobalProperties.GetIPGlobalProperties();
            var addresses = globalNet.GetUnicastAddresses();
            return addresses.Where(ip=> !IPAddress.IsLoopback(ip.Address)&&((excldeIPV6 && ip.Address.AddressFamily != AddressFamily.InterNetworkV6) || !excldeIPV6)).Select(c => c.Address).ToArray();
        }

        /// <summary>
        /// 获取本地机器的ip地址 与命令中 ipconfig 获取结果一致
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetLocalIps(bool excldeIPV6=true)
        {
            string sHostName = Dns.GetHostName();
            // Dns.GetHostEntry("") 传递空字符串获取的也是当前机器本地的ip
            IPHostEntry ipE = Dns.GetHostEntry(sHostName);
            return excldeIPV6? ipE.AddressList.Where(ip=>ip.AddressFamily!= AddressFamily.InterNetworkV6).ToArray() : ipE.AddressList;
        }
    }
}