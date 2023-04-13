using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections.HardwareInfo
{
    /// <summary>
    /// 获取计算机信息 --- cpu序列号；mac序列号；mac序列号；ip地址；登录用户名；计算机名；系统类型；内存大小(单位:M)
    /// </summary>
    public class ComputerInfo
    {
        public string[] CpuID;                //1.cpu序列号
        public string[] MacAddress;           //2.mac序列号
        public string DiskID;               //3.硬盘id
        public string IpAddress;            //4.ip地址
        public string LoginUserName;        //5.登录用户名
        public string ComputerName;         //6.计算机名
        public string SystemType;           //7.系统类型
        public string TotalPhysicalMemory; //8.内存量 单位：M

        string[] unknowStrings = new string[] { "unknow" };

        // 构造函数
        public ComputerInfo()
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskID = GetDiskID();

            IpAddress = GetIPAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();

            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
        }

        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        /// <returns></returns>
        string[] GetCpuID()
        {
            try
            {
                using (ManagementClass mc = new ManagementClass("Win32_Processor"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        var cpuIds = new string[moc.Count];
                        var i = 0;
                        foreach (ManagementObject mo in moc)
                        {
                            cpuIds[i] = mo.Properties["ProcessorId"].Value.ToString();
                            i++;
                        }
                        return cpuIds;
                    }
                }
            }
            catch
            {
                return unknowStrings;
            }
        }

        /// <summary>
        /// 获取物理网卡的MAC地址 【推荐，Win32_NetworkAdapter 中 PhysicalAdapter 属性 表示是否为物理网卡】
        /// </summary>
        /// GUID:连接唯一标识;
        /// MACAddress:网卡地址;
        /// NetEnabled: 是否启用了适配器，True为启用，False为禁用;
        /// PhysicalAdapter: 适配器是否物理或逻辑适配器,True为物理，False为逻辑;
        /// Index: 网络适配器的索引号,存储在系统注册表中。注册表路径Win32Registry|System\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}
        /// <returns></returns>
        string[] GetMacAddress(bool onlyEnabled = false)
        {
            try
            {
                var macs = new List<string>();
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapter"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            if ((bool)mo["PhysicalAdapter"] == true)
                            {
                                if (onlyEnabled)
                                {
                                    if ((bool)mo["NetEnabled"] == true)
                                    {
                                        macs.Add(mo["MacAddress"].ToString());
                                    }
                                }
                                else
                                {
                                    macs.Add(mo["MacAddress"].ToString());
                                }
                            }
                        }
                    }
                };

                return macs.ToArray();
            }
            catch
            {
                return unknowStrings;
            }

        }


        /// <summary>
        /// 获取网卡MAC地址 【不推荐，Win32_NetworkAdapterConfiguration 中没有 是否为物理网卡 的属性，有可能获取虚拟网卡】
        /// </summary>
        /// <returns></returns>
        string[] GetMacAddress_NotRecommend()
        {
            try
            {
                var macs = new List<string>();
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    using (ManagementObjectCollection moc = mc.GetInstances())
                    {
                        foreach (ManagementObject mo in moc)
                        {
                            if ((bool)mo["IPEnabled"] == true)
                            {
                                macs.Add(mo["MacAddress"].ToString());
                            }
                        }
                    }
                };

                return macs.ToArray();
            }
            catch
            {
                return unknowStrings;
            }
        }

        // 3.获取硬盘ID
        string GetDiskID()
        {
            try
            {
                /*
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
                disk.Get();
                return disk.GetPropertyValue("VolumeSerialNumber").ToString();
                */

                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");  // DeviceId  ;  Win32_PhysicalMedia  "SerialNumber"
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        // 4.获取IP地址
        string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        // 5.操作系统的登录用户名
        string GetUserName()
        {
            try
            {
                return Environment.UserName;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        // 6.获取计算机名
        string GetComputerName()
        {
            try
            {
                return System.Environment.MachineName;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        // 7.PC类型 
        string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }

        // 8.物理内存
        string GetTotalPhysicalMemory()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
        }
    }
}
