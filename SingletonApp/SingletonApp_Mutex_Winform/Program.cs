#define GlobalSingleton
using System;
using System.Windows.Forms;

namespace SingletonApp_Mutex_Winform
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool ret;

            // 注意 自定义编译符号 GlobalSingleton 实现 全局单体应用 或 文件夹内单体应用，可根据  注释#define GlobalSingleton
#if GlobalSingleton
            // 全局单个应用，当前系统内单个应用
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
#else
            // 当前文件夹内单个应用。不同文件夹，可以打开，相同文件夹只打开一个
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ExecutablePath.Replace("\\",""), out ret);
#endif
            if (ret)
            {
                #region 原有的默认Winform项目生成的代码
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //Application.DoEvents();

                Application.Run(new Form1());
                #endregion

                // 释放全局互斥体  
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
            else
            {
                // 提示信息。
                // 可根据需要修改为 复制 SingletonApp_Winform\Program.cs 文件内的进程获取代码 myProcess = GetRunningInstance();
                // - 并设置窗体前置激活 CallBack myCallBack = new CallBack(FindAppWindow);EnumWindows(myCallBack, 0); 或 HandleRunningInstanceWhnd(myProcess);
                MessageBox.Show(null, "有一个和本程序相同的应用程序已经在运行，请不要同时运行多个程序。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   
                Application.Exit();//退出程序
                return;
            }

        }
    }
}
