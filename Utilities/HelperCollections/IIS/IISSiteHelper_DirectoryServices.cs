using HelperCollections.IIS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HelperCollections
{
    /// <summary>
    /// 主要用于 IIS6 的配置操作。严格来说只是IIS中的Web
    /// 
    /// 由于IIS6，不推荐使用
    /// 更多参见 https://learn.microsoft.com/en-us/previous-versions/iis/6.0-sdk/ms524896(v=vs.90)
    /// 
    /// 引用 System.DirectoryServices.dll
    /// Windows功能 启用 - “IIS 元数据库和IIS 6 配置兼容性”
    /// </summary>
    public class IISSiteHelper_DirectoryServices
    {
        public DirectoryEntry IISEntry { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iisServer">servername，通常为本地localhost</param>
        public IISSiteHelper_DirectoryServices(string iisServer= "localhost")
        {
            //  metabasePath is of the form "IIS://<servername>/<path>"
            //    for example "IIS://localhost/W3SVC/1/Root/MyVDir" 
            //    or "IIS://localhost/W3SVC/AppPools/MyAppPool"
            //new DirectoryEntry(metabasePath);

            // FTP则应是”MSFTPSVC”

            IISEntry = new DirectoryEntry($"IIS://{iisServer}/w3svc");
        }

        /// <summary>  
        /// 获取本地IIS版本  
        /// </summary>  
        /// <returns></returns>  
        public String GetIISVersion()
        {
            try
            {
                //DirectoryEntry entry = new DirectoryEntry("IIS://" + _hostName + "/W3SVC/INFO");
                //DirectoryEntry entry = IISEntry.Children.Find("INFO"); // System.AccessViolationException:“尝试读取或写入受保护的内存。这通常指示其他内存已损坏。”
                DirectoryEntry entry=null;
                foreach (DirectoryEntry childEntry in IISEntry.Children)
                {
                    if (childEntry.Name== "INFO")
                    {
                        entry = childEntry;
                        break;
                    }
                }
                if (entry==null)
                {
                    return String.Empty;
                }
                var MinorIIsVersionNumber = entry.Properties["MajorIISVersionNumber"].Value?.ToString();
                    return entry.Properties["MajorIISVersionNumber"].Value.ToString()+(string.IsNullOrEmpty(MinorIIsVersionNumber)?"": ($".{MinorIIsVersionNumber}"));
            }
            catch (Exception se)
            {
                //说明一点:IIS5.0中没有(int)entry.Properties["MajorIISVersionNumber"].Value;属性，  
                //将抛出异常 证明版本为 5.0  
                return String.Empty;
            }
        }



        /// <summary>  
        /// 创建 IIS网站  
        /// </summary>  
        /// <param name="webSiteName">网站名称</param>  
        /// <param name="physicalPath">物理路径</param>  
        /// <param name="ip">ip</param>  
        /// <param name="port">端口，不能为空，1024~65534</param>  
        /// <param name="hostName">主机名，即 域名</param>  
        /// <param name="isStart">WEB启动状态</param>  
        /// <returns></returns>  
        public void CreateWebSite(string webSiteName, string physicalPath, string ip="", int port=80,string hostName="", bool isStart=true)
        {
            // 为新WEB站点查找一个未使用的ID  
            int siteID = 1;
            foreach (DirectoryEntry e in IISEntry.Children)
            {
                if (e.SchemaClassName.Equals("IIsWebServer", StringComparison.OrdinalIgnoreCase))
                {
                    int ID = Convert.ToInt32(e.Name);
                    if (ID >= siteID) { siteID = ID + 1; }
                }
            }

            //是否应该创建目录  
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(physicalPath);
            if (!dir.Exists) { dir.Create(); }

            DirectoryEntry newSite = IISEntry.Children.Add(siteID.ToString(), "IIsWebServer");
            newSite.Properties["ServerComment"].Value = webSiteName;
            newSite.Properties["ServerBindings"].Value = ((string.IsNullOrWhiteSpace(ip) || ip == "*") ? "" : ip) + $":{port}:{hostName}";
            newSite.CommitChanges();

            DirectoryEntry newVDRoot = newSite.Children.Add("Root", "IIsWebVirtualDir");
            newVDRoot.Properties["Path"].Value = physicalPath;
            //newVDRoot.Properties["ServerAutoStart"].Value = isStart;//网站是否启动，默认启动
            newVDRoot.CommitChanges();

            // 其他属性设置
            //newVDRoot.Invoke("AppCreate", true);//创建应用程序，默认第一个Root VirtualDir为应用程序，不用单独指定
            //newVDRoot.Properties["AccessRead"][0] = true; //设置读取权限
            //newVDRoot.Properties["AccessWrite"][0] = true;
            //newVDRoot.Properties["AccessExecute"][0] = false;
            //newVDRoot.Properties["AccessScript"].Value = true; // System.Runtime.InteropServices.COMException:“异常来自 HRESULT:0x80005005”
            //newVDRoot.Properties["DefaultDoc"][0] = "Login_gdzc.aspx";//设置默认文档
            //newVDRoot.Properties["AppFriendlyName"][0] = "LabManager"; //应用程序名称           
            //newVDRoot.Properties["AuthFlags"][0] = 1;//0表示不允许匿名访问,1表示就可以3为基本身份验证，7为windows继承身份验证
            //newVDRoot.CommitChanges();

            #region 此处更像创建后的 Put 更新
            //// 创建WEB站点  
            //DirectoryEntry site = (DirectoryEntry)IISEntry.Invoke(State.Create.ToString(), new Object[] { "IIsWebServer", siteID });
            //site.Invoke("Put", "ServerComment", webSiteName);
            //site.Invoke("Put", "KeyType", "IIsWebServer");
            //site.Invoke("Put", "ServerBindings", domainPort + ":");
            //site.Invoke("Put", "ServerState", serverState);
            ////site.Invoke("Put", "FrontPageWeb", 1);  
            ////site.Invoke("Put", "DefaultDoc", "Login.html");  
            //// site.Invoke("Put", "SecureBindings", ":443:");  
            ////site.Invoke("Put", "ServerAutoStart", 1);  
            ////site.Invoke("Put", "ServerSize", 1);  
            //site.Invoke("SetInfo");
            //// 创建应用程序虚拟目录  
            //DirectoryEntry siteVDir = site.Children.Add("Root", "IISWebVirtualDir");
            //siteVDir.Properties["Path"][0] = physicalPath;

            ////siteVDir.Properties["AppIsolated"][0] = 2;  
            ////siteVDir.Properties["AccessFlags"][0] = 513;  
            ////siteVDir.Properties["FrontPageWeb"][0] = 1;  
            ////siteVDir.Properties["AppRoot"][0] = "LM/W3SVC/" + siteID + "/Root";  
            ////siteVDir.Properties["AppFriendlyName"][0] = "Root";  
            //siteVDir.CommitChanges();
            //site.CommitChanges(); 
            #endregion
        }
        /// <summary>  
        /// 更新 IIS网站  
        /// </summary>  
        /// <param name="oldWebSiteName">旧网站名称</param>  
        /// <param name="webSiteName">网站名称</param>  
        /// <param name="physicalPath">物理路径</param>  
        /// <param name="ip">ip</param>  
        /// <param name="port">端口，不能为空，1024~65534</param>  
        /// <param name="hostName">主机名，即 域名</param>  
        /// <returns></returns>  
        public void UpdateIISWebSite(String oldWebSiteName, string webSiteName, string physicalPath, string ip = "", int port = 80, string hostName = "")
       {
           DirectoryEntry childrenEntry = this.GetWebEntry(oldWebSiteName);
           childrenEntry.Properties["ServerComment"].Value = webSiteName;
           //childrenEntry.Properties["ServerState"].Value = serverState;
           childrenEntry.Properties["ServerBindings"].Value = ((string.IsNullOrWhiteSpace(ip) || ip == "*") ? "" : ip) + $":{port}:{hostName}";
           //更新程序所在目录  
           foreach (DirectoryEntry childrenDir in childrenEntry.Children)
           {
               if (childrenDir.Name=="Root" && childrenDir.SchemaClassName.Equals("IISWebVirtualDir", StringComparison.OrdinalIgnoreCase))
               {
                   childrenDir.Properties["Path"].Value = physicalPath;
                    childrenDir.CommitChanges();
                    break;
               }
           }

        }


        static void SetSingleProperty(string metabasePath, string propertyName, object newValue)
        {
            //  metabasePath is of the form "IIS://<servername>/<path>"
            //    for example "IIS://localhost/W3SVC/1" 
            //  propertyName is of the form "<propertyName>", for example "ServerBindings"
            //  value is of the form "<intStringOrBool>", for example, ":80:"
            Console.WriteLine("\nSetting single property at {0}/{1} to {2} ({3}):",
                metabasePath, propertyName, newValue, newValue.GetType().ToString());

            try
            {
                DirectoryEntry path = new DirectoryEntry(metabasePath);
                PropertyValueCollection propValues = path.Properties[propertyName];
                string oldType = propValues.Value.GetType().ToString();
                string newType = newValue.GetType().ToString();
                Console.WriteLine(" Old value of {0} is {1} ({2})", propertyName, propValues.Value, oldType);
                if (newType == oldType)
                {
                    path.Properties[propertyName][0] = newValue;
                    path.CommitChanges();
                    Console.WriteLine("Done");
                }
                else
                    Console.WriteLine(" Failed in SetSingleProperty; type of new value does not match property");
            }
            catch (Exception ex)
            {
                if ("HRESULT 0x80005006" == ex.Message)
                    Console.WriteLine(" Property {0} does not exist at {1}", propertyName, metabasePath);
                else
                    Console.WriteLine("Failed in SetSingleProperty with the following exception: \n{0}", ex.Message);
            }
        }


        static void CreateApplication(string metabasePath, string vDirName, string physicalPath)
        {

        }


static void CreateVDir(string metabasePath, string vDirName, string physicalPath)
        {
            //  metabasePath is of the form "IIS://<servername>/<service>/<siteID>/Root[/<vdir>]"
            //    for example "IIS://localhost/W3SVC/1/Root" 
            //  vDirName is of the form "<name>", for example, "MyNewVDir"
            //  physicalPath is of the form "<drive>:\<path>", for example, "C:\Inetpub\Wwwroot"
            Console.WriteLine("\nCreating virtual directory {0}/{1}, mapping the Root application to {2}:",
                metabasePath, vDirName, physicalPath);

            try
            {
                DirectoryEntry site = new DirectoryEntry(metabasePath);
                string className = site.SchemaClassName.ToString();
                if ((className.EndsWith("Server")) || (className.EndsWith("VirtualDir")))
                {
                    DirectoryEntries vdirs = site.Children;
                    DirectoryEntry newVDir = vdirs.Add(vDirName, (className.Replace("Service", "VirtualDir")));
                    newVDir.Properties["Path"][0] = physicalPath;
                    newVDir.Properties["AccessScript"][0] = true;
                    // These properties are necessary for an application to be created.
                    newVDir.Properties["AppFriendlyName"][0] = vDirName;
                    newVDir.Properties["AppIsolated"][0] = "1";
                    newVDir.Properties["AppRoot"][0] = "/LM" + metabasePath.Substring(metabasePath.IndexOf("/", ("IIS://".Length)));

                    newVDir.CommitChanges();

                    Console.WriteLine(" Done.");
                }
                else
                    Console.WriteLine(" Failed. A virtual directory can only be created in a site or virtual directory node.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in CreateVDir with the following exception: \n{0}", ex.Message);
            }
        }

        #region IISWeb 启动/停止/删除 
        /// <summary>
        /// 通过 serverComment站点名称 启动
        /// </summary>
        /// <param name="serverComment"></param>
        public void StartWebSite(String serverComment)
       {
            this.WebEnable(serverComment, IISState.Start);
        }

        /// <summary>
        /// 通过 Entry Name(站点id) 启动
        /// </summary>
        public void StartWebSiteByNameId(string name)
       {
            WebEnableByNameId(name,     IISState.Start);
        }
        /// <summary>
        /// 通过 serverComment站点名称 重置
        /// </summary>
        /// <param name="serverComment"></param>
        public void ResetWebSite(String serverComment)
       {
            WebEnable(serverComment, IISState.Reset);
        }

        /// <summary>
        /// 通过 Entry Name(站点id) 重置
        /// </summary>
        public void ResetWebSiteByNameId(string name)
       {
            WebEnableByNameId(name, IISState.Reset);
       }
        /// <summary>
        /// 通过 serverComment站点名称 停止
        /// </summary>
        /// <param name="serverComment"></param>
        public void StopWebSite(String serverComment)
       {
            WebEnable(serverComment, IISState.Stop);
        }
        /// <summary>
        /// 通过 Entry Name(站点id) 停止
        /// </summary>
        public void StopWebSiteByNameId(string name)
       {
           WebEnableByNameId(name, IISState.Stop);
       }
        /// <summary>  
        /// 依据 ServerComment网站名称 删除站点
        /// <param name="serverComment">网站名称:如:Test</param>  
        /// </summary>  
        public void RemoveIISWebSite(String serverComment)
        {
            DirectoryEntry siteEntry = this.GetWebEntry(serverComment);
            RemoveIISWebSite(siteEntry);
        }

        private static void RemoveIISWebSite(DirectoryEntry siteEntry)
        {
            //WebEnable(siteEntry,IISState.Stop);
            //先停止IIS站点  
            siteEntry.DeleteTree();
            // 不用调用CommitChanges
            //siteEntry.CommitChanges(); // System.IO.DirectoryNotFoundException:“系统找不到指定的路径。 (异常来自 HRESULT: 0x80070003)”
        }

        /// <summary>  
        /// 依据网站名称在IIS中的排列顺序删除  
        /// <param name="name">排列顺序:如:Test的</param>  
        /// </summary>  
        public void RemoveIISWebSiteByNameId(string name)
       {
            DirectoryEntry siteEntry = this.GetWebEntryByNameId(name);
            RemoveIISWebSite(siteEntry);
       }
        /// <summary>
        /// 启用Web状态，启动、停止、重置
        /// </summary>
        /// <param name="serverComment"></param>
        /// <param name="state"></param>
       private void WebEnable(string serverComment, IISState state)
       {
           DirectoryEntry siteEntry = GetWebEntry(serverComment);
            WebEnable(siteEntry, state);
        }
         /// <summary>
        /// 启用Web状态，启动、停止、重置
        /// </summary>
        /// <param name="serverComment"></param>
        /// <param name="state"></param>
       private void WebEnableByNameId(string name, IISState state)
       {
           DirectoryEntry siteEntry = GetWebEntryByNameId(name);
            WebEnable(siteEntry, state);
        }

        /// <summary>
        /// 启用Web状态，启动、停止、重置
        /// </summary>
        /// <param name="serverComment"></param>
        /// <param name="state"></param>
        private void WebEnable(DirectoryEntry siteEntry, IISState state)
       {
           siteEntry?.Invoke(state.ToString(), new Object[] { });
       }
        /// <summary>
        /// 获取 web 的 DirectoryEntry 【根据站点id】
        /// </summary>
        /// <param name="name">通常为站点对应的id</param>
        /// <returns></returns>
        private DirectoryEntry GetWebEntryByNameId(string entryName)
       {
           foreach (DirectoryEntry entry in IISEntry.Children)
           {
               if (entry.SchemaClassName.Equals("IIsWebServer", StringComparison.OrdinalIgnoreCase))
               {
                   if (entry.Name
                       .Equals(entryName))
                   {
                       return entry;
                   }
               }
           }
           return null;
       }
        /// <summary>
        /// 获取 web 的 DirectoryEntry【根据 serverComment 站点名称】
        /// </summary>
        /// <param name="serverComment">serverComment 站点名称</param>
        /// <returns></returns>
        private DirectoryEntry GetWebEntry(string serverComment)
       {
           foreach (DirectoryEntry entry in IISEntry.Children)
           {
               if (entry.SchemaClassName.Equals("IIsWebServer", StringComparison.OrdinalIgnoreCase))
               {
                   if (entry.Properties["ServerComment"].Value.ToString()
                       .Equals(serverComment, StringComparison.OrdinalIgnoreCase))
                   {
                       return entry;
                   }
               }
           }
           return null;
       }


        #endregion

        /*
           #region 注释得到物理路径+程序池  
           / <summary>  
           / 得到网站的物理路径  
           / </summary>  
           / <param name = "rootEntry" > 网站节点 </ param >
           / < returns ></ returns >
           //private String GetWebsitePhysicalPath(DirectoryEntry rootEntry)  
           //{  
           //    return GetDirectoryEntryChildren(rootEntry, "Path");  
           //}  
           / < summary >
           / 得到网站的物理路径
           / </ summary >
           / < param name="rootEntry">网站节点</param>  
           / <returns></returns>  
           //private String GetAppPoop(DirectoryEntry rootEntry)  
           //{  
           //    return GetDirectoryEntryChildren(rootEntry, "AppPoolId");  
           //}  
           //private String GetDirectoryEntryChildren(DirectoryEntry rootEntry, String properties)  
           //{  
           //    String propValue = String.Empty;  
           //    foreach (DirectoryEntry childEntry in rootEntry.Children)  
           //    {  
           //        if ((childEntry.SchemaClassName.Equals("IIsWebVirtualDir", StringComparison.OrdinalIgnoreCase))  
           //            && (childEntry.Name.Equals("root", StringComparison.OrdinalIgnoreCase)))  
           //        {  
           //            if (childEntry.Properties[properties].Value != null)  
           //            {  
           //                propValue = childEntry.Properties[properties].Value.ToString();  
           //            }  
           //        }  
           //    }  
           //    return propValue;  
           //}   
           #endregion  

           public bool AnyServerComment(String serverComment)
            {
                List<IISWebManager> list = this.ServerBindings();
                Boolean flag = list.Any<IISWebManager>(w => w.ServerComment.Equals(serverComment, StringComparison.OrdinalIgnoreCase));
                return flag;
            }
            public bool AnyDomainProt(Int32 domainPort)
            {
                List<IISWebManager> list = this.ServerBindings();
                Boolean flag = list.Any<IISWebManager>(w => w.DomainPort == domainPort);
                return flag;
            }
            /// <summary>  
            /// 获取站点信息  
            /// </summary>  
            public List<IISWebManager> ServerBindings()
            {
                List<IISWebManager> iislist = new List<IISWebManager>();
                DirectoryEntry rootChildrenEntry = null;
                IEnumerator enumeratorRoot = null;
                foreach (DirectoryEntry entry in rootEntry.Children)
                {
                    if (entry.SchemaClassName.Equals("IIsWebServer", StringComparison.OrdinalIgnoreCase))
                    {
                        var props = entry.Properties;

                        // if (props["ServerComment"][0].ToString().Contains("Default")) { continue; }  
                        //获取网站绑定的IP，端口，主机头  
                        String[] serverBindings = props["ServerBindings"].Value.ToString().Split(':');
                        var physicalPath = "";
                        var appPoolId = "";
                        enumeratorRoot = entry.Children.GetEnumerator();
                        while (enumeratorRoot.MoveNext())
                        {
                            rootChildrenEntry = (DirectoryEntry)enumeratorRoot.Current;
                            appPoolId = rootChildrenEntry.Properties["AppPoolId"].Value.ToString();
                            if (rootChildrenEntry.SchemaClassName.Equals("IIsWebVirtualDir", StringComparison.OrdinalIgnoreCase))
                            {
                                physicalPath = rootChildrenEntry.Properties["Path"].Value.ToString();
                                break;
                            }
                        }

                        iislist.Add(new IISWebManager()
                        {
                            Name = Convert.ToInt32(entry.Name),
                            ServerComment = props["ServerComment"].Value.ToString(),
                            DomainIP = serverBindings[0].ToString(),
                            DomainPort = Convert.ToInt32(serverBindings[1]),
                            ServerState = props["ServerState"][0].ToString(),//运行状态  
                            PhysicalPath = physicalPath,
                            AppPoolId = appPoolId
                        });

                        //String EnableDeDoc = props["EnableDefaultDoc"][0].ToString();  
                        //String DefaultDoc = props["DefaultDoc"][0].ToString();//默认文档  
                        //String MaxConnections = props["MaxConnections"][0].ToString();//iis连接数,-1为不限制  
                        //String ConnectionTimeout = props["ConnectionTimeout"][0].ToString();//连接超时时间  
                        //String MaxBandwidth = props["MaxBandwidth"][0].ToString();//最大绑定数  
                        //String ServerState = props["ServerState"][0].ToString();//运行状态  
                        //var ServerComment = (String)Server.Properties["ServerComment"][0];  
                        //var AccessRead = (Boolean)Server.Properties["AccessRead"][0];  
                        //var AccessScript = (Boolean)Server.Properties["AccessScript"][0];  
                        //var DefaultDoc = (String)Server.Properties["DefaultDoc"][0];  
                        //var EnableDefaultDoc = (Boolean)Server.Properties["EnableDefaultDoc"][0];  
                        //var EnableDirBrowsing = (Boolean)Server.Properties["EnableDirBrowsing"][0];  
                        //var Port = Convert.ToInt32(((String)Server.Properties["Serverbindings"][0])  
                        //   .Substring(1, ((String)Server.Properties["Serverbindings"][0]).Length - 2));  

                        //ieRoot = Root.Children.GetEnumerator();  
                        //while (ieRoot.MoveNext())  
                        //{  
                        //    VirDir = (DirectoryEntry)ieRoot.Current;  
                        //    if (VirDir.SchemaClassName != "IIsWebVirtualDir" && VirDir.SchemaClassName != "IIsWebDirectory")  
                        //        continue;  
                        //    var TName = VirDir.Name;  
                        //    var TAccessRead = (Boolean)VirDir.Properties["AccessRead"][0];  
                        //    var TAccessScript = (Boolean)VirDir.Properties["AccessScript"][0];  
                        //    var TDefaultDoc = (String)VirDir.Properties["DefaultDoc"][0];  
                        //    var TEnableDefaultDoc = (Boolean)VirDir.Properties["EnableDefaultDoc"][0];  
                        //    if (VirDir.SchemaClassName == "IIsWebVirtualDir")  
                        //    {  
                        //        var TPath = (String)VirDir.Properties["Path"][0];  
                        //    }  
                        //    else if (VirDir.SchemaClassName == "IIsWebDirectory")  
                        //    {  
                        //        var TPath = Root.Properties["Path"][0] + @"\" + VirDir.Name;  
                        //    }  
                        //}  
                        取全部的字段
                        //String str = "";  
                        //System.DirectoryServices.PropertyCollection props = entry.Properties;  
                        //foreach (String name in props.PropertyNames)  
                        //{  
                        //    foreach (Object o in props[name])  
                        //    {  
                        //        str += name.ToString() + ":" + o.ToString() + "\n";  
                        //    }  
                        //}  
                   }
                }
                return iislist;
            }



            public List<IISAppPoolInfo> GetAppPools()
            {
                List<IISAppPoolInfo> list = new List<IISAppPoolInfo>();
                DirectoryEntry appPoolEntry = new DirectoryEntry(String.Format("IIS://{0}/W3SVC/AppPools", _hostName));
                foreach (DirectoryEntry entry in appPoolEntry.Children)
                {
                    var schmeName = entry.Name;
                    list.Add(new IISAppPoolInfo()
                    {
                        AppPoolName = entry.Name,
                        AppPoolIdentityType = entry.Properties["AppPoolIdentityType"].Value.ToString(),
                        AppPoolCommand = Convert.ToInt32(entry.Properties["AppPoolCommand"].Value),
                        AppPoolState = Convert.ToInt32(entry.Properties["AppPoolState"].Value),
                        ManagedPipelineMode = Convert.ToInt32(entry.Properties["ManagedPipelineMode"].Value),
                        ManagedRuntimeVersion = entry.Properties["ManagedRuntimeVersion"].Value.ToString()
                    });
                }
                return list;
            }
            public Boolean DeleteAppPool(String appPool)
            {
                Boolean flag = false;
                if (String.IsNullOrEmpty(appPool)) return flag;
                DirectoryEntry de = new DirectoryEntry(String.Format("IIS://{0}/W3SVC/AppPools", _hostName));
                foreach (DirectoryEntry entry in de.Children)
                {
                    if (entry.Name.Equals(appPool, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            entry.DeleteTree();
                            flag = true;
                        }
                        catch
                        {
                            flag = false;
                        }
                    }
                }
                return flag;
            }
            /// <summary>  
            /// 判断程序池是否存在  
            /// </summary>  
            /// <param name="AppPoolName">程序池名称</param>  
            /// <returns>true存在 false不存在</returns>  
            public Boolean IsAppPoolName(String AppPoolName)
            {
                Boolean result = false;
                DirectoryEntry appPools = new DirectoryEntry(String.Format("IIS://{0}/W3SVC/AppPools", _hostName));
                foreach (DirectoryEntry getdir in appPools.Children)
                {
                    if (getdir.Name.Equals(AppPoolName))
                    {
                        result = true;
                        return result;
                    }
                }
                return result;
            }

            public Boolean CreateAppPool(String appPoolName, String appPoolCommand, String appPoolState,
                String managedPipelineMode, String managedRuntimeVersion,
                String appPoolIdentityType, String Username, String Password)
            {
                Boolean issucess = false;
                try
                {
                    //创建一个新程序池  
                    DirectoryEntry apppools = new DirectoryEntry("IIS://" + _hostName + "/W3SVC/AppPools");
                    DirectoryEntry newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");

                    //设置属性 访问用户名和密码 一般采取默认方式  
                    newpool.Properties["WAMUserName"][0] = Username;
                    newpool.Properties["WAMUserPass"][0] = Password;

                    //newpool.Properties["AppPoolIdentityType"].Value = "4"; //这个默认,不晓得是什么参数  
                    newpool.Properties["AppPoolCommand"].Value = appPoolCommand;
                    newpool.Properties["AppPoolState"].Value = appPoolState;
                    newpool.Properties["ManagedPipelineMode"].Value = managedPipelineMode;
                    newpool.Properties["ManagedRuntimeVersion"].Value = managedRuntimeVersion;

                    newpool.CommitChanges();
                    issucess = true;
                    return issucess;
                }
                catch // (Exception ex)   
                {
                    return false;
                }
            }

            public Boolean UpdateAppPool(String appPoolName, String appPoolCommand,
                String appPoolState, String managedPipelineMode, String managedRuntimeVersion,
                String Username, String Password)
            {
                Boolean issucess = false;
                try
                {
                    DirectoryEntry appPoolEntry = new DirectoryEntry(String.Format("IIS://{0}/W3SVC/AppPools", _hostName));
                    foreach (DirectoryEntry entry in appPoolEntry.Children)
                    {
                        if (entry.Name.Equals(appPoolName, StringComparison.OrdinalIgnoreCase))
                        {
                            // entry.Properties["AppPoolIdentityType"].Value = appPoolIdentityType;  
                            entry.Properties["AppPoolCommand"].Value = appPoolCommand;
                            entry.Properties["AppPoolState"].Value = appPoolState;
                            entry.Properties["ManagedPipelineMode"].Value = managedPipelineMode;
                            entry.Properties["ManagedRuntimeVersion"].Value = managedRuntimeVersion;
                            entry.CommitChanges();
                            issucess = true;
                            return issucess;
                        }
                    }
                }
                catch // (Exception ex)   
                {
                }
                return issucess;
            }


            /// <summary>  
            /// 建立程序池后关联相应应用程序  
            /// </summary>  
            public void SetAppToPool(String appname, String poolName)
            {
                DirectoryEntry children = this.GetWebEntry(appname);
                foreach (DirectoryEntry childrenRoot in children.Children)
                {
                    if (childrenRoot.SchemaClassName.Equals("IIsWebVirtualDir", StringComparison.OrdinalIgnoreCase))
                    {
                        childrenRoot.Properties["AppPoolId"].Value = poolName;
                        childrenRoot.CommitChanges();
                        return;
                    }
                }
            }
        }

        /// <summary>  
        /// IIS应用程序池  
        /// </summary>  
        public class IISAppPoolInfo
        {
            /// <summary>  
            /// 应用程序池名称  
            /// </summary>  
            public String AppPoolName { set; get; }

            public String AppPoolIdentityType { get; set; }

            private Int32 _AppPoolState = 2;
            /// <summary>  
            /// 是否启动:2:启动,4:停止,XX:回收  
            /// </summary>  
            public Int32 AppPoolState
            {
                get { return _AppPoolState; }
                set { _AppPoolState = value; }
            }
            private Int32 _AppPoolCommand = 1;
            /// <summary>  
            /// 立即启动应用程序池.1:以勾选,2:未勾选  
            /// </summary>  
            public Int32 AppPoolCommand
            {
                get { return _AppPoolCommand; }
                set { _AppPoolCommand = value; }
            }
            /// <summary>  
            /// .NET Framework版本.  
            /// "":无托管代码,V2.0: .NET Framework V2.0XXXX,V4.0: .NET Framework V4.0XXXX  
            /// </summary>  
            public String ManagedRuntimeVersion { get; set; }

            private Int32 _ManagedPipelineMode = 0;
            /// <summary>  
            /// 托管管道模式.0:集成,1:经典  
            /// </summary>  
            public Int32 ManagedPipelineMode
            {
                get { return _ManagedPipelineMode; }
                set { _ManagedPipelineMode = value; }
            }
        }
        public sealed class IISWebManager
        {
            /// <summary>  
            /// 网站名称ID  
            /// </summary>  
            public Int32 Name { get; set; }
            /// <summary>  
            /// 网名名称  
            /// </summary>  
            public String ServerComment { get; set; }
            /// <summary>  
            /// 应用程序池  
            /// </summary>  
            public String AppPoolId { set; get; }
            /// <summary>  
            /// 物理路径  
            /// </summary>  
            public String PhysicalPath { set; get; }
            /// <summary>  
            /// 绑定类型  
            /// </summary>  
            public String DomainType { set; get; }
            /// <summary>  
            /// 绑定IP  
            /// </summary>  
            public String DomainIP { set; get; }

            /// <summary>  
            /// 绑定端口  
            /// </summary>  
            public Int32 DomainPort { set; get; }
            /// <summary>  
            /// 是否启动:2:启动,4:停止  
            /// </summary>  
            public String ServerState { set; get; }
            /// <summary>  
            /// 服务命令  
            /// </summary>  
            public String ServerCommand { get; set; }

        }
            */

        /// <summary>
        /// 获取所有的站点信息。信息格式为 Tuple<Name, SchemaClassName, ServerComment>
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, string, string>> GetAllWebSites()
        {
            return GetAllSites(IISEntry);
        }
        /// <summary>
        /// 获取所有的站点信息。信息格式为 Tuple<Name, SchemaClassName, ServerComment>
        /// </summary>
        /// <param name="iisMetabasePath">metabasePath is of the form "IIS://<servername>/<path>"
        //    for example "IIS://localhost/W3SVC/1/Root/MyVDir" 
        //    or "IIS://localhost/W3SVC/AppPools/MyAppPool"</param>
        /// <returns></returns>
        public static List<Tuple<string, string, string>> GetAllWebSites(string iisMetabasePath)
        {
            return GetAllSites(new DirectoryEntry(iisMetabasePath));
        }
        /// <summary>
        /// 获取所有的站点信息。信息格式为 Tuple<Name, SchemaClassName, ServerComment>
        /// </summary>
        /// <param name="iisEntry"></param>
        /// <returns></returns>
        public static List<Tuple<string, string, string>> GetAllSites(DirectoryEntry iisEntry)
        {
            var allSiteInfos = new List<Tuple<string,string,string>>();

            foreach (DirectoryEntry childEntry in iisEntry.Children)
            {
                switch (childEntry.SchemaClassName)
                {
                    case "IIsWebServer": // IIS网站的类型，其Name全部都是数字形式的Id；ServerComment为网站的名称
                        // IIsWebServer 的 ServerComment 不会为空，其他类型有可能为null。
                        allSiteInfos.Add(Tuple.Create(childEntry.Name, childEntry.SchemaClassName, childEntry.Properties["ServerComment"].Value?.ToString()));
                        break;
                    case "IIsWebVirtualDir": // IIS虚拟目录的类型
                        allSiteInfos.Add(Tuple.Create(childEntry.Name, childEntry.SchemaClassName, childEntry.Properties["ServerComment"].Value?.ToString()));
                        break;
                    //case "IIsApplicationPools": // IIS应用程序池s

                    //    allSiteInfos.Add(Tuple.Create(childEntry.Name, childEntry.SchemaClassName, childEntry.Properties["ServerComment"].Value?.ToString()));
                    //    break;
                    //case "IIsApplicationPool": // IIS应用程序池s

                    //    allSiteInfos.Add(Tuple.Create(childEntry.Name, childEntry.SchemaClassName, childEntry.Properties["ServerComment"].Value?.ToString()));
                    //    break;
                    default:
                        // 没有 "Application" 类型
   
                        // 其他类型
                        Debug.Print(childEntry.SchemaClassName);
                        break;
                }

                allSiteInfos.AddRange(GetAllSites(childEntry));
            }
            return allSiteInfos;
        }

        /// <summary>
        /// 获取 IIS DirectoryEntry 的所有属性名和值
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> EnumerateProperties()
        {
            try
            {
                var dict=new Dictionary<string, string>();
                foreach (string propName in IISEntry.Properties.PropertyNames)
                {
                    Console.Write(" {0} =", propName);
                    dict.Add(propName, IISEntry.Properties[propName].Value?.ToString());
                }
                return dict;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in EnumeratePath with the following exception: \n{0}", ex.Message);
                throw ex;
            }
        }

 
        /// <summary>
        /// 获取所有的站点信息 嵌套类型的SiteInfo信息【树形】
        /// </summary>
        /// <returns></returns>
        public List<SiteInfo> GetSiteList()
        {
            return GetSiteList(IISEntry);
        }
        /// <summary>
        /// 获取所有的站点信息 获取嵌套类型的SiteInfo信息【树形】
        /// </summary>
        /// <returns></returns>
        public static List<SiteInfo> GetSiteList(DirectoryEntry iisEntry)
        {
            var entry = iisEntry;

            var result = new List<SiteInfo>();
            foreach (DirectoryEntry childEntry in entry.Children)
            {
                var sites = GetSiteList(childEntry);
                if (childEntry.SchemaClassName == "IIsWebServer")
                {
                    var site = new SiteInfo();
                    site.Id = childEntry.Name;
                    site.Name = childEntry.Properties["ServerComment"].Value.ToString();
                    site.Path = sites[0].Path;
                    site.IsApp = true;
                    site.Children = new List<SiteInfo>();
                    foreach (var subSite in sites[0].Children)
                        site.Children.Add(subSite);

                    if (!(childEntry.Properties["ServerBindings"].Value is null))
                    {
                        if (childEntry.Properties["ServerBindings"].Value is object[] serverBindings)
                        {
                            site.ServerBindings = serverBindings.Select(b => b.ToString()).ToArray();
                        }
                        else
                        {
                            site.ServerBindings = new string[] { childEntry.Properties["ServerBindings"].Value.ToString() };
                        }

                        site.ServerBindingDatas = new List<ServerBindingData>();
                        foreach (var serverBinding in site.ServerBindings)
                        {
                            var ip_port_hostname = serverBinding.Split(':');

                            site.ServerBindingDatas.Add(new ServerBindingData()
                            {
                                Ip = ip_port_hostname[0],
                                Port = ip_port_hostname[1],
                                HostName = ip_port_hostname[2]
                            });
                        }
                    }
                    
                    result.Add(site);
                }
                else if (childEntry.SchemaClassName == "IIsWebVirtualDir")
                {
                    var site = new SiteInfo();
                    //site.Id = childEntry.Name;
                    site.Name = childEntry.Name;
                    site.Path = childEntry.Properties["Path"].Value.ToString();
                    site.Children = sites;
                    if (childEntry.Properties.Contains("AppRoot")
                        && childEntry.Properties["AppRoot"].Value != null
                        && !string.IsNullOrEmpty(childEntry.Properties["AppRoot"].Value.ToString()))
                        site.IsApp = true;

                    result.Add(site);
                }
            }
            return result;
        }
        /// <summary>
        /// 将嵌套树形格式的 List<SiteInfo> 转换为文本格式
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="parentPadding"></param>
        /// <returns></returns>
        public static List<KeyValuePair<SiteInfo, string>> GetFlatSiteList(List<SiteInfo> sites, string parentPadding = "")
        {
            var result = new List<KeyValuePair<SiteInfo, string>>();
            foreach (var site in sites)
            {
                var currentPrefix =  string.IsNullOrEmpty(parentPadding) ? string.Empty : "└" + parentPadding;
                result.Add(new KeyValuePair<SiteInfo, string>(site, currentPrefix + site.Name));
                result.AddRange(GetFlatSiteList(site.Children, parentPadding + "--"));
            }
            return result;
        }
        private enum IISState
        {
            Start,
            Stop,
            Reset
        }

    }

}
