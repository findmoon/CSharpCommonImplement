using CMCode.Call;
using CMCode.ProcessExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExecAppTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //var result= await ExecApp.StartExecutAsync(@"C:\WINDOWS\system32\cmd.exe");
            var result = await ExecApp.StartExecutAsync(@"C:\WINDOWS\system32\cmd.exe", waitComplete: true, getOuput: true);
            MessageBox.Show($"{result.Output}");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Process.Start("www.bing.com");

            //Process.Start("my.png");

            //Process.Start(@"C:\WINDOWS\system32\cmd.exe");

            #region 作为参数，无法执行命令
            //Process.Start(@"C:\WINDOWS\system32\cmd.exe", "/K ping 127.0.0.1");

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/K ping 127.0.0.1";
            process.StartInfo = startInfo;
            process.Start();

            //Process.Start("CMD.exe", "ping 127.0.0.1"); 
            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Windows\Notepad.exe";// process.StartInfo.FileName = "Notepad";
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            //Exited事件处理代码，外部程序退出后激活，可以执行相关操作
            MessageBox.Show("Notepad.exe运行完毕");
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Process parentProcess = Process.GetCurrentProcess().GetParent();
            Process parentProcess2 = Process.GetCurrentProcess().GetParent2();
            var parentProcessId = ProcessUtilities.ParentProcessId( Process.GetCurrentProcess().Id);

            var c = Environment.ProcessorCount;
        }


        private void button11_Click(object sender, EventArgs e)
        {
            var parentProcess = ParentProcessUtilities.GetParentProcess();
            var a = parentProcess.Id;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var parentProcessId = Process.GetCurrentProcess().ParentProcessId_PInvoke();
            var a = parentProcessId;
        }

    }
}
