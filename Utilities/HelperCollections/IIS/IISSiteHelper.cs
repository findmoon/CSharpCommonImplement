//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security.AccessControl;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using System.Data.SqlClient;
//using System.Management;
//using System.DirectoryServices;
//using Microsoft.Web.Administration;

///* 
// * using System.DirectoryServices  ------ 引用： System.DirectoryServices.dll
// * using Microsoft.Web.Administration  [IIS 7.0 + 管理的托管API]------ 引用： Microsoft.Web.Administration.dll
// */
//namespace Set_SQlServer_IIS_Lib
//{
//    /// <summary>
//    /// IIS站点配置信息帮助类。严格来说只是IIS中的Web
//    /// </summary>
//    public class IISSiteHelper
//    {
//        private string hostIP;   // 主机IP
//        private string port;   // 网站端口号
   
//        private string commentOfWebSite;// 网站注释。一般也为网站的网站名。
//        private string webPath;   // 网站的主目录。例如"e:\ mp"

//        /// <summary>
//        /// 实例化IIS站点配置
//        /// </summary>
//        /// <param name="iisServer">iis服务器地址，通常为本地的IIS，即localhost</param>
//        /// <param name="hostIP">主机IP</param>
//        /// <param name="port">网站端口号</param>
//        /// <param name="hostName">网站域名。一般为网站的网站名。例如"www.dns.com.cn"--【主机名(域名)】</param>
//        /// <param name="commentOfWebSite">网站注释。一般也为网站的网站名。--【iis网站站点名称】</param>
//        /// <param name="webPath">网站的主目录。例如"e:\ mp"</param>
//        public IISSiteHelper(string iisServer,string hostIP, string port, string hostName, string commentOfWebSite, string webPath)
//        {
//            this.iisServer = iisServer;
//            this.hostIP = hostIP;
//            this.port = port;
//            this.descOfWebSite = hostName;
//            this.commentOfWebSite = commentOfWebSite;
//            this.webPath = webPath;
//        }

//        /// <summary>
//        /// 网站标识
//        /// </summary>
//        public string BindString
//        {
//            get
//            {
//                return String.Format("{0}:{1}:{2}", hostIP, port, descOfWebSite); //网站标识（IP,端口，主机头值）
//            }
//        }
//        /// <summary>
//        /// 网站端口号
//        /// </summary>
//        public string PortNum
//        {
//            get
//            {
//                return port;
//            }
//        }
//        /// <summary>
//        /// 网站表示。一般为网站的网站名。例如"www.dns.com.cn"
//        /// </summary>
//        public string CommentOfWebSite
//        {
//            get
//            {
//                return commentOfWebSite;
//            }
//        }
//        /// <summary>
//        /// 网站的主目录。例如"e:\ mp"
//        /// </summary>
//        public string WebPath
//        {
//            get
//            {
//                return webPath;
//            }
//        }

//        /// <summary>
//        /// 创建IIS站点
//        /// </summary>
//        /// <param name="siteInfo">新站点配置信息</param>
//        public void CreateNewWebSite()
//        {
//            string entPath = String.Format("IIS://{0}/W3SVC", iis);

//            //SetFileRole();

//            if (!EnsureNewSiteEnavaible(siteInfo.BindString, entPath))
//            {
//                throw new Exception("该网站已存在" + Environment.NewLine + siteInfo.BindString);
//            }

//            DirectoryEntry rootEntry = new DirectoryEntry(entPath);

//            string newSiteNum = GetNewWebSiteID(entPath);

//            DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum, "IIsWebServer");
//            newSiteEntry.CommitChanges();

//            newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
//            newSiteEntry.Properties["ServerComment"].Value = siteInfo.CommentOfWebSite;
//            newSiteEntry.Properties["ServerAutoStart"].Value = true;//网站是否启动
//            newSiteEntry.CommitChanges();

//            DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
//            vdEntry.CommitChanges();
//            string ChangWebPath = siteInfo.WebPath.Trim().Remove(siteInfo.WebPath.Trim().LastIndexOf('\\'), 1);
//            vdEntry.Properties["Path"].Value = ChangWebPath;


