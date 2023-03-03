**C#使用Microsoft.Web.Administration操作IIS服务器站点【IIS7+】**

[toc]

> **IIS的Windows服务名称 `iisadmin`**

> `Microsoft.Web.Administration`是IIS7中引入的新的管理配置IIS的非常全面的托管API。
>
> 对于较高版本的IIS，推荐使用它。

> IIS includes Microsoft.Web.Administration, which is a new a management API for the web server that enables editing configuration through complete manipulation of the XML configuration 
files. It also provides convenience objects to manage the server, its properties and state.

# IIS操作的帮助类

```C#
using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Permissions;

/* 
 * using Microsoft.Web.Administration  [IIS 7.0 + 管理的托管API]------ 引用： Microsoft.Web.Administration.dll
 */
namespace HelperCollections
{
    /// <summary>
    /// IIS站点配置信息帮助类，使用Microsoft.Web.Administration 【IIS 7+】。严格来说只是操作IIS中的Web
    /// 只能操作本地IIS服务器
    /// </summary>
    public class IISSiteHelper_MWA
    {
        ServerManager serverManager = new ServerManager();

        /// <summary>  
        /// 获取本地IIS版本  
        /// </summary>  
        /// <returns></returns>  
        [RegistryPermissionAttribute(SecurityAction.PermitOnly, Read = @"HKEY_LOCAL_MACHINE\Software\Microsoft\InetStp")]
        public Version GetIISVersion()
        {
            using (RegistryKey componentsKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (componentsKey != null)
                {
                    int majorVersion = (int)componentsKey.GetValue("MajorVersion", -1);
                    int minorVersion = (int)componentsKey.GetValue("MinorVersion", -1);

                    if (majorVersion != -1 && minorVersion != -1)
                    {
                        return new Version(majorVersion, minorVersion);
                    }
                }

                return new Version(0, 0);
            }
        }
        /// <summary>
        /// 判断iis是否安装
        /// </summary>
        /// <returns></returns>
        [RegistryPermissionAttribute(SecurityAction.PermitOnly, Read = @"HKEY_LOCAL_MACHINE\Software\Microsoft\InetStp")]
        public bool IISInstalled()
        {
            try
            {
                using (RegistryKey iisKey = Registry.
                    LocalMachine.
                    OpenSubKey(@"Software\Microsoft\InetStp"))
                {
                    return (int)iisKey.GetValue("MajorVersion") >= 6;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>  
        /// 创建 IIS网站 【IIS7+】
        /// </summary>  
        /// <param name="webSiteName">网站名称</param>  
        /// <param name="physicalPath">物理路径</param>  
        /// <param name="ip">ip</param>  
        /// <param name="port">端口，不能为空，1024~65534</param>  
        /// <param name="hostName">主机名，即 域名</param>  
        /// <param name="isStart">WEB启动状态</param>  
        /// <param name="appPoolName">应用程序池，如果为空将使用默认DefaultAppPool</param>
        /// <returns></returns>  
        public void CreateWebSite(string webSiteName, string physicalPath, string ip = "", int port = 80, string hostName = "", string appPoolName = "")
        {
            //SetFileRole();

            #region 创建WebSite 和 AppPool
            var bindInfo = ((string.IsNullOrWhiteSpace(ip) || ip == "*") ? "" : ip) + $":{port}:{hostName}";

            Site mySite;
            if (serverManager.Sites[webSiteName] == null)
            {
                mySite = serverManager.Sites.Add(webSiteName, "HTTP", bindInfo, physicalPath);
            }
            else
            {
                mySite = serverManager.Sites[webSiteName];
                mySite.Applications[0].VirtualDirectories[0].PhysicalPath = physicalPath;
                mySite.Bindings.Add(bindInfo, "HTTP");

                //mySite.Applications[0].VirtualDirectoryDefaults
            }
   
            #region 创建 应用程序池
            ApplicationPool apppool = null;
            if (!string.IsNullOrWhiteSpace(appPoolName))
            {
                if (serverManager.ApplicationPools[appPoolName] == null)
                {
                    serverManager.ApplicationPools.Add(appPoolName);
                }

                apppool = serverManager.ApplicationPools[appPoolName];
                // 托管模式及其NET运行时版本
                //apppool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                //apppool.ManagedRuntimeVersion = "v4.0";
                //apppool.Enable32BitAppOnWin64 = true;

                mySite.Applications[0].ApplicationPoolName = appPoolName;

            }
            #endregion

            mySite.ServerAutoStart = true;

            //// 启用失败追踪日志
            //mySite.TraceFailedRequestsLogging.Enabled = true;
            //mySite.TraceFailedRequestsLogging.Directory = @"C:inetpub\customfolder\site";

            serverManager.CommitChanges();

            // Once the application pool configuration data is serialized to the file via the update call, you can execute the recycle method on it.
            // This recycle call is not necessary, since the application pool will simply be created and there is no need.
            // But this illustrates that action can be taken in objects that have been created only after they are serialized to disk and the server can fetch this configuration and act upon it.
            apppool?.Recycle();
            #endregion


            #region //启动aspnet_regiis.exe程序为网站安装 .NET4.0 CLR 版本 的脚本映射
            ////aspnet_regiis.exe -s <路径> 在指定的路径以递归方式安装此版本的脚本映射。
            ////例如，aspnet_regiis.exe - s W3SVC / 1 / ROOT / SampleApp1


            //var IISServer = "localhost";
            //var newSiteId = ""// 数字id

            //string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
            //ProcessStartInfo startInfo = new ProcessStartInfo(fileName);

            //DirectoryEntry rootEntry = new DirectoryEntry($"IIS://{IISServer}/W3SVC");
            //DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteId, "IIsWebServer");
            //DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");

            ////处理目录路径 
            //string path = vdEntry.Path.ToUpper();
            //int index = path.IndexOf("W3SVC");
            //path = path.Remove(0, index);
            ////启动ASPnet_iis.exe程序,刷新脚本映射 
            //startInfo.Arguments = "-s " + path;


            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //Process process = new Process();
            //process.StartInfo = startInfo;
            //process.Start();
            //process.WaitForExit();
            //string errors = process.StandardError.ReadToEnd();

            //if (errors != string.Empty)
            //{
            //    throw new Exception(errors);
            //} 
            #endregion

        }

        /// <summary>
        /// 创建虚拟路径/应用程序 【创建或更新，可重复执行】
        /// </summary>
        /// <param name="webSiteName">站点名称，在该站点下创建</param>
        /// <param name="app_vDir_Name">虚拟路径/应用程序名称。也是Path路径</param>
        /// <param name="physicalPath">物理路径</param>
        /// <param name="isApplication">是否创建更新应用程序</param>
        /// <param name="parentAppName">webSiteName下的应用程序名称，当创建更新虚拟目录时如果不指定，将在默认App下操作</param>
        /// <param name="appPoolName">应用程序池，如果为空将使用默认的DefaultAppPool;当创建、更新应用程序时，可指定应用程序池</param>
        public void CreateUpdateVDirApplication(string webSiteName, string app_vDir_Name, string physicalPath, bool isApplication = true, string parentAppName = "", string appPoolName = "")
        {
            var site = serverManager.Sites[webSiteName];
            if (site == null)
            {
                return;
            }
            if (isApplication)
            {
                // App          
                var app = site.Applications[app_vDir_Name];
                if (app == null)
                {
                    app = site.Applications.Add(app_vDir_Name, physicalPath);
                }
                else
                {
                    //app.Path = app_vDir_Name; 不用赋值，从path获取的
                    app.VirtualDirectories[0].PhysicalPath= physicalPath;
                }

                #region 创建 应用程序池
                ApplicationPool apppool = null;
                if (!string.IsNullOrWhiteSpace(appPoolName))
                {
                    apppool = serverManager.ApplicationPools[appPoolName];
                    if (apppool == null)
                    {
                        apppool=serverManager.ApplicationPools.Add(appPoolName);
                    }

                    // 托管模式及其NET运行时版本
                    //apppool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                    //apppool.ManagedRuntimeVersion = "v4.0";

                    app.ApplicationPoolName = appPoolName;
                }
                #endregion

                site.ServerAutoStart = true;

                serverManager.CommitChanges();
                apppool?.Recycle();
                return;
            }

            // vDir
            Application parentApp=null;
            if (!string.IsNullOrWhiteSpace(parentAppName))
            {
                parentApp = site.Applications[parentAppName];
            }
            if (parentApp == null)
            {
                parentApp = site.Applications[0];
            }

            var vDir = parentApp.VirtualDirectories[app_vDir_Name];
            if (vDir == null)
            {
                vDir = parentApp.VirtualDirectories.Add(app_vDir_Name, physicalPath);
            }
            else
            {
                vDir.PhysicalPath = physicalPath;
            }

            serverManager.CommitChanges();
        }

        #region IISWeb 启动/停止/删除 
        /// <summary>
        /// 启动站点
        /// </summary>
        /// <param name="webSiteName"></param>
        public void StartWebSite(String webSiteName)
        {
            var site = serverManager.Sites[webSiteName];
            if (site!=null)
            {
                site.Start();
            }
        }

        /// <summary>
        /// 停止站点
        /// </summary>
        /// <param name="webSiteName"></param>
        public void StopWebSite(String webSiteName)
        {
            var site = serverManager.Sites[webSiteName];
            if (site != null)
            {
                site.Stop();
            }
        }

        /// <summary>  
        /// 删除 站点
        /// <param name="webSiteName">网站名称</param>  
        /// </summary>  
        public void RemoveIISWebSite(String webSiteName)
        {
            var site = serverManager.Sites[webSiteName];
            if (site != null)
            {
                serverManager.Sites.Remove(site);
            }
        }        
        #endregion

        #region 设置文件/文件夹权限
        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="targetDirOrFile">文件夹/文件路径</param>
        public void SetFileRole(string targetDirOrFile)
        {
            string FileAdd = targetDirOrFile;
            FileAdd = FileAdd.Remove(FileAdd.LastIndexOf('\\'), 1);
            DirectorySecurity fSec = new DirectorySecurity();
            fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(FileAdd, fSec);
        }

        #endregion
    }
}
```

