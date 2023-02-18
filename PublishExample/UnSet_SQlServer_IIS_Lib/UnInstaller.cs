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
    }
}
