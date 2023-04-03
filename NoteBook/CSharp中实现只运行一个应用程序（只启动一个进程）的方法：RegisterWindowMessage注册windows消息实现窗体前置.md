C#中实现只运行一个应用程序（只启动一个进程）的方法：RegisterWindowMessage注册windows消息实现窗体前置

[toc]

> **本质是进程间通信将窗体前置的消息发送到已打开的窗体，并处理消息实现前置！**
>
> 所以，除了 注册Windows消息 之外的其他任何 进程间通信 的方法，都可以实现该功能。

# windows消息处理窗体前置


> `SingletonApp_Mutex_Winform` 项目下实现

## 本地方法 NativeMethods

DllImport 引入发送和注册Windows消息的 Win32 API，并实现注册消息。

```C#
internal class NativeMethods {
    public const int HWND_BROADCAST = 0xffff;
    public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
    [DllImport("user32")]
    public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
    [DllImport("user32")]
    public static extern int RegisterWindowMessage(string message);

    /// <summary>
    /// 切换窗口获得焦点
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="fAltTab"></param>
    [DllImport("user32.dll ", SetLastError = true)]
    public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
}
```

## Form 窗体 WndProc 处理 Windows 消息

WndProc 消息处理中，如果是自定义的 `NativeMethods.WM_SHOWME` 则处理窗体显示和前置。

> 注：此处理中 `ActiveControl = this` 不可用

```C#
public partial class Form1 : Form {
    public Form1() {
        InitializeComponent();
    }
    protected override void WndProc(ref Message m) {
        if(m.Msg == NativeMethods.WM_SHOWME) {
            ShowMe();
        }
        base.WndProc(ref m);
    }
    private void ShowMe()
    {
        if (WindowState == FormWindowState.Minimized)
        {
            WindowState = FormWindowState.Normal;
        }

        #region 借助SwitchToThisWindow前置窗体。实际效果不完美  比如 Debug模式下启动，然后进入bin/Debug程序文件夹下点击，并不会实现前置窗体【SingletonApp_Winform可以实现】 其他情况下正常
        // 即使加上这两个
        //NativeMethods.ShowWindowAsync(Handle, NativeMethods.WS_SHOWNORMAL);
        //NativeMethods.SetForegroundWindow(Handle);

        NativeMethods.SwitchToThisWindow(this.Handle, true); 
        #endregion
    }
}
```

> 此处，SwitchToThisWindow前置窗体。实际效果不完美  比如 Debug模式下启动，然后进入bin/Debug程序文件夹下点击，并不会实现前置窗体【SingletonApp_Winform可以实现】 其他情况下正常

参考文章**借助 TopMost 实现前置**，不过使用它后的效果不太好，**需要多次的点击资源管理器（甚至无效，需要点击资源管理顶部），TopMost置前的才会置后【暂时无法解决】。**

```C#
private void ShowMe() {
    if(WindowState == FormWindowState.Minimized) {
        WindowState = FormWindowState.Normal;
    }
    #region TopMost 在资源管理器中的实际效果不太好，需要多次的点击资源管理器（甚至无效，需要点击资源管理顶部），TopMost置前的才会置后
    // get our current "TopMost" value (ours will always be false though)
    bool top = TopMost;
    // make our form jump to the top of everything
    TopMost = true;
    // set it back to whatever it was
    TopMost = top;
    #endregion

    // InvokeLostFocus(this, EventArgs.Empty);
}
```

> 也可以在自定义消息处理中，向自己发送显示和前置的Windows消息。比如：
> 
> ```C#
> ShowWindowAsync(this.Handle, WS_SHOWNORMAL);
> SetForegroundWindow(this.Handle);
> ```
> 
> 具体参见之前的介绍。

## Program 中实现发送自定义的显示用的Windows消息

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
                Application.Run(new Form1());
                #endregion

                // 释放全局互斥体  
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
            else
            {
                #region 通过注册发送Windows消息实现消息处理前置窗体
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                #endregion
            }
        }
    }
}
```

# 重点：使用GUID作为全局互斥体的名称

如参考文章中介绍的内容，其创建 全局Mutex 时使用的是 GUID 作为名称。这样可以防止出现重复的名称。

```C#
static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
```

包括通过遍历进程名获取已打开的进程，也是一样，最好再添加额外的能够唯一标识的判断，防止重复！

# 参考

[C# .NET Single Instance Application](http://sanity-free.org/143/csharp_dotnet_single_instance_application.html)