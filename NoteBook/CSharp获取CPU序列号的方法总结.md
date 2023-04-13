**C#获取CPU序列号的方法总结**

[toc]

> HelperCollections 项目下，新建 `HardwareInfo\CPUInfo.cs`。

# x86 汇编 CPUID指令 获取

Win32 平台上，获取CPUID的办法主要有两种，一种是利用 WMI ；另一种是利用 x86 汇编的 cpuid 指令。

而最快的办法就是通过汇编了，而且 WMI 与汇编之间效率上的差距的确有点让人难以忍受，WMI 获取 CPUID 的效率几乎接近了一秒钟，而利用 cpuid 指令的办法，大概是几个 us 时间

> 可以测量微妙精度

```C#
public class CPUInfo
{

    [DllImport("kernel32.dll", SetLastError = false)]
    private static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int __cpuid(ref int s1, ref int s2);

    /// <summary>
    /// 利用 x86 汇编的 cpuid 指令 获取CPU序列号
    /// </summary>
    /// __asm 关键字用于调用内联汇编程序...
    /// <returns></returns>
    [Obsolete("必须在x86上运行，否则 cpuid执行报错 System.Runtime.InteropServices.SEHException:“外部组件发生异常。”")]
    public static string SerialNumber_Use_asm()
    {
        /*
                pushad
                mov eax, 01h
                xor ecx, ecx
                xor edx, edx
                cpuid
                mov ecx, dword ptr[ebp + 8]
                mov dword ptr[ecx], edx
                mov ecx, dword ptr[ebp + 0Ch]
                mov dword ptr[ecx], eax
                popad
        */
        byte[] shellcode = { 96, 184, 1, 0, 0, 0, 51, 201, 51, 210, 15, 162, 139, 77, 8, 137, 17, 139, 77, 12, 137, 1, 97, 195 };
        IntPtr address = GCHandle.Alloc(shellcode, GCHandleType.Pinned).AddrOfPinnedObject();
        VirtualProtect(address, (uint)shellcode.Length, 0x40, out uint lpflOldProtect);
        __cpuid cpuid = (__cpuid)Marshal.GetDelegateForFunctionPointer(address, typeof(__cpuid));


        int s1 = 0;
        int s2 = 0;
        for (int i = 0; i < 100000; i++)
        {
            cpuid(ref s1, ref s2);
        }
        // Console.Write("asm: {0}", );

        return $"{s1.ToString("X2")}{s2.ToString("X2")}";
    }
}
```

# WMI 获取

添加引用 System.Management.dll 

```C#
public class CPUInfo
{
    /// <summary>
    /// 使用 WMI 获取CPU序列号
    /// </summary>
    /// Windows Management Instrumentation
    /// <returns></returns>
    public static string[] SerialNumber_Use_WMI()
    {
        using (ManagementClass mc = new ManagementClass("Win32_Processor"))
        {
            using (ManagementObjectCollection moc = mc.GetInstances())
            {
                var cpus = new string[moc.Count];
                var i = 0;
                foreach (ManagementObject mo in moc)
                {
                    //Console.WriteLine(", wmi: {0}", mo.Properties["ProcessorId"].Value.ToString());
                    cpus[i] = mo.Properties["ProcessorId"].Value.ToString();
                    i++;
                }
                return cpus;
            }
        }
    }
}
```

# ComputerInfo 类

ComputerInfo.cs 未整理完成。只整理了`GetCpuID()`、`GetMacAddress()`、`GetMacAddress_NotRecommend()`。



# ManagementObjectSearcher 查询方式

