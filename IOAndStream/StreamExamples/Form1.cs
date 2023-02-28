using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamExamples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileName = "streamWriterFile.txt"; 
            //var fileName = "streamWriterFile.url"; // shortcut
            // 生成程序的快捷方式内容
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                //string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
            }
        }
    }
}
