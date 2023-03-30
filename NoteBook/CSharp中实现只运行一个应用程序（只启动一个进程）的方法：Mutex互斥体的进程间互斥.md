**C#中实现只运行一个应用程序（只启动一个进程）的方法：Mutex互斥体的进程间互斥实现，及其最大优点**

[toc]

> 只运行一个程序，只能打开一个程序

# 互斥体的进程间互斥实现

Mutex 对象用于多线程与多进程之间对数据或资源的同步访问，每个调用线程在获取互斥体(mutex)所有权之前都会被阻塞，并且所有获取互斥体所有权的线程在使用完后都，**必须调用`ReleaseMutex`释放**。

> A synchronization primitive that can also be used for interprocess synchronization.

如下是 Mutex 进程间互斥实现的单体应用程序。同样，和之前的介绍一样，通过`#define GlobalSingleton`自定义编译符号，实现 全局单体应用 或 文件夹内的单体应用 两种形式。

默认全局整个系统只能运行一个程序。

```C#
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

                //   Main 为你程序的主窗体，如果是控制台程序不用这句   
                mutex.ReleaseMutex();
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
```

# 互斥体只运行一个应用的最大优点



# Mutex 互斥体介绍

