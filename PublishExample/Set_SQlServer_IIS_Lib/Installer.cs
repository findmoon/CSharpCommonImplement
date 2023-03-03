using HelperCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Set_SQlServer_IIS_Lib
{
    /// <summary>
    /// 组件不会显示出来
    /// </summary>
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        public override async void Install(IDictionary stateSever)
        {
            base.Install(stateSever);

            MessageBox.Show("测试");

            //接收 CustomActionData 传入的参数

            //数据库服务器地址
            string sqlServer_IPInstance = Context.Parameters["sqlServerInstance"];
            //账号
            string sqlServer_user = Context.Parameters["sqlServerUser"];
            //密码
            string sqlServer_pwd = Context.Parameters["sqlServerPwd"];

            string sqlServer_portTmp = Context.Parameters["sqlServerPort"];
            ushort? sqlServer_port = null;
            if (ushort.TryParse(sqlServer_portTmp, out ushort sqlPort))
            {
                sqlServer_port = sqlPort;
            }


            //IIS服务器地址【通常应该为本地地址，如果为远程IIS地址，可能还需要用户名密码】
            string siteHost = this.Context.Parameters["stieHost"];
            //ip
            string siteIp = this.Context.Parameters["siteIp"];
            //端口
            string sitePortTmp = this.Context.Parameters["sitePort"];
            ushort sitePort = 80;
            if (ushort.TryParse(sitePortTmp,out ushort currSitePort))
            {
                sitePort= currSitePort;
            }
            //网站名
            string iisSiteName = this.Context.Parameters["iisSiteName"];
            //是否使用默认应用程序池
            var useDefaultAppPool = !string.IsNullOrEmpty(Context.Parameters["useDefaultAppPool"]);

            //安装路径
            string targetdir = Context.Parameters["targetdir"].Replace(@"\\", @"\"); // .TrimEnd('\\')竟然对结尾多余的\\无效

            // 制作商、产品名
            string manufacturer = Context.Parameters["manufacturer"];
            string productName = Context.Parameters["productName"];

            #region 连接附加数据库

            // 连接附加数据库
            var attDBName=await AttachDBHandle(targetdir,sqlServer_IPInstance, sqlServer_user, sqlServer_pwd, sqlServer_port);


            var dbName = attDBName ?? productName;
            // 更新Web.config的连接字符串
            string webconfigFile = Path.Combine(targetdir, "Web.config");
            if (File.Exists(webconfigFile))
            {
                MessageBox.Show("存在");
            }
            else
            {
                throw new Exception("不存在");
            }
            var xml = new XmlDocument();
            xml.Load(webconfigFile);
            var defaultConnNode = xml.SelectSingleNode("/configuration/connectionStrings/add[name=\"DefaultConnection\"]");
            var port = "";
            if (sqlServer_port != null)
            {
                port = "," + sqlServer_port;
            }
            defaultConnNode.Attributes["connectionString"].Value = $"Server={sqlServer_IPInstance}{port};Database={dbName};User Id={sqlServer_user};Password={sqlServer_pwd};";

            xml.Save(webconfigFile);
            #endregion

            #region 检测并安装IIS

            #endregion

            #region 创建IIS Web网站
            var iisHelper = new IISSiteHelper_MWA();
            iisHelper.CreateUpdateWebSite(iisSiteName??productName, targetdir, siteIp, sitePort,siteHost, useDefaultAppPool?"": productName);
            #endregion

            #region vbs打开链接、文件脚本
            File.WriteAllLines(Path.Combine(targetdir,"open.vbs"),new string[]
            {
                "Set objShell = CreateObject(\"Wscript.Shell\")",
                $"objShell.Run(\"http://localhost:{port}\")"
            });
            #endregion

            #region 添加url快捷方式
            var urlLinkFile = GetStartMenuUrlLinkFile();

            File.WriteAllLines(urlLinkFile, new string[] {
                "[InternetShortcut]",
                $"URL=http://localhost:{port}",
                "IconIndex=0",
                $"IconFile="+Path.Combine(targetdir,"favicon.ico")
            });
            #endregion
        }

        /// <summary>
        /// 连接附加数据库的 处理
        /// </summary>
        /// <param name="sqlServer_IPInstance"></param>
        /// <param name="sqlServer_user"></param>
        /// <param name="sqlServer_pwd"></param>
        /// <param name="sqlServer_port"></param>
        /// <param name="targetdir"></param>
        /// <returns>返回新附加的dbName</returns>
        private static async Task<string> AttachDBHandle(string targetdir,string sqlServer_IPInstance, string sqlServer_user, string sqlServer_pwd, ushort? sqlServer_port=null)
        {
            #region 附加前设置文件权限是否必要？
            //给文件添加"Authenticated Users,Everyone,Users"用户组的完全控制权限 ，要附加的数据库文件必须加权限否则无法附加
            //if (File.Exists(Context.Parameters["targetdir"].ToString() + "App_Data\\ceshi.mdf"))
            //{
            //    FileInfo fi = new FileInfo(Context.Parameters["targetdir"].ToString() + "App_Data\\ceshi.mdf");
            //    System.Security.AccessControl.FileSecurity fileSecurity = fi.GetAccessControl();
            //    fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
            //    fileSecurity.AddAccessRule(new FileSystemAccessRule("Authenticated Users", FileSystemRights.FullControl, AccessControlType.Allow));
            //    fileSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
            //    fi.SetAccessControl(fileSecurity);
            //    FileInfo fi1 = new FileInfo(Context.Parameters["targetdir"].ToString() + "App_Data\\ceshi.ldf");
            //    System.Security.AccessControl.FileSecurity fileSecurity1 = fi1.GetAccessControl();
            //    fileSecurity1.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
            //    fileSecurity1.AddAccessRule(new FileSystemAccessRule("Authenticated Users", FileSystemRights.FullControl, AccessControlType.Allow));
            //    fileSecurity1.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
            //    fi1.SetAccessControl(fileSecurity1);
            //} 
            #endregion

            using (var sqlHelper = SQLServerHelper.Init(sqlServer_IPInstance, sqlServer_user, sqlServer_pwd, "master", sqlServer_port))
            {
                var app_DataDir = Path.Combine(targetdir, "App_Data");
                var mdf_Files = Directory.GetFiles(app_DataDir, "*.mdf");
                var ldf_Files = Directory.GetFiles(app_DataDir, "*.ldf");
                string mdf_File = null;
                string ldf_File = null;
                // 一个mdf
                if (mdf_Files.Length > 0)
                {
                    mdf_File = mdf_Files[0];
                }
                else
                {
                    return null;
                }
                if (ldf_Files.Length > 0)
                {
                    ldf_File = ldf_Files[0];
                }
                // 附加处理时的DbName
                var dbName = Path.GetFileNameWithoutExtension(mdf_File);
                var dbExists = await sqlHelper.ExistsDBOrTableOrColAsync(dbName, null);
                if (!dbExists)
                {
                    var dataPath = await sqlHelper.DefaultDataPathAsync();

                    // 复制文件 [存在将失败]
                    var last_mdf_File = Path.Combine(dataPath, Path.GetFileName(mdf_File));
                    var last_ldf_File = Path.Combine(dataPath, Path.GetFileName(ldf_File));
                    File.Copy(mdf_File, last_mdf_File);
                    File.Copy(ldf_File, last_ldf_File);

                    await sqlHelper.AttachDBAsync(dbName, last_mdf_File, last_ldf_File);
                    await sqlHelper.ChangeOwnerAsync(dbName, sqlServer_user);
                }

                return dbName;
            }
        }

        /// <summary>
        /// 获取开始菜单中 Url快捷方式的文件 完全路径。
        /// </summary>
        /// <returns></returns>
        private string GetStartMenuUrlLinkFile()
        {
            string manufacturer = Context.Parameters["manufacturer"];
            string productName = Context.Parameters["productName"];
            // 获取偶尔竟然有问题
            //var productName = Path.GetFileName(targetdir.TrimEnd('\\'));
            //var manufacturer = Path.GetFileName(targetdir.Substring(0, targetdir.IndexOf(productName)).TrimEnd('\\'));

            // 开始菜单中创建url快捷方式
            var urlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), manufacturer);

            if (!Directory.Exists(urlPath))
            {
                Directory.CreateDirectory(urlPath);
            }
            return Path.Combine(urlPath, productName + ".url");
        }
    }
}