//            vdEntry.Invoke("AppCreate", true);//创建应用程序
//            //vdEntry.Properties["ServerAutoStart"].Value = true;//网站是否启动
//            vdEntry.Properties["AccessRead"][0] = true; //设置读取权限
//            vdEntry.Properties["AccessWrite"][0] = true;
//            vdEntry.Properties["AccessScript"][0] = true;//执行权限
//            vdEntry.Properties["AccessExecute"][0] = false;
//            vdEntry.Properties["DefaultDoc"][0] = "Login_gdzc.aspx";//设置默认文档
//            vdEntry.Properties["AppFriendlyName"][0] = "LabManager"; //应用程序名称           
//            vdEntry.Properties["AuthFlags"][0] = 1;//0表示不允许匿名访问,1表示就可以3为基本身份验证，7为windows继承身份验证
//            vdEntry.CommitChanges();

//            //操作增加MIME
//            //IISOle.MimeMapClass NewMime = new IISOle.MimeMapClass();
//            //NewMime.Extension = ".xaml"; NewMime.MimeType = "application/xaml+xml";
//            //IISOle.MimeMapClass TwoMime = new IISOle.MimeMapClass();
//            //TwoMime.Extension = ".xap"; TwoMime.MimeType = "application/x-silverlight-app";
//            //rootEntry.Properties["MimeMap"].Add(NewMime);
//            //rootEntry.Properties["MimeMap"].Add(TwoMime);
//            //rootEntry.CommitChanges();

//            #region 针对IIS7
//            DirectoryEntry getEntity = new DirectoryEntry("IIS://" + iis + "/W3SVC/INFO");
//            int Version = int.Parse(getEntity.Properties["MajorIISVersionNumber"].Value.ToString());
//            if (Version > 6)
//            {
//                #region 创建应用程序池
//                string AppPoolName = "LabManager";
//                if (!IsAppPoolName(AppPoolName))
//                {
//                    DirectoryEntry newpool;
//                    DirectoryEntry appPools = new DirectoryEntry("IIS://" + iis + "/W3SVC/AppPools");
//                    newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
//                    newpool.CommitChanges();
//                }
//                #endregion

//                #region 修改应用程序的配置(包含托管模式及其NET运行版本)
//                ServerManager sm = new ServerManager();
//                sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "v4.0";
//                sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Classic; //托管模式Integrated为集成 Classic为经典
//                sm.CommitChanges();
//                #endregion

//                vdEntry.Properties["AppPoolId"].Value = AppPoolName;
//                vdEntry.CommitChanges();
//            }

//            #endregion


//            //启动aspnet_regiis.exe程序 
//            string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
//            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
//            //处理目录路径 
//            string path = vdEntry.Path.ToUpper();
//            int index = path.IndexOf("W3SVC");
//            path = path.Remove(0, index);
//            //启动ASPnet_iis.exe程序,刷新脚本映射 
//            startInfo.Arguments = "-s " + path;
//            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
//            startInfo.UseShellExecute = false;
//            startInfo.CreateNoWindow = true;
//            startInfo.RedirectStandardOutput = true;
//            startInfo.RedirectStandardError = true;
//            Process process = new Process();
//            process.StartInfo = startInfo;
//            process.Start();
//            process.WaitForExit();
//            string errors = process.StandardError.ReadToEnd();

//            if (errors != string.Empty)
//            {
//                throw new Exception(errors);
//            }

//        }

//        #region 判定网站是否存在

//        /// <summary>
//        /// 确定一个新的网站与现有的网站没有相同的。 
//        /// 这样防止将非法的数据存放到IIS里面去 
//        /// </summary>
//        /// <param name="bindStr">网站邦定信息</param>
//        /// <returns>真为可以创建，假为不可以创建</returns>
//        public bool EnsureNewSiteEnavaible(string bindStr, string entPath)
//        {
//            DirectoryEntry ent = new DirectoryEntry(entPath);

//            foreach (DirectoryEntry child in ent.Children)
//            {
//                if (child.SchemaClassName == "IIsWebServer" && child.Properties["ServerBindings"].Value != null && child.Properties["ServerBindings"].Value.ToString() == bindStr)
//                {
//                    return false;
//                }
//            }
//            return true;
//        }

//        /// <summary>
//        /// 设置文件夹权限 处理给EVERONE赋予所有权限
//        /// </summary>
//        /// <param name="FileAdd">文件夹路径</param>
//        public void SetFileRole()
//        {
//            string FileAdd = this.Context.Parameters["targetdir"].ToString();
//            FileAdd = FileAdd.Remove(FileAdd.LastIndexOf('\\'), 1);
//            DirectorySecurity fSec = new DirectorySecurity();
//            fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
//            System.IO.Directory.SetAccessControl(FileAdd, fSec);
//        }

//        #endregion
//    }
//}
