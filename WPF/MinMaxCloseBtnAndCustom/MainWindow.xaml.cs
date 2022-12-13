using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinMaxCloseBtnAndCustom
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // 设置最大化、最小化按钮的隐藏
            // ResizeMode = ResizeMode.NoResize;
            //WindowStyle = WindowStyle.None;


            InitializeComponent();

            Activated += MainWindow_Activated;

            //Loaded += MainWindow_Loaded;

            //Closing += MainWindow_Closing;

            // 事件不触发
            //Initialized += MainWindow_Initialized;


        }




        #region 禁用关闭按钮（变灰）
        //[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        //private static extern IntPtr GetSystemMenu(IntPtr hWnd, UInt32 bRevert);
        //[DllImport("USER32.DLL ", CharSet = CharSet.Unicode)]
        //private static extern UInt32 RemoveMenu(IntPtr hMenu, UInt32 nPosition, UInt32 wFlags);
        //private const UInt32 SC_CLOSE = 0x0000F060;
        //private const UInt32 MF_BYCOMMAND = 0x00000000;

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var hwnd = new WindowInteropHelper(this).Handle;  //获取window的句柄
        //    IntPtr hMenu = GetSystemMenu(hwnd, 0);
        //    RemoveMenu(hMenu, SC_CLOSE, MF_BYCOMMAND);
        //}
        #endregion

        #region 取消关闭操作（禁用关闭[功能]）
        //private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //}

        ///// <summary>
        ///// 禁用关闭
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    e.Cancel = true;
        //} 
        #endregion

        #region 去除隐藏最大化最小化关闭按钮
        private const int GWL_STYLE = -16;

        // 窗口标题栏上的菜单【最大最小关闭】
        private const int WS_SYSMENU = 0x80000;
        // 最大化按钮
        private const int WS_MAXIMIZEBOX = 0x10000;
        // 最小化按钮
        private const int WS_MINIMIZEBOX = 0x20000;

        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        extern private static int GetWindowLongPtr(IntPtr hWnd, int nindex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        extern private static int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        /// <summary>
        /// 隐藏最小化最大化按钮，等同 ResizeMode.NoResize
        /// </summary>
        /// <param name="hwnd"></param>
        private static void HideMinMaxButtons(IntPtr hwnd)
        {
            if (IntPtr.Size == 4)
            {
                var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
                SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
            }
            else
            {
                var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
                SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
            }
            //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
            SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
        /// <summary>
        /// 隐藏最小化最大化关闭按钮
        /// </summary>
        /// <param name="hwnd"></param>
        private static void HideMinMaxCloseButtons(IntPtr hwnd)
        {
            if (IntPtr.Size == 4)
            {
                var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
                SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
            }
            else
            {
                var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
                SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
            }
            //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
            SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
        /// <summary>
        /// [禁用]最小化按钮 变非常灰 
        /// </summary>
        /// <param name="hwnd"></param>
        private static void DisableMinButton(IntPtr hwnd)
        {
            if (IntPtr.Size == 4)
            {
                var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
                SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_MINIMIZEBOX);
            }
            else
            {
                var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
                SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_MINIMIZEBOX);
            }
            //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
            SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
        /// <summary>
        /// [禁用]最大化按钮 变非常灰
        /// </summary>
        /// <param name="hwnd"></param>
        private static void DisableMaxButton(IntPtr hwnd)
        {
            if (IntPtr.Size == 4)
            {
                var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
                SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX);
            }
            else
            {
                var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
                SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX);
            }
            //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
            SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }

        /// <summary>
        /// MainWindow_Loaded MainWindow_Activated 等事件中执行，会在按钮显示后再消失(隐藏)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            //HideMinMaxCloseButtons(hwnd);
            //HideMinMaxButtons(hwnd);
            //DisableMinButton(hwnd);
            DisableMaxButton(hwnd);
        }
        #endregion
    }
}
