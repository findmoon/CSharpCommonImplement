using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set_SQlServer_IIS_Lib
{
    /// <summary>
    /// IIS站点配置信息帮助类
    /// </summary>
    public class IISSiteHelper
    {
        private string hostIP;   // 主机IP
        private string portNum;   // 网站端口号
        private string descOfWebSite; // 网站表示。一般为网站的网站名。例如"www.dns.com.cn"
        private string commentOfWebSite;// 网站注释。一般也为网站的网站名。
        private string webPath;   // 网站的主目录。例如"e:\ mp"

        /// <summary>
        /// 实例化IIS站点配置
        /// </summary>
        /// <param name="hostIP">主机IP</param>
        /// <param name="portNum">网站端口号</param>
        /// <param name="descOfWebSite">网站表示。一般为网站的网站名。例如"www.dns.com.cn"--【主机名(域名)】</param>
        /// <param name="commentOfWebSite">网站注释。一般也为网站的网站名。--【iis网站站点名称】</param>
        /// <param name="webPath">网站的主目录。例如"e:\ mp"</param>
        public IISSiteHelper(string hostIP, string portNum, string descOfWebSite, string commentOfWebSite, string webPath)
        {
            this.hostIP = hostIP;
            this.portNum = portNum;
            this.descOfWebSite = descOfWebSite;
            this.commentOfWebSite = commentOfWebSite;
            this.webPath = webPath;
        }

        /// <summary>
        /// 网站标识
        /// </summary>
        public string BindString
        {
            get
            {
                return String.Format("{0}:{1}:{2}", hostIP, portNum, descOfWebSite); //网站标识（IP,端口，主机头值）
            }
        }
        /// <summary>
        /// 网站端口号
        /// </summary>
        public string PortNum
        {
            get
            {
                return portNum;
            }
        }
        /// <summary>
        /// 网站表示。一般为网站的网站名。例如"www.dns.com.cn"
        /// </summary>
        public string CommentOfWebSite
        {
            get
            {
                return commentOfWebSite;
            }
        }
        /// <summary>
        /// 网站的主目录。例如"e:\ mp"
        /// </summary>
        public string WebPath
        {
            get
            {
                return webPath;
            }
        }
    }
}
