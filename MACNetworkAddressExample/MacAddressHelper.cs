using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
    /// <summary>
    /// Mac地址获取帮助类
    /// </summary>
    public static class MacAddressHelper
    {
        ///<summary>
        /// 根据截取ipconfig /all命令的输出流获取网卡Mac，支持不同语言编码  获取的MAC与NetworkInterface获取的一致
        /// cmd中可以识别ipconfig/all，powershell中必须为ipconfig /all
        ///</summary>
        ///<returns></returns>
        public static string[] GetMacByIpConfig()
        {
            List<string> macs = new List<string>();

            var runCmd = Cmd.RunCmd("chcp 437&&ipconfig /all");

            foreach (var line in runCmd.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("Physical Address"))
                    {
                        macs.Add(line.Substring(36));
                    }
                    // 参考文章中此处的处理很不理解
                    //else if (line.StartsWith("DNS Servers") && line.Length > 36 && line.Substring(36).Contains("::"))
                    //{
                    //    macs.Clear();
                    //}
                    //else if (macs.Count > 0 && line.StartsWith("NetBIOS") && line.Contains("Enabled"))
                    //{
                    //    return macs.Last();
                    //}
                }
            }

            return macs.ToArray();
        }
        /// <summary>
        /// 不借助cmd直接执行ipconfig程序获取本机Mac地址
        /// </summary>
        /// <param name="ipAddress">ip，为空则获取所有mac地址</param>
        /// <returns></returns>
        public static string[] GetMacByIpConfig2(string ipAddress="")
        {
            var ips = new List<string>();
            string output;

            try
            {
                // 开始一个子进程
                Process p = new Process();
                
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "ipconfig";
                p.StartInfo.Arguments = "/all";
                p.Start();
                // 在等待子进程之前获取其重定向的输出流
                // p.WaitForExit();
                // Read the output stream first and then wait.
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }
            catch
            {
                return null;
            }

            // pattern to get all connections
            var pattern = @"(?xis) 
(?<Header>
     (\r|\n) [^\r]+ :  \r\n\r\n
)
(?<content>
    .+? (?= ( (\r\n\r\n)|($)) )
)";

            List<Match> matches = new List<Match>();

            foreach (Match m in Regex.Matches(output, pattern))
                matches.Add(m);
            if (string.IsNullOrWhiteSpace(ipAddress)) ipAddress = "";
            // 中文下 将 "Physical \s Address"替换为"物理地址"
            var connection = matches.Select(m => new
            {
                containsIp = m.Value.Contains(ipAddress),
                containsPhysicalAddress = Regex.Match(m.Value, @"(?ix)物理地址").Success, 
                content = m.Value
            }).Where(x => x.containsIp && x.containsPhysicalAddress)
            .Select(m => Regex.Match(m.content, @"(?ix)  物理地址[^:]+ : \s* (?<Mac>[^\s]+)").Groups["Mac"].Value).ToArray();

            return connection;
        }
        /// <summary>
        /// 通过NetworkInterface获取本机指定ip的mac地址
        /// </summary>
        /// <param name="ipAddress">ip，为空则获取所有mac地址</param>
        /// <returns></returns>
        public static string[] GetMacByNetworkInterface2(string ipAddress="")
        {
            // grab all online interfaces
            var query = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n =>
                    n.OperationalStatus == OperationalStatus.Up && // only grabbing what's online
                    n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(_ => new
                {
                    PhysicalAddress = _.GetPhysicalAddress(),
                    IPProperties = _.GetIPProperties(),
                });
            if (string.IsNullOrWhiteSpace(ipAddress)) ipAddress = "";
            // grab the first interface that has a unicast address that matches your search string
            var mac = query
                .Where(q => q.IPProperties.UnicastAddresses
                    .Any(ua => ua.Address.ToString().Contains(ipAddress)))
                .Select(_=>_.PhysicalAddress)
                .ToArray();

            // return the mac address with formatting (eg "00-00-00-00-00-00")
            return mac.Select(_=> string.Join("-", _.GetAddressBytes().Select(b => b.ToString("X2")))).ToArray();
        }

        /// <summary>
        /// 通过WMI读取系统信息里的网卡MAC  获取所有网卡MAC时还是很多的，比“更改网络适配器”中显示的多；但是仅启用的又比看到的少
        /// </summary>
        /// <param name="onlyEnabledNICMAC">仅获取启用网卡的MAC地址</param>
        /// <param name="containINCName">是否包含网卡名称，默认不包含</param>
        /// <returns></returns>
        public static List<string> GetMacByWMI(bool onlyEnabledNICMAC = true,bool containINCName=false)
        {
            try
            {
                List<string> macs = new List<string>();
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if (onlyEnabledNICMAC)
                    {
                        if (!(bool)mo["IPEnabled"]) continue;
                    }
                    var mac = mo["MacAddress"]?.ToString();
                    if (mac == null) continue;
                    if (containINCName)
                    {
                        macs.Add($"{mo["Caption"].ToString()}：{mac}");
                    }
                    else
                    {
                        macs.Add(mac);
                    }
                }
                return macs;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac  获取数量比“更改网络适配器”中显示的多，但比WMI所有的少
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                // NetworkInterfaceType enum 可以过滤 network interface 的类型，比如不是以太网 nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet
                // OperationalStatus 判断是否可用，如 ni.OperationalStatus == OperationalStatus.Up
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }

        ///<summary>
        /// 通过SendARP获取对应ip(远程)网卡的Mac地址
        /// 网络被禁用或未接入网络（如没插网线）时此方法失灵
        ///</summary>
        ///<param name="remoteIP"></param>
        ///<returns></returns>
        public static string GetMacBySendArp(string remoteIP)
        {
            StringBuilder macAddress = new StringBuilder();
            try
            {
                Int32 remote = inet_addr(remoteIP);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);
                string temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();
                int x = 12;
                for (int i = 0; i < 6; i++)
                {
                    if (i == 5)
                    {
                        macAddress.Append(temp.Substring(x - 2, 2));
                    }
                    else
                    {
                        macAddress.Append(temp.Substring(x - 2, 2) + "-");
                    }
                    x -= 2;
                }
                return macAddress.ToString();
            }
            catch
            {
                return macAddress.ToString();
            }
        }

        /// <summary>
        /// SendARP发送ARP请求，获取IPv4对应的物理地址。 Iphlpapi.dll（IP Helper API）是 IP帮助API，位于'C:\WINDOWS\system32\'
        /// </summary>
        /// <param name="destIp">目标IP</param>
        /// <param name="srcIP">源IP</param>
        /// <param name="mac">ULONG变量数组的指针,数组至少要两个元素</param>
        /// <param name="phyAddrLen">指向ULONG值的指针,表示以字节为单位的buffer大小，即设置接受mac的大小，最少应该为6个字节（mac为6字节）</param>
        /// <returns></returns>
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 destIp, Int32 srcIP, ref Int64 mac, ref Int32 phyAddrLen);
        //[DllImport("iphlpapi.dll", ExactSpelling = true)]
        //public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);

        /// <summary>
        /// 将ip字符串转换为 IN_ADDR 结构的地址  Ws2_32.dll为Windows Sockets应用程序接口
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);

      
    }
}
