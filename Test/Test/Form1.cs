using HelperCollections;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;


namespace MiscellaneousTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer1.Tick += Timer1_Tick;

        }


        private void Timer1_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine($"秒：{DateTime.Now.Second}");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //        new ToastContentBuilder()
            //.AddArgument("action", "viewConversation")
            //.AddArgument("conversationId", 9813)
            //.AddText("Andrew sent you a picture")
            //.AddText("Check this out, The Enchantments in Washington!")
            //.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater

            var dt = new DataTable();
            //dt.Clone();// 克隆结构
            //dt.Select(
        
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //unsafe 
            //{
            //    var num = 10;
            //    int* pNum = &num;
            //    Debug.WriteLine();
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string @int = "你好";
            int @string = 10;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 多次启动运行，并不会多次调用。
            // 两者等同，启动一个即可。
            //timer1.Start();
            timer1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //FileDirRightHelper.SetDirRights(null);
            FileDirRightHelper.SetDirRights(@"D:\SoftWareDevelope\CSharp\csharp-common-implement");

            MessageBox.Show("结束");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var targetdir = @"D:\SoftWareDevelope\CSharp\csharp-common-implement\Web\ASPNETMVC_UserAutho\App_Data\";
            var app_DataDir = Path.Combine(targetdir, "App_Data");
            var mdf_Files = Directory.GetFiles(app_DataDir, "*.mdf");
            var ldf_Files = Directory.GetFiles(app_DataDir, "*.ldf");
            if (mdf_Files.Length>0)
            {
                
            }
            if (ldf_Files.Length>0)
            {
                
            }

            MessageBox.Show("结束");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var webconfigFile = @"D:\SoftWareDevelope\CSharp\csharp-common-implement\Web\ASPNETMVC_UserAutho\bin\ASPNETMVC_UserAutho_Web\Web.config";
            var xml = new XmlDocument();
            xml.Load(webconfigFile);
            //var configurationNode = xml.SelectSingleNode("/configuration");
            //var connectionStringsNode = xml.SelectSingleNode("/configuration/connectionStrings");
            //var addNode = xml.SelectSingleNode("/configuration/connectionStrings/add");
            var defaultConnNode = xml.SelectSingleNode("/configuration/connectionStrings/add[@name=\"DefaultConnection\"]");
            if (defaultConnNode == null)
            {
                MessageBox.Show("A");
            }
            var port = "";

            var k = defaultConnNode.Attributes["connectionString"].Value;// = $"test";
             defaultConnNode.Attributes["connectionString"].Value = $"test";

            xml.Save(webconfigFile);

            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var t = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // 刷新
            SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);
        }
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            new FileDirIconTest().Show();
        }
    }
}
