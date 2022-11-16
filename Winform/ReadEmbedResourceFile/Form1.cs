using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadEmbedResourceFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ReadEmbedResourceFile();
        }


        void ReadEmbedResourceFile()
        {
            //获得命名空间
            Type type = MethodBase.GetCurrentMethod().DeclaringType;
            string _namespace = type.Namespace;

            //获得当前运行的Assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            //// 或者使用assembly获取程序集名(项目名、Namespace)
            //string projectName = assembly.GetName().Name;

            //$"{projectName}.Resources.Snipaste_2022-07-12_09-16-29.png";
            //$"{projectName}.Star.png";

            //根据名称空间和文件名生成资源名称   Resources为文件夹路径
            string resourceName1 = $"{_namespace}.Resources.Snipaste_2022-07-12_09-16-29.png";
            string resourceName2 = $"{_namespace}.Star.png";

            //根据资源名称从Assembly中获取此资源的Stream
            // GetManifestResourceStream的资源名称必须是完整路径的资源（包含完整命名空间）
            Stream stream1 = assembly.GetManifestResourceStream(resourceName1);
            Stream stream2 = assembly.GetManifestResourceStream(resourceName2);
            
        }
    }
}