# C#的命令行工具创建IIS

> [Programmatically Create IIS Website and Application Pool Using C#](https://www.c-sharpcorner.com/UploadFile/satisharveti/programmatically-create-iis-website-and-application-pool-usi/)

```C#
static void Main(string[] args)  
{  
    Console.WriteLine("Do you want to create an Application Pool:y/n");  
    string response = Console.ReadLine();  
    if (response.ToString() == "y")  
    {  
        Console.Write("Please enter Application Pool Name:");  
        string poolname = Console.ReadLine();  
        bool isEnable32bit = false;  
        ManagedPipelineMode mode = ManagedPipelineMode.Classic;  
        Console.Write("Need to enable 32 bit on Windows 64 bit?y/n [Applicable for 64 bit OS]: y/n?");  
        string enable32bit = Console.ReadLine();  
        if (enable32bit.ToLower() == "y")  
        {  
            isEnable32bit = true;  
        }  
        Console.Write("Please select Pipeline Mode: 1 for Classic, 2 for Integrated:");  
        string pipelinemode = Console.ReadLine();  
        if (pipelinemode.ToLower() == "2")  
        {  
            mode = ManagedPipelineMode.Integrated;  
        }  
        Console.Write("Please select Runtime Version for Application Pool: 1 for v2.0, 2 for v4.0:");  
        string runtimeVersion = Console.ReadLine()== "1" ? "v2.0" : "v4.0";  
          
        CreateAppPool(poolname, isEnable32bit, mode, runtimeVersion);  
        Console.WriteLine("Application Pool created successfully...");  
    }  
                Console.WriteLine("Do you want to create a website:y/n");  
    response = Console.ReadLine();  
    if (response.ToString() == "y")  
    {  
        Console.Write("Please enter website name:");  
        string websiteName = Console.ReadLine();  
        Console.Write("Please enter host name:");  
        string hostname = Console.ReadLine();  
        Console.Write("Please enter physical path to point for website:");  
        string phypath = Console.ReadLine();  
        Console.WriteLine("Please enter Application pool Name:");  
        foreach(var pool in new ServerManager().ApplicationPools)  
        {  
            Console.WriteLine(pool.Name);  
        }  
        Console.WriteLine("");  
        Console.Write("Please enter Application pool Name for web site:");  
        string poolName = Console.ReadLine();  
        CreateIISWebsite(websiteName,hostname,phypath,poolName);  
        Console.WriteLine("Web site created successfully...");  
        Console.ReadLine();  
    }  
} 

private static void CreateIISWebsite(string websiteName, string hostname, string phyPath, string appPool)  
{  
    ServerManager iisManager = new ServerManager();  
    iisManager.Sites.Add(websiteName, "http", "*:80:" + hostname, phyPath);  
    iisManager.Sites[websiteName].ApplicationDefaults.ApplicationPoolName = appPool;  
  
    foreach (var item in iisManager.Sites[websiteName].Applications)  
    {  
        item.ApplicationPoolName = appPool;  
    }  
  
    iisManager.CommitChanges();  
}  

private static void CreateAppPool(string poolname,bool enable32bitOn64, ManagedPipelineMode mode,string runtimeVersion="v4.0")  
{  
    using (ServerManager serverManager = new ServerManager())  
    {  
        ApplicationPool newPool = serverManager.ApplicationPools.Add(poolname);  
        newPool.ManagedRuntimeVersion = runtimeVersion;  
        newPool.Enable32BitAppOnWin64 = true;  
        newPool.ManagedPipelineMode = mode;  
        serverManager.CommitChanges();  
    }  
}  
```

# 创建站点、AppPool、获取DNS信息、IIS服务管理

> 出自 [How to host website programmatically in IIS using C#?](https://www.asptricks.net/2016/08/how-to-host-website-programmatically-in.html)

用到了 `Microsoft.Web.Administration.dll`、`System.ServiceProcess.dll`

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Microsoft.Web.Administration;
using System.Net;
using HostHsmApp;

namespace Create_IIS_website
{
    class Program
    {

        private static void CreateAppPool(
            string poolname, 
            bool enable32bitOn64,
            ManagedPipelineMode mode,
            string runtimeVersion = "v4.0")
        {
            using (ServerManager serverManager = new ServerManager())
            {
                ApplicationPool newPool = serverManager.ApplicationPools.Add(poolname);
                newPool.ManagedRuntimeVersion = runtimeVersion;
                newPool.Enable32BitAppOnWin64 = true;
                newPool.ManagedPipelineMode = mode;
                serverManager.CommitChanges();
            }
        }


        private static void CreateWebsite(
            string websiteName,
            string hostname,
            string phyPath,
            string appPool,
            string port = "80")
        {
            ServerManager iisManager = new ServerManager();
            iisManager.Sites.Add(websiteName, "http", "*:"+ port +":" + hostname, phyPath);
            iisManager.Sites[websiteName].ApplicationDefaults.ApplicationPoolName = appPool;


            string ipAddress = "*";
            if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0)
            {
                string hostName = Dns.GetHostName(); 
                ipAddress  = Dns.GetHostByName(hostName).AddressList[0].ToString();
            }
            serverManager.Sites[websiteName].Bindings.Add(ipAddress + ":"+ port.ToString() + ":", "http");


            foreach (var item in iisManager.Sites[websiteName].Applications)
            {
                item.ApplicationPoolName = appPool;
            }

            iisManager.CommitChanges();
        }


        static void CreateIISWebsiteNow()
        {
            try
            {
                ServiceController sc = new ServiceController("World Wide Web Publishing Service");
                if ((sc.Status.Equals(ServiceControllerStatus.Stopped) || sc.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    Console.WriteLine("IIS service stopped, starting...");
                    sc.Start();
                }
                else
                {
                    Console.WriteLine("Service already running...");
                    sc.Stop();
                }

                if (sc.Status.Equals(ServiceControllerStatus.Running))
                {

                    Console.WriteLine("Do you want to create an Application Pool:y/n");
                    string response = Console.ReadLine();
                    if (response.ToString() == "y")
                    {
                        Console.Write("Enter Application Pool Name:");
                        string poolname = Console.ReadLine();
                        bool isEnable32bit = false;
                        ManagedPipelineMode mode = ManagedPipelineMode.Classic;
                        Console.Write("Need to enable 32 bit on Windows 64 bit?y/n [Applicable for 64 bit OS]: y/n?");
                        string enable32bit = Console.ReadLine();
                        if (enable32bit.ToLower() == "y")
                        {
                            isEnable32bit = true;
                        }
                        Console.Write("Select Pipeline Mode: 1 for Classic, 2 for Integrated:");
                        string pipelinemode = Console.ReadLine();
                        if (pipelinemode.ToLower() == "2")
                        {
                            mode = ManagedPipelineMode.Integrated;
                        }
                        Console.Write("Select Runtime Version for Application Pool: 1 for v2.0, 2 for v4.0:");
                        string runtimeVersion = Console.ReadLine() == "1" ? "v2.0" : "v4.0";

                        CreateAppPool(poolname, isEnable32bit, mode, runtimeVersion);
                        Console.WriteLine("Application Pool created successfully...");
                    }
                    Console.WriteLine("Do you want to create a website:y/n");
                    response = Console.ReadLine();
                    if (response.ToString() == "y")
                    {
                        Console.Write("Enter website name:");
                        string websiteName = Console.ReadLine();
                        Console.Write("Enter host name:");
                        string hostname = Console.ReadLine();
                        Console.Write("Enter physical path for website:");
                        string phypath = Console.ReadLine();
                        Console.WriteLine("Enter Application pool Name from listed name:");
                        foreach (var pool in new ServerManager().ApplicationPools)
                        {
                            Console.WriteLine(pool.Name);
                        }
                        Console.WriteLine("");
                        Console.Write("Please enter Application pool Name for web site:");
                        string poolName = Console.ReadLine();
                        CreateWebsite(websiteName, hostname, phypath, poolName);
                        Console.WriteLine("Website created successfully.");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error:" + exc.Message);
            }
        }

        static void Main(string[] args)
        {
            CreateIISWebsiteNow();
            Console.Read();
        }  
    }
}
```

# 参考

[How to Use Microsoft.Web.Administration](https://learn.microsoft.com/en-us/iis/manage/scripting/how-to-use-microsoftwebadministration)