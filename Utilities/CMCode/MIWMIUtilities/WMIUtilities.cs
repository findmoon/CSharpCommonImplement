using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CMCode.MIWMIUtilities
{
    /// <summary>
    /// WMI工具类
    /// </summary>
    public static class WMIUtilities
    {
        #region 使用ManagementObjectSearcher查询
        #region CPU序列号
        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        /// <returns></returns>
        public static string[] GetCPUSerialNumber()
        {
            var cpuSerialNumbers = new List<string>();

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor"))
            {
                var moCollects = searcher.Get();
                foreach (ManagementObject mo in moCollects)
                {
                    cpuSerialNumbers.Add(mo["ProcessorId"].ToString().Trim());
                }
                return cpuSerialNumbers.ToArray();
            }
        }
        #endregion

        #region 主板序列号
        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        public static string GetBIOSSerialNumber()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS"))
            {
                string sBIOSSerialNumber = "";
                #region 遍历属性
                //foreach (ManagementObject mo in searcher.Get())
                //{
                //    foreach (var property in mo.Properties)
                //    {
                //        Console.WriteLine(property.Name + ":" + property.Value);
                //    }
                //} 
                #endregion

                foreach (ManagementObject mo in searcher.Get())
                {
                    sBIOSSerialNumber = mo.GetPropertyValue("SerialNumber").ToString().Trim();
                    break;
                }
                return sBIOSSerialNumber;
            }
        }
        //获取主板信息 参考测试
        public static void GetBIOSInfo()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"\\.\root\cimv2", "SELECT * FROM Win32_BIOS"))
            {
                foreach (var _object in searcher.Get())
                {
                    if (_object != null)
                    {
                        string object_name = "?";

                        if (_object["Name"] != null)
                        {
                            object_name = _object["Name"].ToString();
                        }
                        else if (_object["Caption"] != null)
                        {
                            object_name = _object["Caption"].ToString();
                        }
                        else if (_object["Description"] != null)
                        {
                            object_name = _object["Description"].ToString();
                        }


                        var property_value_B = new StringBuilder();
                        foreach (var property in _object.Properties)
                        {
                            string property_name = property.Name;

                            if ((property.Value != null) &&
                                (!property_name.Contains("CreationClassName")))
                            {

                                if (!(property.Value is Array))
                                {
                                    property_value_B.AppendLine(property.Value.ToString());
                                }
                                else
                                {
                                    StringBuilder _property_value = new StringBuilder();

                                    Array _property_array = property.Value as Array;

                                    int count = 0;

                                    foreach (var entry in _property_array)
                                    {
                                        _property_value.AppendLine(entry.ToString());
                                        count++;
                                    }

                                    property_value_B.AppendLine(_property_value.ToString().Trim());
                                }
                            }
                        }

                        Console.WriteLine(object_name);
                        Console.WriteLine(property_value_B.ToString().Trim());

                    }
                }
            }
        }
        #endregion

        #region 硬盘序列号
        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string[] GetHardDiskSerialNumber()
        {
            var diskSerialNumbers = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia"))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    diskSerialNumbers.Add(mo["SerialNumber"].ToString().Trim());
                }
                return diskSerialNumbers.ToArray();
            }
        }
        #endregion

        #region 获取网卡地址
        /// <summary>
        /// 获取网卡地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetNetCardMACAddress()
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))"))
            {
                var macSerialNumbers = new List<string>();


                foreach (ManagementObject mo in searcher.Get())
                {
                    macSerialNumbers.Add(mo["MACAddress"].ToString().Trim());
                }
                return macSerialNumbers.ToArray();
            }
        }
        #endregion

        #region 获得串口信息
        /// <summary>
        /// 获得串口信息。获取的不准确，比如会获取到蓝牙设备
        /// </summary>
        /// <returns></returns>
        public static string[] GetComInfo()
        {
            List<string> Comstrs = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity"))
            {
                var hardInfos = searcher.Get();
                foreach (var comInfo in hardInfos)
                {
                    var nameValue = comInfo.Properties["Name"].Value;
                    if (nameValue != null && nameValue.ToString().Contains("COM")) // 是否需要.ToUpper()？
                    {
                        try
                        {
                            //通讯端口(COM1) 转变为 COM1:通讯端口
                            string com = nameValue.ToString();
                            string[] strcom = com.Split(new char[2] { '(', ')' });

                            Comstrs.Add(strcom[1] + ":" + strcom[0]);
                        }
                        catch { }

                    }
                }
                searcher.Dispose();
            }
            return Comstrs.ToArray();
        }
        #endregion
        #endregion

        #region 使用ManagementClass
        /// <summary>
        /// Returns a string containing information on running processes
        /// </summary>
        /// <param name="tb"></param>
        public static string ListAllProcesses()
        {
            // list out all processes and write them into a stringbuilder
            using (ManagementClass MgmtClass = new ManagementClass("Win32_Process"))
            {
                StringBuilder sb = new StringBuilder();
                foreach (ManagementObject mo in MgmtClass.GetInstances())
                {
                    sb.Append("Name:\t" + mo["Name"] + Environment.NewLine);
                    sb.Append("ID:\t" + mo["ProcessId"] + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// Determine if a process is running by name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool CheckForProcessByName(string processName)
        {
            using (ManagementClass MgmtClass = new ManagementClass("Win32_Process"))
            {
                foreach (ManagementObject mo in MgmtClass.GetInstances())
                {
                    if (mo["Name"].ToString().ToLower() == processName.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// Check for the existence of a process by ID; if the ID
        /// is found, the method will return a true
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static bool CheckForProcessById(string processId)
        {
            using (ManagementClass MgmtClass = new ManagementClass("Win32_Process"))
            {
                foreach (ManagementObject mo in MgmtClass.GetInstances())
                {
                    if (mo["ProcessId"].ToString() == processId)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion
    }
}
