**C#中实现只运行一个应用程序（只启动一个进程）的方法：WPF程序只启动一次**

[toc]

# 获取WPF的可执行程序路径

Winform 中可以用 `Application.ExecutablePath` 来获得程序路径

而 WPF 中 Application 类已经换了含义，无法获取。

但是可以借助`Assembly`反射获取当前正在运行的程序集的路径：

```C#
using System.Reflection;

Assembly.GetExecutingAssembly().Location;
```

# App.xaml.cs 文件内实现只启用一个进程的逻辑

> 新建WPF项目`SingletonApp_Mutex_WPF`。

如下所示，`App.xaml.cs` 中，通过 Startup 和 Exit 事件处理全局互斥体的获取和释放：

```C#
#define GlobalSingleton
using System;
using System.Reflection;
using System.Windows;

namespace SingletonApp_Mutex_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        bool mutex_ret;
        System.Threading.Mutex mutex;

        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
            Exit += App_Exit;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {

            // 注意 自定义编译符号 GlobalSingleton 实现 全局单体应用 或 文件夹内单体应用，可根据  注释#define GlobalSingleton
#if GlobalSingleton
            // 全局单个应用，当前系统内单个应用
            mutex = new System.Threading.Mutex(true, Assembly.GetExecutingAssembly().FullName, out mutex_ret);
#else
            // 当前文件夹内单个应用。不同文件夹，可以打开，相同文件夹只打开一个
            mutex = new System.Threading.Mutex(true, Assembly.GetExecutingAssembly().Location.Replace("\\", ""), out mutex_ret);
#endif

            if (!mutex_ret)
            {
                // 可以考虑给出 不能多次打开的提示
                // 或参考 SingletonApp_Winform\Program.cs 中的方法，获取已打开的进程，实现wpf程序窗口前置并激活。
                Environment.Exit(0);
                //Application.Current.Shutdown();

            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (mutex_ret)
            {
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
        }
    }
}
```

也可以同样修改为 遍历进程的方式 实现。

