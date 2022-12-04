**C#中实现单体应用程序（只运行一个程序、只启动一个进程）的方法：判断进程【激活并显示已打开的程序到前台】**

[toc]

# 最简单实现代码

如下，通过判断进程，确定是否存在已打开的当前程序的进程，如果存在则将已打开的进程窗口置前，当前程序退出不再打开运行。

```cs
/// <summary>
/// 应用程序的主入口点。
/// </summary>
[STAThread]
static void Main()
{
    // 判断进程，只能启动一个实例
    Process cur = Process.GetCurrentProcess();
    foreach (Process p in Process.GetProcesses())
    {
        if (p.Id == cur.Id) continue;
        if (p.ProcessName == cur.ProcessName)
        {
            SetForegroundWindow(p.MainWindowHandle);
            return;
        }
    }
    // Main方法开始处添加上面的代码

    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new Form1());
}

    
[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
public static extern int SetForegroundWindow(IntPtr hwnd);
```

# 标准通用方法【直接复制使用，根据需要最小修改】

对应的实现只启动一个程序的代码放于 `Program.cs` 入口文件出，其全部代码如后面所示。具体可以直接复制，或者单独复制 `region` 部分的单体有关的代码。

代码解释已经非常详细，包括注意点。此代码原本是参考网上的内容，并进行了一些修改和说明，由于时间久远，未记录出处，后续碰到再添加。

如 `region` 部分的提示，注意以下几点（**重要，尤其第7条。**）：

1. 复制下面`单体应用`相关的代码到 `Program.cs` 入口文件，替换默认的 `Application.Run(new Form1());` 为 `SingletonApp();`。【或者，在项目新建后，直接复制下面的全部代码到`Program.cs`】

2. 如有需要 修改 `Application.Run` 运行的主窗体类名。

3. 注意定义 编译符号 `#define GlobalSingleton` 是否为全局单体。

4. 单体实现了全局单个应用（当前系统内单个应用），和 非全局单个应用，本示例为：同一个路径内单个应用。 具体可根据需要修改 `GetRunningInstance()` 内对 单体进程的获取 的代码。

5. 设置窗台到前台的 win32 API `SetForegroundWindow` 的使用注意，见代码注释。

6. 位于 `C:\Program Files (x86)` 等管理员权限的目录下时，使用`GetExecutingAssembly().Location` 获取程序集或位置，会报错无权限访问。需要启动程序也是以管理员权限。这一点需要注意，具体见代码内的注释。

    为什么如此，原因暂时不知。

7. 由于其单个程序运行的实现是通过判断`进程`，因此，**在`进程`启动起来的短时间内，是可以实现运行多个程序的（比如通过脚本快速运行多个、快速点击运行多个等）**。这也是很多 **启动多个程序的脚本** 能够成功的原因，比如 "运行多个xx"。

8. 不推荐使用 `Process.MainWindowHandle`，`MainWindowHandle`并不是win32窗体中的标准概念，在遇到多个窗口的winform程序时，此值的获取将会不准确。

> 测试或示例程序位于 `SingletonApp` 解决方案文件夹下的 `SingletonApp_Winform` 程序。

