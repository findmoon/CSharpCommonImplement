using CMCode.Handle;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace WindowHandle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wndHandle = WndHelper.FindWindow(null, "Form测试窗体的标题栏");            

            if (wndHandle != IntPtr.Zero)
            {
                //找到Button
                IntPtr btnHandle = WndHelper.FindWindowEx(wndHandle, IntPtr.Zero, null, "点击测试");

                //IntPtr btnHandle3 = WndHelper.FindWindowEx(msgHandle, IntPtr.Zero, "Control", "点击测试");
                if (btnHandle != IntPtr.Zero)
                {
                    WndHelper.SendClick(btnHandle);
                }

                IntPtr btnHandle2 = WndHelper.FindWindowEx(wndHandle, IntPtr.Zero, null, "Click");
                if (btnHandle2 != IntPtr.Zero)
                {
                    WndHelper.SendClick(btnHandle2);
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            #region WPF无法使用FindWindowEx
            //// winform直接使用.Handle属性
            ////nret = WndHelper.GetClassName(testBtn2.Handle, className, className.Capacity);

            //// wpf获取句柄
            //// new WindowInteropHelper(this).Handle // WPF只有一个窗口句柄，其他控件作为窗口内容存放
            //IntPtr btnHandle = WndHelper.FindWindowEx(new WindowInteropHelper(this).Handle, IntPtr.Zero, null, "测试查找子窗体(控件)");
            //if (btnHandle != IntPtr.Zero)
            //{
            //    WndHelper.SendClick(btnHandle);
            //}

            #endregion
            // 代码触发
            testBtn1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        }

        private void TestBtn1_Copy_Click(object sender, RoutedEventArgs e)
        {
            var wndHandle = WndHelper.FindWindow(null, "测试"); // 查找MessageBox窗体

            if (wndHandle != IntPtr.Zero)
            {
                IntPtr noBtnHandle = WndHelper.FindWindowEx(wndHandle, IntPtr.Zero, null, "否(&N)"); // 使用&对应快捷按键，查找MessageBox中的"否(N)"按钮
                if (noBtnHandle != IntPtr.Zero)
                {
                    WndHelper.SendClick(noBtnHandle);
                }
            }
        }

        private void TestBtn1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //HwndSource source = (HwndSource)HwndSource.FromVisual(testBtn1);
            //IntPtr hWnd = source.Handle;
            //if (hWnd != IntPtr.Zero)
            //{
            //    WndHelper.SendClick(hWnd); // 无效，点击实际发送到了顶层窗口
            //}

            #region 获取没有视觉化的对象句柄为null
            //ListBox lst = new ListBox();
            //HwndSource lstHwnd = HwndSource.FromVisual(lst) as HwndSource;
            //WindowInteropHelper windowHwnd = new WindowInteropHelper(this);

            //Debug.Assert(lstHwnd.Handle == windowHwnd.Handle); 
            #endregion


            HwndSource lstHwnd = HwndSource.FromVisual(testBtn1) as HwndSource;
            WindowInteropHelper windowHwnd = new WindowInteropHelper(this);

            Debug.Assert(lstHwnd.Handle == windowHwnd.Handle);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var wndHandle = WndHelper.FindWindow(null, "Form测试窗体的标题栏");

            if (wndHandle != IntPtr.Zero)
            {
                IntPtr txtHandle = WndHelper.FindWindowEx(wndHandle, IntPtr.Zero, null, "");

                if (txtHandle != IntPtr.Zero)
                {
                    WndHelper.SendText(txtHandle,"我是文字");
                }

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var wndHandle = WndHelper.FindWindow(null, "Form测试窗体的标题栏");

            if (wndHandle != IntPtr.Zero)
            {
                IntPtr txtHandle = WndHelper.FindWindowEx(wndHandle, IntPtr.Zero, null, "");

                if (txtHandle != IntPtr.Zero)
                {
                    WndHelper.SendKey(txtHandle, 65); // 发送字母A
                }

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var wndHandle = WndHelper.FindWindow(null, "Form测试窗体的标题栏");

            if (wndHandle != IntPtr.Zero)
            {
                var childWs = WndHelper.FindAllChildWindows(wndHandle, x => x.Title == "");
                var rect=WndHelper.GetClientRect(childWs[0].Hwnd);
                Console.WriteLine(rect);
                
            }
        }
    }
}
