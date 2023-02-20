using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections.IIS.Model
{
    public class SiteInfo
    {
        public string Id { get; set; }
        /// <summary>
        /// 如果是IIS网站，Properties["ServerComment"]为其名称
        /// </summary>
        public string Name { get; set; }
        public string Path { get; set; }
        public string[] ServerBindings { get; set; }
        public List<ServerBindingData> ServerBindingDatas { get; set; }
        public bool IsApp { get; set; }
        public List<SiteInfo> Children { get; set; }
    }

    public class ServerBindingData {
        public string HostName { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        //public string Type { get; set; }
    }
}
