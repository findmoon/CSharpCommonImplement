using CMCode.MIWMIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMIAndMIUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(Environment.NewLine, GetCPUSerialNumber()));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WMIUtilities.GetBIOSInfo();
            MessageBox.Show(WMIUtilities.GetBIOSSerialNumber());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(Environment.NewLine, WMIUtilities.GetHardDiskSerialNumber()));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(Environment.NewLine, WMIUtilities.GetNetCardMACAddress()));

        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(Environment.NewLine, WMIUtilities.GetComInfo()));
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Process process = Process.GetCurrentProcess();
            //当前进程的id
            Console.WriteLine(process.Id);
            //获取关联的进程的终端服务会话标识符。
            Console.WriteLine(process.SessionId);
            //当前进程的名称
            Console.WriteLine(process.ProcessName);
            //当前进程的启动时间
            Console.WriteLine(process.StartTime);
            //获取关联进程终止时指定的值,在退出事件中使用
            //Console.WriteLine(cur.ExitCode);
            //获取进程的当前机器名称 .代表本地
            Console.WriteLine(process.MachineName);
            //获取进程的主窗口标题。
            Console.WriteLine(process.MainWindowTitle);
            //当前进程的主模块 一般为程序名
            Console.WriteLine(process.MainModule);

        }


        private void button13_Click(object sender, EventArgs e)
        {

        }

        #region 一些进程方法 https://www.c-sharpcorner.com/article/finding-and-listing-processes-in-C-Sharp/
        /// <summary>
        /// Returns a string containing information on running processes
        /// </summary>
        /// <returns></returns>
        public static string ListAllApplications()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Process p in Process.GetProcesses("."))
            {
               
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        sb.Append("Window Title:\t" + p.MainWindowTitle.ToString() + Environment.NewLine);
                        sb.Append("Process Name:\t" + p.ProcessName.ToString() + Environment.NewLine);
                        sb.Append("Window Handle:\t" + p.MainWindowHandle.ToString() + Environment.NewLine);
                        sb.Append("Memory Allocation:\t" + p.PrivateMemorySize64.ToString() + Environment.NewLine);
                        sb.Append(Environment.NewLine);
                    }
                }
                catch { }
            }
            return sb.ToString();
        }
        /// <summary>
        /// List all processes by image name
        /// </summary>
        /// <returns></returns>
        [Obsolete("几乎是一个错误方法", true)]
        public static string ListAllByImageName()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Process p in Process.GetProcesses("."))
            {
                try
                {
                    foreach (ProcessModule pm in p.Modules)
                    {
                        sb.Append("Image Name:\t" + pm.ModuleName.ToString() + Environment.NewLine);
                        sb.Append("File Path:\t\t" + pm.FileName.ToString() + Environment.NewLine);
                        sb.Append("Memory Size:\t" + pm.ModuleMemorySize.ToString() + Environment.NewLine);
                        sb.Append("Version:\t\t" + pm.FileVersionInfo.FileVersion.ToString() + Environment.NewLine);
                        sb.Append(Environment.NewLine);
                    }
                }
                catch { }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Determine if a process is running by image name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool CheckForProcessByImageName(string processImageName)
        {
            bool rtnVal = false;
            foreach (Process p in Process.GetProcesses("."))
            {
                try
                {
                    foreach (ProcessModule pm in p.Modules)
                    {
                        if (pm.ModuleName.ToLower() == processImageName.ToLower())
                            rtnVal = true;
                    }
                }
                catch { }
            }
            return rtnVal;
        }

        /// <summary>
        /// Determine if an application is running by name
        /// </summary>
        /// <param name="AppName"></param>
        /// <returns></returns>
        public static bool CheckForApplicationByName(string AppName)
        {
            bool bRtn = false;
            foreach (Process p in Process.GetProcesses("."))
            {
                try
                {
                    if (p.ProcessName.ToString().ToLower() == AppName.ToLower())
                    {
                        bRtn = true;
                    }
                }
                catch { }
            }
            return bRtn;
        }

        #endregion
    }
}