```C#
#define GlobalSingleton
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SingletonApp_Winform
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region 实现单体应用 替换默认的 Application.Run(new Form1());
            SingletonApp();
            #endregion            
        }

        #region 实现单体应用的代码。注意定义 编译符号 #define GlobalSingleton ；及 单体进程的获取 的处理
        /// <summary>
        /// 实现单体应用。注意修改 Application.Run 运行的主窗体类名 为正确的名称，通常 winform 默认为 Form1
        /// </summary>
        static void SingletonApp()
        {
            myProcess = GetRunningInstance();
            if (myProcess == null)
            {
                Application.Run(new Form1());
            }
            else
            {
                CallBack myCallBack = new CallBack(FindAppWindow);
                EnumWindows(myCallBack, 0);
            }
        }

        #region 单体应用处理  注意定义 条件编译符号 #define GlobalSingleton
        /// <summary>
        /// 【单体进程的获取】获取正在运行的实例，没有运行的实例返回null
        /// </summary>
        /// <returns></returns>
        private static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();//获取当前进程
            Process[] processes = Process.GetProcessesByName(current.ProcessName);//获取当前名称的进程数组
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)//当前进程id与进程组的id不相等时
                {
#if GlobalSingleton
                    // 全局单个应用，当前系统内单个应用
                    return process;
#else
                    // 非全局单个应用，比如下面，同一个路径内单个应用                    
                    try
                    {
                        // 安装在 `C:\Program Files (x86)` 下，此处执行 GetExecutingAssembly().Location 获取路径 会有无权访问的问题，因为此处需要管理员权限访问（其他位置时没有此问题）
                        // 理论上仅仅读取，非管理员权限应该就可以。具体原理不知？
                        //判断process的名字，不同文件夹，可以打开，相同文件夹只打开一个
                        if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == process.MainModule.FileName)
                        {
                            return process;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                        return process;
                    }
#endif
                }
            }
            return null;
        }

        /// <summary>
        /// 进程，仅运行显示一个的进程
        /// </summary>
        private static Process myProcess = null;

        private static bool FindAppWindow(IntPtr hwnd, int lParam)
        {
            ////获取窗口标题
            //StringBuilder sb = new StringBuilder(200);
            //int n = GetWindowText(hwnd, sb, sb.Capacity);
            int currentID;
            //获取进程ID
            GetWindowThreadProcessId(hwnd, out currentID);

            //if (sb.ToString() == mainWindowTitle&& process !=null&& currentID== process.Id)//标题栏进程ID相同
            if (currentID == myProcess?.Id)// 进程ID相同
            {
                HandleRunningInstanceWhnd(hwnd);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 处理进程的主窗体 显示到最前面 【不推荐】
        /// </summary>
        /// <param name="instance"></param>
        [Obsolete("不推荐，因为使用了 Process.MainWindowHandle")]
        private static void HandleRunningInstanceWhnd(Process instance)
        {
            HandleRunningInstanceWhnd(instance.MainWindowHandle);
        }
        /// <summary>
        /// 处理当前运行的窗体 显示到最前面或获得焦点
        /// </summary>
        /// <param name="windowHandle"></param>
        private static void HandleRunningInstanceWhnd(IntPtr windowHandle)
        {
            ShowWindowAsync(windowHandle, WS_SHOWNORMAL);//显示
            SetForegroundWindow(windowHandle);//放到前端
            SwitchToThisWindow(windowHandle, true);
        }
        #endregion

        #region 单体应用处理所用的 win32 API
        /// <summary>
        /// 设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        //private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        /// <summary>
        /// 将指定窗口设置到前台
        /// SetForegroundWindow 设置窗体到前方，有一个问题，如果有在系统托盘时，通过右键显示了一个右键菜单（不点击处理，点击其他位置使菜单隐藏），此时再设置窗体最前方，显示和设置的就是“右键的菜单”，也就是“右键的菜单”作为了“主窗体”的显示
        /// “右键的菜单”不显示的情况下，可正常显示设置窗体最前方
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 根据窗体句柄获得窗体标题 【暂时未用到，可考虑使用】
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpStr"></param>
        /// <param name="nCount"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpStr, int nCount);

        /// <summary>
        /// 枚举窗口
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int EnumWindows(CallBack x, int y);
        private delegate bool CallBack(IntPtr hWnd, int lParam);

        /// <summary>
        /// 根据窗口句柄获取进程id
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ID);

        /// <summary>
        /// 切换窗口获得焦点
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>
        [DllImport("user32.dll ", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        private const int WS_SHOWNORMAL = 1;
        //private const int SW_SHOW = 5;
        #endregion

        #endregion
    }
}
```