```C#
//获取CPU序列号
public string GetCPUSerialNumber()
{
    try
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
        string sCPUSerialNumber = "";
        foreach (ManagementObject mo in searcher.Get())
        {
            sCPUSerialNumber = mo["ProcessorId"].ToString().Trim();
            break;
        }
        return sCPUSerialNumber;
    }
    catch
    {
        return "";
    }
}
 
 
//获取主板序列号
public string GetBIOSSerialNumber()
{
    try
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
        string sBIOSSerialNumber = "";
        foreach (ManagementObject mo in searcher.Get())
        {
            sBIOSSerialNumber = mo.GetPropertyValue("SerialNumber").ToString().Trim();
            break;
        }
        return sBIOSSerialNumber;
    }
    catch
    {
        return "";
    }
}
 
 
//获取硬盘序列号
public string GetHardDiskSerialNumber()
{
    try
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
        string sHardDiskSerialNumber = "";
        foreach (ManagementObject mo in searcher.Get())
        {
            sHardDiskSerialNumber = mo["SerialNumber"].ToString().Trim();
            break;
        }
        return sHardDiskSerialNumber;
    }
    catch
    {
        return "";
    }
}
 
 
//获取网卡地址
public string GetNetCardMACAddress()
{
    try
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))");
        string NetCardMACAddress = "";
        foreach (ManagementObject mo in searcher.Get())
        {
            NetCardMACAddress = mo["MACAddress"].ToString().Trim();
            break;
        }
        return NetCardMACAddress;
    }
    catch
    {
        return "";
    }
}
```

```sh
// 硬件 
Win32_Processor, // CPU 处理器 
Win32_PhysicalMemory, // 物理内存条 
Win32_Keyboard, // 键盘 
Win32_PointingDevice, // 点输入设备，包括鼠标。 
Win32_FloppyDrive, // 软盘驱动器 
Win32_DiskDrive, // 硬盘驱动器 
Win32_CDROMDrive, // 光盘驱动器 
Win32_BaseBoard, // 主板 
Win32_BIOS, // BIOS 芯片 
Win32_ParallelPort, // 并口 
Win32_SerialPort, // 串口 
Win32_SerialPortConfiguration, // 串口配置 
Win32_SoundDevice, // 多媒体设置，一般指声卡。 
Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP) 
Win32_USBController, // USB 控制器 
Win32_NetworkAdapter, // 网络适配器 
Win32_NetworkAdapterConfiguration, // 网络适配器设置 
Win32_Printer, // 打印机 
Win32_PrinterConfiguration, // 打印机设置 
Win32_PrintJob, // 打印机任务 
Win32_TCPIPPrinterPort, // 打印机端口 
Win32_POTSModem, // MODEM 
Win32_POTSModemToSerialPort, // MODEM 端口 
Win32_DesktopMonitor, // 显示器 
Win32_DisplayConfiguration, // 显卡 
Win32_DisplayControllerConfiguration, // 显卡设置 
Win32_VideoController, // 显卡细节。 
Win32_VideoSettings, // 显卡支持的显示模式。 
 
// 操作系统 
Win32_TimeZone, // 时区 
Win32_SystemDriver, // 驱动程序 
Win32_DiskPartition, // 磁盘分区 
Win32_LogicalDisk, // 逻辑磁盘 
Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。 
Win32_LogicalMemoryConfiguration, // 逻辑内存配置 
Win32_PageFile, // 系统页文件信息 
Win32_PageFileSetting, // 页文件设置 
Win32_BootConfiguration, // 系统启动配置 
Win32_ComputerSystem, // 计算机信息简要 
Win32_OperatingSystem, // 操作系统信息 
Win32_StartupCommand, // 系统自动启动程序 
Win32_Service, // 系统安装的服务 
Win32_Group, // 系统管理组 
Win32_GroupUser, // 系统组帐号 
Win32_UserAccount, // 用户帐号 
Win32_Process, // 系统进程 
Win32_Thread, // 系统线程 
Win32_Share, // 共享 
Win32_NetworkClient, // 已安装的网络客户端 
Win32_NetworkProtocol, // 已安装的网络协议
```

