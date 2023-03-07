using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace UnSet_SQlServer_IIS_Lib
{
    [RunInstaller(true)]
    public partial class UnInstaller : System.Configuration.Install.Installer
    {
        public UnInstaller()
        {
            InitializeComponent();
        }

        /// <summary>
         /// 卸载处理
         /// </summary>
         /// <param name="savedState"></param>
         public override void Uninstall(IDictionary savedState)
         {
             base.Uninstall(savedState);

            // 卸载的额外处理
            // 卸载中无法使用 Context.Parameters 等环境参数【未确认】
        }
    }
}
