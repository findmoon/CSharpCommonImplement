using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
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
                // 或者查看 VersionString
                if (componentsKey != null)
                {
                    int majorVersion = (int)componentsKey.GetValue("MajorVersion", -1);
                    int minorVersion = (int)componentsKey.GetValue("MinorVersion", -1);

                    if (majorVersion != -1 && minorVersion != -1)
                    {
                        return new Version(majorVersion, minorVersion);
                    }
                }

                return null;// new Version(0, 0);
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
                    // 判断 Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp", "VersionString", null) != null;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>  
        /// 创建或更新 IIS网站 【IIS7+】
        /// </summary>  
        /// <param name="webSiteName">网站名称</param>  
        /// <param name="physicalPath">物理路径</param>  
        /// <param name="ip">ip</param>  
        /// <param name="port">端口，不能为空，1024~65534；如果是更新，将会新增绑定</param>  
        /// <param name="hostName">主机名，即 域名</param> 
        /// <param name="appPoolName">应用程序池，如果为空将使用默认DefaultAppPool</param>
        /// <param name="enable32BitAppOnWin64">默认启用32位应用程序，如果为64位网站则不需要启用</param>
        /// <returns></returns>  
        public void CreateUpdateWebSite(string webSiteName, string physicalPath, string ip = "", ushort port = 80, string hostName = "", string appPoolName = "", bool enable32BitAppOnWin64 = true)
        {
            //SetFileRole();

            #region 创建WebSite 和 AppPool
            var bindInfo = ((string.IsNullOrWhiteSpace(ip) || ip == "*") ? "" : ip) + $":{port}:{hostName}";

            Site mySite;
            if (serverManager.Sites[webSiteName] == null)
            {
                if (string.IsNullOrWhiteSpace($"{ip}{hostName}"))
                {
                    mySite = serverManager.Sites.Add(webSiteName, physicalPath, port); // 为了更接近管理器中直接创建的结果
                }
                else
                {
                    mySite = serverManager.Sites.Add(webSiteName, "http", bindInfo, physicalPath);
                }
            }
            else
            {
                mySite = serverManager.Sites[webSiteName];
                mySite.Applications[0].VirtualDirectories[0].PhysicalPath = physicalPath;

                if (!mySite.Bindings.Any(b => b.ToString().EndsWith(bindInfo)))
                {
                    mySite.Bindings.Add(bindInfo, "http");
                }

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
                // 托管模式 默认为 集成模式【推荐】；.NET运行时版本 默认为 v4.0
                //apppool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                //apppool.ManagedRuntimeVersion = "v4.0";
                apppool.Enable32BitAppOnWin64 = enable32BitAppOnWin64;

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
        /// <param name="app_vDir_Name">虚拟路径/应用程序名称。实际为`path路径，如'/a/b'`</param>
        /// <param name="physicalPath">物理路径</param>
        /// <param name="isApplication">是否创建更新应用程序</param>
        /// <param name="parentAppName">webSiteName下的应用程序名称，实际为`path路径，如'/a/b'`。当创建更新虚拟目录时如果不指定，将在默认App下操作</param>
        /// <param name="appPoolName">应用程序池，如果为空将使用默认的DefaultAppPool;当创建、更新应用程序时，可指定应用程序池；虚拟目录指定AppPool无效</param>
        /// <param name="enable32BitAppOnWin64">默认启用32位应用程序，如果为64位网站则不需要启用</param>
        public void CreateUpdateVDirApplication(string webSiteName, string app_vDir_Name, string physicalPath, bool isApplication = true, string parentAppName = "", string appPoolName = "", bool enable32BitAppOnWin64 = true)
        {
            var site = serverManager.Sites[webSiteName];
            if (site == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(app_vDir_Name))
            {
                return;
            }
            if (!app_vDir_Name.StartsWith("/"))
            {
                app_vDir_Name = $"/{app_vDir_Name.Trim()}";
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
                    app.VirtualDirectories[0].PhysicalPath = physicalPath;
                }

                #region 创建 应用程序池
                ApplicationPool apppool = null;
                if (!string.IsNullOrWhiteSpace(appPoolName))
                {
                    apppool = serverManager.ApplicationPools[appPoolName];
                    if (apppool == null)
                    {
                        apppool = serverManager.ApplicationPools.Add(appPoolName);
                    }

                    // 托管模式 默认为 集成模式【推荐】；.NET运行时版本 默认为 v4.0
                    //apppool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

                    //apppool.ManagedRuntimeVersion = "v4.0";
                    apppool.Enable32BitAppOnWin64 = enable32BitAppOnWin64;

                    app.ApplicationPoolName = appPoolName;
                }
                #endregion

                site.ServerAutoStart = true;

                serverManager.CommitChanges();
                apppool?.Recycle();
                return;
            }

            // vDir
            Application parentApp = null;
            if (!string.IsNullOrWhiteSpace(parentAppName))
            {
                if (!parentAppName.StartsWith("/"))
                {
                    parentAppName = $"/{parentAppName.Trim()}";
                }
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
            if (site != null)
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
                serverManager.CommitChanges();
            }
        }

        /// <summary>  
        /// 删除 应用程序
        /// <param name="appName">应用程序名称，实际为`path路径，如'/a/b'`</param>  
        /// <param name="webSiteName">网站名称，通常应该指定</param>  
        /// </summary>  
        public void RemoveApp(string appName, string webSiteName = "")
        {
            if (string.IsNullOrWhiteSpace(appName)) // 默认APP。即Site
            {
                return;
            }
            if (!appName.StartsWith("/"))
            {
                appName = $"/{appName.Trim()}";
            }
            if (string.IsNullOrWhiteSpace(webSiteName))
            {
                // 查找所有site的appName
                foreach (var site in serverManager.Sites)
                {
                    var app = site.Applications[appName];
                    if (app != null)
                    {
                        site.Applications.Remove(app);
                        serverManager.CommitChanges();
                        break;
                    }
                }
            }
            else
            {
                var site = serverManager.Sites[webSiteName];
                if (site != null)
                {
                    var app = site.Applications[appName];
                    if (app != null)
                    {
                        site.Applications.Remove(app);
                        serverManager.CommitChanges();
                    }
                }
            }
        }

        /// <summary>  
        /// 删除 虚拟目录
        /// <param name="webSiteName">虚拟目录名称，实际为`path路径，如'/a/b'`</param> 
        /// <param name="appName">应用程序名称，通常应该指定，实际为`path路径，如'/a/b'`</param>  
        /// <param name="webSiteName">网站名称，通常应该指定</param>  
        /// </summary>  
        public void RemoveVDir(string vDirName, string appName = "", string webSiteName = "")
        {
            if (string.IsNullOrWhiteSpace(vDirName)) // 默认APP的默认Vdir。即Site
            {
                return;
            }
            if (!vDirName.StartsWith("/"))
            {
                vDirName = $"/{vDirName.Trim()}";
            }
            if (!string.IsNullOrWhiteSpace(appName) && !appName.StartsWith("/"))
            {
                appName = $"/{appName.Trim()}";
            }
            if (string.IsNullOrWhiteSpace(webSiteName))
            {
                // 查找所有site的appName
                foreach (var site in serverManager.Sites)
                {
                    if (string.IsNullOrWhiteSpace(appName))
                    {
                        foreach (var currApp in site.Applications)
                        {
                            var vDir = currApp.VirtualDirectories[vDirName];
                            if (vDir != null)
                            {
                                currApp.VirtualDirectories.Remove(vDir);
                                serverManager.CommitChanges();
                                return;
                                //break;
                            }
                        }
                    }
                    else
                    {
                        var app = site.Applications[appName];
                        if (app != null)
                        {
                            var vDir = app.VirtualDirectories[vDirName];
                            if (vDir != null)
                            {
                                app.VirtualDirectories.Remove(vDir);
                                serverManager.CommitChanges();
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                var site = serverManager.Sites[webSiteName];
                if (site != null)
                {
                    if (string.IsNullOrWhiteSpace(appName))
                    {
                        foreach (var currApp in site.Applications)
                        {
                            var vDir = currApp.VirtualDirectories[vDirName];
                            if (vDir != null)
                            {
                                currApp.VirtualDirectories.Remove(vDir);
                                serverManager.CommitChanges();
                                return;
                                //break;
                            }
                        }
                    }
                    else
                    {
                        var app = site.Applications[appName];
                        if (app != null)
                        {
                            var vDir = app.VirtualDirectories[vDirName];
                            if (vDir != null)
                            {
                                app.VirtualDirectories.Remove(vDir);
                                serverManager.CommitChanges();
                                return;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region IISWe 站点状态
        /// <summary>
        /// 获取Web站点的状态
        /// </summary>
        /// <param name="webSiteName"></param>
        /// <returns>如果站点不存在，将返回null</returns>
        public ObjectState? WebSiteState(String webSiteName)
        {
            var site = serverManager.Sites[webSiteName];
            if (site != null)
            {
                return site.State;
            }
            return null;
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