```C#
/// <summary>
        /// 枚举win32 api
        /// </summary>
        private enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }

        /// <summary>
        /// WMI取硬件信息
        /// </summary>
        /// <param name="hardType"></param>
        /// <param name="propKey"></param>
        /// <returns></returns>
        private static string[] MulGetHardwareInfo(HardwareEnum hardType, string propKey)
        {

            List<string> deviceList = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        deviceList.Add(hardInfo.Properties[propKey].Value.ToString());
                    }

                    searcher.Dispose();
                }
                return deviceList.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            { deviceList = null; }
        }



        public  void GetAllCOMs()
        {
            try
            {
                string[] Port_Names =  MulGetHardwareInfo(HardwareEnum.Win32_SerialPort, "Name");
            }
            catch
            {
                ;
            }
        }
```

using System.Management;  
/// 获取本机用户名、MAC地址、内网IP地址、公网IP地址、硬盘ID、CPU序列号、系统名称、物理内存。  
/// </summary>  
public class GetSystemInfo  
{  
    /// <summary>  
    /// 操作系统的登录用户名  
    /// </summary>  
    /// <returns>系统的登录用户名</returns>  
    public static string GetUserName()  
    {  
        try  
        {  
            string strUserName = string.Empty;  
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");  
            ManagementObjectCollection moc = mc.GetInstances();  
            foreach (ManagementObject mo in moc)  
            {  
                strUserName = mo["UserName"].ToString();  
            }  
            moc = null;  
            mc = null;  
            return strUserName;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取本机MAC地址  
    /// </summary>  
    /// <returns>本机MAC地址</returns>  
    public static string GetMacAddress()  
    {  
        try  
        {  
            string strMac = string.Empty;  
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");  
            ManagementObjectCollection moc = mc.GetInstances();  
            foreach (ManagementObject mo in moc)  
            {  
                if ((bool)mo["IPEnabled"] == true)  
                {  
                    strMac = mo["MacAddress"].ToString();  
                }  
            }  
            moc = null;  
            mc = null;  
            return strMac;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取本机的物理地址  
    /// </summary>  
    /// <returns></returns>  
    public static string getMacAddr_Local()  
    {  
        string madAddr = null;  
        try  
        {  
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");  
            ManagementObjectCollection moc2 = mc.GetInstances();  
            foreach (ManagementObject mo in moc2)  
            {  
                if (Convert.ToBoolean(mo["IPEnabled"]) == true)  
                {  
                    madAddr = mo["MacAddress"].ToString();  
                    madAddr = madAddr.Replace(':', '-');  
                }  
                mo.Dispose();  
            }  
            if (madAddr == null)  
            {  
                return "unknown";  
            }  
            else  
            {  
                return madAddr;  
            }  
        }  
        catch (Exception)  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取客户端内网IPv6地址  
    /// </summary>  
    /// <returns>客户端内网IPv6地址</returns>  
    public static string GetClientLocalIPv6Address()  
    {  
        string strLocalIP = string.Empty;  
        try  
        {  
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());  
            IPAddress ipAddress = ipHost.AddressList[0];  
            strLocalIP = ipAddress.ToString();  
            return strLocalIP;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取客户端内网IPv4地址  
    /// </summary>  
    /// <returns>客户端内网IPv4地址</returns>  
    public static string GetClientLocalIPv4Address()  
    {  
        string strLocalIP = string.Empty;  
        try  
        {  
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());  
            IPAddress ipAddress = ipHost.AddressList[0];  
            strLocalIP = ipAddress.ToString();  
            return strLocalIP;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取客户端内网IPv4地址集合  
    /// </summary>  
    /// <returns>返回客户端内网IPv4地址集合</returns>  
    public static List<string> GetClientLocalIPv4AddressList()  
    {  
        List<string> ipAddressList = new List<string>();  
        try  
        {  
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());  
            foreach (IPAddress ipAddress in ipHost.AddressList)  
            {  
                if (!ipAddressList.Contains(ipAddress.ToString()))  
                {  
                    ipAddressList.Add(ipAddress.ToString());  
                }  
            }  
        }  
        catch  
        {  
  
        }  
        return ipAddressList;  
    }  
  
    /// <summary>  
    /// 获取客户端外网IP地址  
    /// </summary>  
    /// <returns>客户端外网IP地址</returns>  
    public static string GetClientInternetIPAddress()  
    {  
        string strInternetIPAddress = string.Empty;  
        try  
        {  
            using (WebClient webClient = new WebClient())  
            {  
                strInternetIPAddress = webClient.DownloadString("http://www.coridc.com/ip");  
                Regex r = new Regex("[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}");  
                Match mth = r.Match(strInternetIPAddress);  
                if (!mth.Success)  
                {  
                    strInternetIPAddress = GetClientInternetIPAddress2();  
                    mth = r.Match(strInternetIPAddress);  
                    if (!mth.Success)  
                    {  
                        strInternetIPAddress = "unknown";  
                    }  
                }  
                return strInternetIPAddress;  
            }  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取本机公网IP地址  
    /// </summary>  
    /// <returns>本机公网IP地址</returns>  
    private static string GetClientInternetIPAddress2()  
    {  
        string tempip = "";  
        try  
        {  
            //http://iframe.ip138.com/ic.asp 返回的是：您的IP是：[220.231.17.99] 来自：北京市 光环新网  
            WebRequest wr = WebRequest.Create("http://iframe.ip138.com/ic.asp");  
            Stream s = wr.GetResponse().GetResponseStream();  
            StreamReader sr = new StreamReader(s, Encoding.Default);  
            string all = sr.ReadToEnd(); //读取网站的数据  
  
            int start = all.IndexOf("[") + 1;  
            int end = all.IndexOf("]", start);  
            tempip = all.Substring(start, end - start);  
            sr.Close();  
            s.Close();  
            return tempip;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取硬盘序号  
    /// </summary>  
    /// <returns>硬盘序号</returns>  
    public static string GetDiskID()  
    {  
        try  
        {  
            string strDiskID = string.Empty;  
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");  
            ManagementObjectCollection moc = mc.GetInstances();  
            foreach (ManagementObject mo in moc)  
            {  
                strDiskID = mo.Properties["Model"].Value.ToString();  
            }  
            moc = null;  
            mc = null;  
            return strDiskID;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取CpuID  
    /// </summary>  
    /// <returns>CpuID</returns>  
    public static string GetCpuID()  
    {  
        try  
        {  
            string strCpuID = string.Empty;  
            ManagementClass mc = new ManagementClass("Win32_Processor");  
            ManagementObjectCollection moc = mc.GetInstances();  
            foreach (ManagementObject mo in moc)  
            {  
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();  
            }  
            moc = null;  
            mc = null;  
            return strCpuID;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取操作系统类型  
    /// </summary>  
    /// <returns>操作系统类型</returns>  
    public static string GetSystemType()  
    {  
        try  
        {  
            string strSystemType = string.Empty;  
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");  
            ManagementObjectCollection moc = mc.GetInstances();  
            foreach (ManagementObject mo in moc)  
            {  
                strSystemType = mo["SystemType"].ToString();  
            }  
            moc = null;  
            mc = null;  
            return strSystemType;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取操作系统名称  
    /// </summary>  
    /// <returns>操作系统名称</returns>  
    public static string GetSystemName()  
    {  
        try  
        {  
            string strSystemName = string.Empty;  
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT PartComponent FROM Win32_SystemOperatingSystem");  
            foreach (ManagementObject mo in mos.Get())  
            {  
                strSystemName = mo["PartComponent"].ToString();  
            }  
            mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption FROM Win32_OperatingSystem");  
            foreach (ManagementObject mo in mos.Get())  
            {  
                strSystemName = mo["Caption"].ToString();  
            }  
            return strSystemName;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
    /// <summary>  
    /// 获取物理内存信息  
    /// </summary>  
    /// <returns>物理内存信息</returns>  
    public static string GetTotalPhysicalMemory()  
    {  
        try  
        {  
            string strTotalPhysicalMemory = string.Empty;  
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");  
            ManagementObjectCollection moc = mc.GetInstances();  
            foreach (ManagementObject mo in moc)  
            {  
                strTotalPhysicalMemory = mo["TotalPhysicalMemory"].ToString();  
            }  
            moc = null;  
            mc = null;  
            return strTotalPhysicalMemory;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
  
    /// <summary>  
    /// 获取主板id  
    /// </summary>  
    /// <returns></returns>  
    public static string GetMotherBoardID()  
    {  
        try  
        {  
            ManagementClass mc = new ManagementClass("Win32_BaseBoard");  
            ManagementObjectCollection moc = mc.GetInstances();  
            string strID = null;  
            foreach (ManagementObject mo in moc)  
            {  
                strID = mo.Properties["SerialNumber"].Value.ToString();  
                break;  
            }  
            return strID;  
        }  
        catch  
        {  
            return "unknown";  
        }  
    }  
  
    /// <summary>  
    /// 获取公用桌面路径           
public static string GetAllUsersDesktopFolderPath()  
{  
    RegistryKey folders;  
    folders = OpenRegistryPath(Registry.LocalMachine, @"/software/microsoft/windows/currentversion/explorer/shell folders");  
    string desktopPath = folders.GetValue("Common Desktop").ToString();  
    return desktopPath;  
}  
/// <summary>  
/// 获取公用启动项路径  
/// </summary>  
public static string GetAllUsersStartupFolderPath()  
{  
    RegistryKey folders;  
    folders = OpenRegistryPath(Registry.LocalMachine, @"/software/microsoft/windows/currentversion/explorer/shell folders");  
    string Startup = folders.GetValue("Common Startup").ToString();  
    return Startup;  
}  
private static RegistryKey OpenRegistryPath(RegistryKey root, string s)  
{  
    s = s.Remove(0, 1) + @"/";  
    while (s.IndexOf(@"/") != -1)  
    {  
        root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));  
        s = s.Remove(0, s.IndexOf(@"/") + 1);  
    }  
    return root;  
}  
private void Test()  
 {  
     RegistryKey folders;  
     folders = OpenRegistryPath(Registry.LocalMachine, @"/software/microsoft/windows/currentversion/explorer/shell folders");  
     // Windows用户桌面路径  
     string desktopPath = folders.GetValue("Common Desktop").ToString();  
     // Windows用户字体目录路径  
     string fontsPath = folders.GetValue("Fonts").ToString();  
     // Windows用户网络邻居路径  
     string nethoodPath = folders.GetValue("Nethood").ToString();  
     // Windows用户我的文档路径  
     string personalPath = folders.GetValue("Personal").ToString();  
     // Windows用户开始菜单程序路径  
     string programsPath = folders.GetValue("Programs").ToString();  
     // Windows用户存放用户最近访问文档快捷方式的目录路径  
     string recentPath = folders.GetValue("Recent").ToString();  
     // Windows用户发送到目录路径  
     string sendtoPath = folders.GetValue("Sendto").ToString();  
     // Windows用户开始菜单目录路径  
     string startmenuPath = folders.GetValue("Startmenu").ToString();  
     // Windows用户开始菜单启动项目录路径  
     string startupPath = folders.GetValue("Startup").ToString();  
     // Windows用户收藏夹目录路径  
     string favoritesPath = folders.GetValue("Favorites").ToString();  
     // Windows用户网页历史目录路径  
     string historyPath = folders.GetValue("History").ToString();  
     // Windows用户Cookies目录路径  
     string cookiesPath = folders.GetValue("Cookies").ToString();  
     // Windows用户Cache目录路径  
     string cachePath = folders.GetValue("Cache").ToString();  
     // Windows用户应用程式数据目录路径  
     string appdataPath = folders.GetValue("Appdata").ToString();  
     // Windows用户打印目录路径  
     string printhoodPath = folders.GetValue("Printhood").ToString();  
 }</span>  


    通过Win32_BaseBoard获取主板信息，但不是所有的主板都有编号，或者说不是能获取所有系统主板的编号。

    通过Win32_PhysicalMedia获取硬盘序列号号，通过Win32_DiskDrive获取的信息不包含SerialNumber,但似乎有DeviceId。

    通过Win32_BIOS获取BIOS信息，基本和获取主板信息差不多。就是说：不是所有的主板BIOS信息都有编号。

# 命令行中执行 wmic cpu 获取CPU信息

```sh
wmic cpu get processorid
```

# 参考

- [获取CPUID序列号的两种办法](https://blog.csdn.net/liulilittle/article/details/80958926)
- [C# 获取CPU序列号、MAC地址、硬盘ID等系统信息](https://blog.csdn.net/K346K346/article/details/86548358)
- [C# 读取串口设备列表](https://www.cnblogs.com/huanjun/p/11254792.html)
- [C# 读取机器码，CPU序列号，生成注册码类](https://blog.csdn.net/weixin_44713908/article/details/101770007)

- [How To Get Hardware Information (CPU ID, MainBoard Info, Hard Disk Serial, System Information , ...)](https://www.codeproject.com/articles/17973/how-to-get-hardware-information-cpu-id-mainboard-i)
- [How do I get the computer’s serial number? Consuming Windows Runtime classes in desktop apps, part 4: C#](https://devblogs.microsoft.com/oldnewthing/20180109-00/?p=97745)
- [C# 获取 PC 序列号的方法示例](https://www.jb51.net/article/144790.htm)

[C#获取电脑硬件信息（CPU ID、主板ID、硬盘ID、BIOS编号）](https://www.cnblogs.com/lenmom/p/8556611.html) 有介绍 Win32 获取网卡 

       public class Win32API   
        {   
            [DllImport("NETAPI32.DLL")]   
            public static extern char Netbios(ref NCB ncb);   
        }   


[How can I get the number of processors in the system, when there are more than 64?](https://devblogs.microsoft.com/oldnewthing/20200824-00/?p=104116)

[Get Volume Serial Number in C#](http://www.nullskull.com/articles/20021019.asp) 也使用了 Win32 API

[DllImport("kernel32.dll")]
private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, UInt32 VolumeNameSize, ref UInt32 VolumeSerialNumber, ref UInt32 MaximumComponentLength, ref UInt32 FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);



Here is the painful experience:
Desktops and servers are usually OK. Use combination of CPU ids and primary hard drive id. Laptops are pain in the butt, as people plug their laptops whereever they want and use hotswap hardeware, therefore some customers report expired licenses.
MAC addresses are BAD because connecting to VPNs or wireless networks or disabling-enabling adapters can change the number or order of network adapters. Your ID will be changing with every single USB or PCMCA NIC plugged in, or when user connects to VPN via Cisco or Nortel clients (they create virtual adapters).
Drive Volume IDs and motherboard IDs misbehave with docking stations and some Thinkpads. We were unable to identify the pattern.
We usually observed some customers to have 2 system IDs changing back and forth. At this moment we have three independent approaches towards software licensing:
USB dongle. Works 100% good, guarantees 1 use at a time. Adds some $$$ to the product price, which we normally can live with; Does not fit for domain-type licenses, where you have as many users as customers want;
WebLease(tm) technology (one of things we are planning to promote on the market) - program asks for confirmation from web server before calling a function. This requires internet connection, but allows pay-as-you-go type of service, monthly fees etc;
Weighted method: you have some processor IDS, some hard drive IDS, some MAC addresses, windows registration code, etc in a text file. Your program reads current system values and looks up these lists. If it fails to find at least 2 matching parameters, it fails (and customer has to request updated license file from you).
It could make things way easier if Windows could have an unique key code per install...

# 其他

- [C/C++获取cpu的id和名字](https://cumtchw.blog.csdn.net/article/details/124423775)
- [通过CPUID指令获取CPU信息](https://blog.csdn.net/qq_14976351/article/details/74983139)
- [Windows/Linux获取Mac地址和CPU序列号实现](https://blog.csdn.net/fengbingchun/article/details/108874436)