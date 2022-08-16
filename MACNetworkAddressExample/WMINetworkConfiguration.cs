using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace MACNetworkAddressExample
{
    class WMINetworkConfiguration
    {
        /// <summary>
        /// 通过WMI读取系统信息里的网卡MAC  获取所有网卡MAC时还是很多的，比“更改网络适配器”中显示的多；但是仅启用的又比看到的少
        /// </summary>
        /// <param name="onlyEnabledNICMAC">仅获取启用网卡的MAC地址</param>
        /// <param name="containINCName">是否包含网卡名称，默认不包含</param>
        /// <returns></returns>
        public static List<string> GetMacByWMI(bool onlyEnabledNICMAC = true, bool containINCName = false)
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

        /// <summary>
        /// 获取网卡名称
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetNICNames()
        {
            ArrayList nicNames = new ArrayList();

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["ipEnabled"])
                {
                    nicNames.Add(mo["Caption"]);
                }
            }
            return nicNames;
        }

        /// <summary>
        /// 获取指定网卡的IP信息
        /// </summary>
        /// <param name="nicName">Nombre de la tarjeta de red</param>
        /// <param name="ipAddresses">Direccion IP</param>
        /// <param name="subnets"></param>
        /// <param name="gateways"></param>
        /// <param name="dnses"></param>
        public static void GetIP(string nicName, out string[] ipAddresses, out string[] subnets, out string[] gateways, out string[] dnses)
        {
            ipAddresses = null;
            subnets = null;
            gateways = null;
            dnses = null;

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["ipEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {
                        ipAddresses = (string[])mo["IPAddress"];
                        subnets = (string[])mo["IPSubnet"];
                        gateways = (string[])mo["DefaultIPGateway"];
                        dnses = (string[])mo["DNSServerSearchOrder"];

                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 获取DNS
        /// </summary>
        /// <param name="nic">网卡名称，为空则为所有网卡</param>
        /// <returns></returns>
        public static string[] GetDns(string nic=null)
        {
            ManagementClass obj_mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection obj_moc = obj_mc.GetInstances();

            List<string> current_dnss = new List<string>();

            foreach (ManagementObject obj_mo in obj_moc)
            {
                if (!((bool)obj_mo["IPEnabled"]))
                    continue;

                if (!string.IsNullOrWhiteSpace(nic) &&!obj_mo["Caption"].ToString().Contains(nic))
                    continue;

                object result = obj_mo.Properties["DNSServerSearchOrder"].Value;
                if (result == null)
                    continue;

                current_dnss.AddRange(from object o in (Array)result
                                      select o.ToString());
            }
            return current_dnss.ToArray();
        }

        /// <summary>
        /// 为指定的网卡设置IP
        /// </summary>
        /// <param name="nicName">Nombre de la tarheta de red</param>
        /// <param name="IPAddresses">Direccion IP</param>
        /// <param name="subnetMask">Subnet</param>
        /// <param name="gateway">Gateway</param>
        /// <param name="DnsSearchOrder">DNS</param>
        public static void SetIP(string nicName, string IPAddresses, string subnetMask, string gateway, string DnsSearchOrder)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {
                        ManagementBaseObject newIP = mo.GetMethodParameters("EnableStatic");
                        ManagementBaseObject newGate = mo.GetMethodParameters("SetGateways");
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");

                        newGate["DefaultIPGateway"] = new string[] { gateway };
                        newGate["GatewayCostMetric"] = new int[] { 1 };

                        newIP["IPAddress"] = IPAddresses.Split(',');
                        newIP["SubnetMask"] = new string[] { subnetMask };

                        newDNS["DNSServerSearchOrder"] = DnsSearchOrder.Split(',');

                        ManagementBaseObject setIP = mo.InvokeMethod("EnableStatic", newIP, null);
                        ManagementBaseObject setGateways = mo.InvokeMethod("SetGateways", newGate, null);
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 为网卡启用DHCP
        /// </summary>
        /// <param name="nicName"></param>
        public static void SetDHCP(string nicName)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (mo["Caption"].Equals(nicName))
                {
                    ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                    newDNS["DNSServerSearchOrder"] = null;
                    ManagementBaseObject enableDHCP = mo.InvokeMethod("EnableDHCP", null, null);
                    ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                }
            }
        }
    }
}
