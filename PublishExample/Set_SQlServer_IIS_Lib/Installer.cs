using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public override void Install(IDictionary stateSever)
        {
            base.Install(stateSever);

            //接收 CustomActionData 传入的参数

            //数据库服务器地址
            string sqlServer_IPInstance = Context.Parameters["serverInstance"];
            //账号
            string sqlServer_user = Context.Parameters["user"];
            //密码
            string sqlServer_pwd = Context.Parameters["pwd"];

            //IIS服务器地址【通常应该为本地地址，如果为远程IIS地址，可能还需要用户名密码】
            string iisServer = this.Context.Parameters["iisServer"];
            //ip
            string siteIp = this.Context.Parameters["siteIp"];
            //端口
            string sitePort = this.Context.Parameters["sitePort"];
            //网站名
            string iisSiteName = this.Context.Parameters["iisSiteName"];
            //是否使用默认应用程序池
            var useDefaultAppPool = !string.IsNullOrEmpty(Context.Parameters["useDefaultAppPool"]);

            //安装路径
            string targetdir = Context.Parameters["targetdir"].Replace(@"\\", @"\");


            ////实例化IIS站点配置信息
            //NewWebSiteInfo nwsif = new NewWebSiteInfo(ip, port, isname.Trim(), (isname.Trim().Length > 0 ? isname : "anjiesigudingzichan"), targetdir);
            ////创建IIS站点
            //CreateNewWebSite(nwsif);
        }
    }
}
