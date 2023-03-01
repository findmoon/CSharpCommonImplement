using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace CMCode.Handle
{
    /// <summary>
    /// 包含枚举当前用户空间下所有窗口的方法。
    /// </summary>
    public class WndHelper
    {
        #region 根据进程信息获取句柄
        /// <summary>
        /// 根据窗口标题查找窗口句柄【不推荐】
        /// </summary>
        /// <param name="puzze_title">窗口标题，支持模糊查询可指定标题的一部分；为null则返回所有句柄</param>
        /// <returns></returns>
        [Obsolete("不推荐，MainWindow(Handle)获取多数情况下会不正确")]
        public List<IntPtr> FindHwndsByTitle(string puzze_title = null)
        {
            //按照窗口标题来寻找窗口句柄
            Process[] ps = Process.GetProcesses();
            var intptrs = new List<IntPtr>();
            foreach (Process p in ps)
            {
                if (puzze_title != null && !p.MainWindowTitle.Contains(puzze_title))
                {
                    continue;
                }
                intptrs.Add(p.MainWindowHandle);
            }
            return intptrs;
        }

        #endregion
        #region 枚举查找窗体信息
        /// <summary>
        /// 查找当前用户空间下所有符合条件的(顶层)窗口。如果不指定条件，将仅查找可见且有标题栏的窗口。
        /// </summary>
        /// <param name="match">过滤窗口的条件。如果设置为 null，将仅查找可见和标题栏不为空的窗口。</param>
        /// <returns>找到的所有窗口信息</returns>
        public static IReadOnlyList<WindowInfo> FindAllWindows(Predicate<WindowInfo> match = null)
        {
            windowList = new List<WindowInfo>();
            //遍历窗口并查找窗口相关WindowInfo信息
            EnumWindows(OnWindowEnum, 0);
            return windowList.FindAll(match ?? DefaultPredicate);
        }
        /// <summary>
        /// 遍历窗体处理的函数
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        private static bool OnWindowEnum(IntPtr hWnd, int lparam)
        {
            // 仅查找顶层窗口。 Win8开始，不需要此判断
            if (GetParent(hWnd) == IntPtr.Zero)
            {
                WindowInfo wdnInfo = GetWindowInfo(hWnd);
                // 添加到已找到的窗口列表。
                windowList.Add(wdnInfo);
            }

            return true;
        }

        /// <summary>
        /// 默认的查找窗口的过滤条件。可见 + 非最小化 + 包含窗口标题。
        /// </summary>
        private static readonly Predicate<WindowInfo> DefaultPredicate = x => x.IsVisible && !x.IsMinimized && x.Title.Length > 0;
        /// <summary>
        /// 窗体列表
        /// </summary>
        private static List<WindowInfo> windowList;
        #endregion

        #region 枚举查找子窗体
        /// <summary>
        /// 查找窗口句柄下所有符合条件的子窗口（控件）。如果不指定条件，将仅查找可见的窗口。
        /// </summary>
        /// <param name="hWndParent">父窗口</param>
        /// <param name="match">过滤窗口的条件。如果设置为 null，将仅查找可见和标题栏不为空的窗口。</param>
        /// <returns>找到的所有窗口信息</returns>
        public static IReadOnlyList<WindowInfo> FindAllChildWindows(IntPtr hWndParent, Predicate<WindowInfo> match = null)
        {
            childWindowList = new List<WindowInfo>();
            //遍历窗口并查找窗口相关WindowInfo信息
            EnumChildWindows(hWndParent,OnChileWindowEnum, 0);
            return childWindowList.FindAll(match ?? ChildWDefaultPredicate);
        }
        /// <summary>
        /// 遍历窗体处理的函数
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        private static bool OnChileWindowEnum(IntPtr hWnd, int lparam)
        {
            WindowInfo wdnInfo = GetWindowInfo(hWnd);
            // 添加到已找到的窗口列表。
            childWindowList.Add(wdnInfo); 

            return true;
        }
        /// <summary>
        /// 默认的查找窗口的过滤条件。可见 + 非最小化 + 包含窗口标题。
        /// </summary>
        private static readonly Predicate<WindowInfo> ChildWDefaultPredicate = x => x.IsVisible && !x.IsMinimized;
        /// <summary>
        /// 窗体列表
        /// </summary>
        private static List<WindowInfo> childWindowList;
        #endregion

        #region FindWindow(Ex)查找(子)窗体
        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <param name="lpClassName">窗体的类名称，比如Form、Window。若不知类名，指定为null或default(string)即可</param>
        /// <param name="lpWindowName">窗体的标题/文字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 根据标题查找窗口，第一个参数必须为IntPtr.Zero
        /// </summary>
        /// <param name="ZeroOnly"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        /// <summary>
        /// 查找指定控件的子窗体（控件）
        /// </summary>
        /// <param name="hwndParent">父窗体句柄，不知道窗体时可指定IntPtr.Zero</param>
        /// <param name="hwndChildAfter">子窗体(控件)，必须为父窗体的直接子窗体，通常不知道子窗体(句柄)，指定IntPtr.Zero即可</param>
        /// <param name="lpszClass">子窗体(控件)的类名，通常指定null，它是window class name，并不等同于C#中的列名Button、Image、PictureBox等，两者并不相同，可通过GetClassName获取正确的类型名</param>
        /// <param name="lpszWindow">子窗体的名字或控件的Title、Text，通常为显示的文字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        /// <summary>
        /// 查找指定控件的子窗体（控件）
        /// </summary>
        /// <param name="hwndParent">父窗体句柄，不知道窗体时可指定IntPtr.Zero</param>
        /// <param name="hwndChildAfter">子窗体(控件)，必须为父窗体的直接子窗体，通常不知道子窗体(句柄)，指定IntPtr.Zero即可</param>
        /// <param name="lpszClass">子窗体(控件)的类名，通常指定null，它是window class name，并不等同于C#中的列名Button、Image、PictureBox等，两者并不相同，可通过GetClassName获取正确的类型名</param>
        /// <param name="lpszWindow">子窗体的名字或控件的Title、Text，通常为显示的文字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindChildWindow(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        #endregion

        #region PostMessage发送到窗口的消息队列【推荐】
        /// <summary>
        /// 发送文本到窗口句柄(或控件)的消息队列，并立即返回
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        /// <returns>发送成功</returns>
        public static bool PostText(IntPtr hWnd, string text)
        {
            return PostMessage(hWnd, WM_SETTEXT, IntPtr.Zero, text);
            //return PostMessage(hWnd, WM_SETTEXT, text, IntPtr.Zero); // 也是可以的，具体区别不知
        }
        /// <summary>
        /// 发送回车键(Enter)到窗口(或控件)的消息队列，并立即返回
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns>发送成功</returns>
        public static bool PostEnter(IntPtr hWnd)
        {
            return PostMessage(hWnd, WM_CHAR, VK_ENTER, 0);
        }
        /// <summary>
        /// 发送键盘按键到窗口(或控件)的消息队列，并立即返回
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="keyValue">按键的数字值KeyValue</param>
        /// <returns>发送成功</returns>
        public static bool PostKey(IntPtr hWnd, int keyValue)
        {
            return PostMessage(hWnd, WM_CHAR, keyValue, 0);
        }
        /// <summary>
        /// 关闭窗口，并立即返回
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns>发送成功</returns>
        public static bool PostCloseWindow(IntPtr hwnd)
        {
            return PostMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        /// <summary>
        /// 发送点击事件到消息队列，并立即返回
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        /// <returns>发送成功</returns>
        public static bool PostClick(IntPtr hwnd)
        {
            return PostMessage(hwnd, WM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }
        /// <summary>
        /// 发送点击事件到消息队列，并立即返回
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        /// <param name="x">鼠标位置x</param>
        /// <param name="y">鼠标位置y</param>
        /// <returns>发送成功</returns>
        public static bool PostClick(IntPtr hwnd, int X, int Y)
        {
            int lparm = (Y << 16) + X;
            return PostMessage(hwnd, WM_CLICK, 0, lparm);
        }

        /// <summary>
        /// 发送鼠标点击事件的消息队列，并立即返回，包含MouseDown和MouseUp
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        /// <param name="x">鼠标位置x</param>
        /// <param name="y">鼠标位置y</param>
        /// <returns>发送成功</returns>
        public static bool PostMouseClick(IntPtr hwnd, int X, int Y)
        {
            int lparm = (Y << 16) + X;
            var lngResult = PostMessage(hwnd, WM_LBUTTONDOWN, 0, lparm);
            var lngResult2 = PostMessage(hwnd, WM_LBUTTONUP, 0, lparm);
            return lngResult && lngResult2;
        }

        #endregion

        #region SendMessage向窗口发送消息或指令
        /// <summary>
        /// 发送文本到窗口句柄(或控件)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        public static void SendText(IntPtr hWnd, string text)
        {
            SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, text);
            //SendMessage(hWnd, WM_SETTEXT, text, IntPtr.Zero); // 放在第三个参数也是可以的，区别暂时不知
        }
        /// <summary>
        /// 发送回车键(Enter)到窗口(或控件)
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SendEnter(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_CHAR, VK_ENTER, 0);
        }
        /// <summary>
        /// 发送键盘按键到窗口(或控件)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="keyValue">按键的数字值KeyValue</param>
        public static void SendKey(IntPtr hWnd, uint keyValue)
        {
            SendMessage(hWnd, WM_CHAR, keyValue, 0);
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="hwnd"></param>
        public static void CloseWindow(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        /// <summary>
        /// 发送点击事件
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        public static void SendClick(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }
        /// <summary>
        /// 发送点击事件
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        /// <param name="x">鼠标位置x</param>
        /// <param name="y">鼠标位置y</param>
        public static void SendClick(IntPtr hwnd, int X, int Y)
        {
            int lparm = (Y << 16) + X;
            SendMessage(hwnd, WM_CLICK, 0, lparm);
        }

        /// <summary>
        /// 发送鼠标点击事件，包含MouseDown和MouseUp
        /// </summary>
        /// <param name="hwnd">控件句柄</param>
        /// <param name="x">鼠标位置x</param>
        /// <param name="y">鼠标位置y</param>
        public static void SendMouseClick(IntPtr hwnd, int X, int Y)
        {
            int lparm = (Y << 16) + X;
            int lngResult = SendMessage(hwnd, WM_LBUTTONDOWN, 0, lparm);
            int lngResult2 = SendMessage(hwnd, WM_LBUTTONUP, 0, lparm);
        }

        #endregion


        #region 窗口操作（枚举、获取窗口相关信息、父窗口、查找子窗口）
        #region 遍历子窗体
        //public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam); 

        /// <summary>
        /// 遍历子窗体(控件)
        /// </summary>
        /// <param name="hwndParent">父窗口句柄</param>
        /// <param name="lpEnumFunc">遍历的回调函数</param>
        /// <param name="lParam">传给遍历时回调函数的额外数据</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WndEnumProc lpEnumFunc, int lParam);
        #endregion


        /// <summary>
        /// 枚举窗口时的委托参数
        /// </summary>
        /// <param name="hWnd">枚举的当前窗口句柄</param>
        /// <param name="lParam">EnumChildWindows调用时传递过来的参数</param>
        /// <returns></returns>
        private delegate bool WndEnumProc(IntPtr hWnd, int lParam);
        /// <summary>
        /// 枚举所有窗口，从Windows8开始，将仅遍历桌面应用的顶层窗口，不需要使用GetParent
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool EnumWindows(WndEnumProc lpEnumFunc, int lParam);
        /// <summary>
        /// 获取窗口的父窗口句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// 判断窗口是否可见
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lptrString, int nMaxCount);

        [DllImport("user32")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref LPRECT rect);


        #endregion


        #region SendMessage、PostMessage向窗口发送消息、事件或指令【推荐PostMessage】
        #region PostMessage
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        #endregion

        #region SendMessage
        // [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">接受消息的窗口句柄</param>
        /// <param name="wMsg">消息类型</param>
        /// <param name="wParam">指定附加的特定信息</param>
        /// <param name="lParam">指定附加的特定信息</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam); 
        #endregion

        #region 发送消息的(类型)常量
        /// <summary>
        /// 文本消息
        /// </summary>
        const uint WM_SETTEXT = 0x000C;
        /// <summary>
        /// 鼠标按下
        /// </summary>
        const uint WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// 鼠标抬起
        /// </summary>
        const uint WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// 点击消息
        /// </summary>
        const uint WM_CLICK = 0xF5;

        /// <summary>
        /// 关闭
        /// </summary>
        const uint WM_CLOSE = 0x0010;

        /// <summary>
        ///  字符消息 0x0102即258
        ///  按下某键,并且是已经发送wm_keydown和wm_keyup消息
        /// </summary>
        const uint WM_CHAR = 0x0102;

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;

        // The WM_COMMAND message is sent when the user selects a command item from 
        // a menu, when a control sends a notification message to its parent window, 
        // or when an accelerator keystroke is translated.
        public const int WM_COMMAND = 0x111;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;

        const int VK_ENTER = 0x0D; // 13 Enter键
        //const int VK_RETURN = 0x0D;
        const int VK_TAB = 0x09;  // tab键
        #endregion
        #endregion

        #region 其他HWND API
        /// <summary>
        /// 操作窗口状态
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow">0-隐藏窗口并激活其他窗口;1-激活并正常大小显示窗口;2-最小化窗口;3-最大化窗口;4-显示但不激活;其他</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 当前窗口是否最小化
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);
        /// <summary>
        /// 当前窗口是否最大化
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);
     

        /// <summary>
        /// 获取当前激活的正在工作的窗口句柄。返回值有可能为null
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 设置窗口标题Title
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowText(IntPtr hWnd, string title);


        /// <summary>
        /// 判断窗口是否存在
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// 获取鼠标坐标
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(out Point pt);
        /// <summary>
        /// 获取指定位置所在的窗口句柄
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        public static extern IntPtr WindowFromPoint(Point pt);
        /// <summary>
        /// 获取鼠标位置的窗体
        /// </summary>
        /// <returns></returns>
        public static IntPtr WindowFromCursor()
        {
            if (GetCursorPos(out Point pt))
            {
                return WindowFromPoint(pt);
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 获取窗口客户区大小
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, out LPRECT lpRect);
        /// <summary>
        /// 获取窗口客户区大小
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static Rectangle GetClientRect(IntPtr hwnd)
        {
            LPRECT rect = default;
            GetClientRect(hwnd, out rect);
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        /// <summary>
        /// 移动窗体、调整窗体大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="X">新位置</param>
        /// <param name="Y">新位置</param>
        /// <param name="nWidth">新大小</param>
        /// <param name="nHeight">新大小</param>
        /// <param name="bRepaint">是否重新渲染客户区，推荐始终为true</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint=true);
        #endregion

        #region 获取WindowInfo窗口相关信息
        /// <summary>
        /// 从窗口句柄获取WindowInfo窗口信息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static WindowInfo GetWindowInfo(IntPtr hWnd)
        {
            // 获取窗口类名。
            var lpString = new StringBuilder(512);
            GetClassName(hWnd, lpString, lpString.Capacity);
            var className = lpString.ToString();

            // 获取窗口标题。
            var lptrString = new StringBuilder(512);
            GetWindowText(hWnd, lptrString, lptrString.Capacity);
            var title = lptrString.ToString().Trim();

            // 获取窗口可见性。
            var isVisible = IsWindowVisible(hWnd);

            // 获取窗口位置和尺寸。
            LPRECT rect = default;
            GetWindowRect(hWnd, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            return new WindowInfo(hWnd, className, title, isVisible, bounds);
        }

        #endregion

        #region LPRECT结构
        /// <summary>
        /// Rect结构。获取窗体、客户区大小时
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct LPRECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        //private readonly struct LPRECT
        //{
        //    public readonly int Left;
        //    public readonly int Top;
        //    public readonly int Right;
        //    public readonly int Bottom;
        //} 
        #endregion
    }

    /// <summary>
    /// 获取 Win32 窗口的一些基本信息。
    /// </summary>
    public struct WindowInfo
    {
        public WindowInfo(IntPtr hWnd, string className, string title, bool isVisible, Rectangle bounds) : this()
        {
            Hwnd = hWnd;
            ClassName = className;
            Title = title;
            IsVisible = isVisible;
            Bounds = bounds;
        }

        /// <summary>
        /// 获取窗口句柄。
        /// </summary>
        public IntPtr Hwnd { get; }

        /// <summary>
        /// 获取窗口类名。
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// 获取窗口标题。
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 获取当前窗口是否可见。
        /// </summary>
        public bool IsVisible { get; }

        /// <summary>
        /// 获取窗口当前的位置和尺寸。
        /// </summary>
        public Rectangle Bounds { get; }

        /// <summary>
        /// 获取窗口当前是否是最小化的。
        /// </summary>
        public bool IsMinimized => Bounds.Left == -32000 && Bounds.Top == -32000;
    }

}
