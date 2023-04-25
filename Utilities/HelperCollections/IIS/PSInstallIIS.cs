using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections.IIS
{
    /// <summary>
    /// PowerShell 安装 IIS及模块，启用Windows功能 【管理员权限运行】
    /// System.Management.Automation.dll
    /// C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Management.Automation\v4.0_3.0.0.0__31bf3856ad364e35
    /// </summary>
    public class PSInstallIIS
    {
        /// <summary>
        /// 使用 逗号分隔多个参数 会 报错 内部异常 COMException: 功能名称 IIS-ApplicationInit,IIS-WebSockets 未知。
        /// PowerShell命令行中可以，比如 Enable-WindowsOptionalFeature -Online -FeatureName IIS-ApplicationInit,IIS-WebSockets
        /// </summary>
        /// <returns>是否需要重启系统</returns>
        public static bool Instll()
        {
            var RestartNeeded=false;
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Enable-WindowsOptionalFeature");
            ps.AddParameter("Online");
            ps.AddParameter("FeatureName", "IIS-ApplicationInit");
            //ps.AddParameter("FeatureName", "IIS-ApplicationInit,IIS-WebSockets");
            // 
            Collection<PSObject> results = ps.Invoke();
            foreach (PSObject result in results)
            {
                PSMemberInfoCollection<PSMemberInfo> memberInfos = result.Members;
                Debug.WriteLine(memberInfos["Path"].Value);
                Debug.WriteLine(memberInfos["Online"].Value);
                if(!RestartNeeded) RestartNeeded= (bool)memberInfos["RestartNeeded"].Value;
                Debug.WriteLine(memberInfos["RestartNeeded"].Value);
            }
            
            ps.AddCommand("Enable-WindowsOptionalFeature");
            ps.AddParameter("Online");
            ps.AddParameter("FeatureName", "IIS-WebSockets");
            // 
            results = ps.Invoke();
            foreach (PSObject result in results)
            {
                PSMemberInfoCollection<PSMemberInfo> memberInfos = result.Members;
                Debug.WriteLine(memberInfos["Path"].Value);
                Debug.WriteLine(memberInfos["Online"].Value);
                if(!RestartNeeded) RestartNeeded= (bool)memberInfos["RestartNeeded"].Value;
                Debug.WriteLine(memberInfos["RestartNeeded"].Value);
            }
            return RestartNeeded;
        }
        /// <summary>
        /// 测试ok
        /// </summary>
        public static void Test()
        {
            PowerShell ps = PowerShell.Create();
            ps.AddCommand("Get-Process");
            ps.AddParameter("Name", "msedge");
            Collection<PSObject> results = ps.Invoke();
            foreach (PSObject result in results)
            {
                PSMemberInfoCollection<PSMemberInfo> memberInfos = result.Members;
                Debug.WriteLine(memberInfos["id"].Value);
            }

        }
    }
}
