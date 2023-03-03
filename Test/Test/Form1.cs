using HelperCollections.FileDirRight;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


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
    }
}
