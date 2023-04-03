using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SingletonApp_Mutex_Winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //ActiveControl = this;     
        }
        /// <summary>
        /// 处理注册的Windows消息前置窗体
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
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
            #region TopMost 在资源管理器中的实际效果不太好，需要多次的点击资源管理器（甚至无效，需要点击资源管理顶部），TopMost置前的才会置后
            //// get our current "TopMost" value (ours will always be false though)
            //bool top = TopMost;
            //// make our form jump to the top of everything
            //TopMost = true;
            //// set it back to whatever it was
            //TopMost = top;
            #endregion

            // InvokeLostFocus(this, EventArgs.Empty);

            #region 借助SwitchToThisWindow前置窗体。实际效果不完美  比如 Debug模式下启动，然后进入bin/Debug程序文件夹下点击，并不会实现前置窗体【SingletonApp_Winform可以实现】 其他情况下正常
            // 即使加上这两个

            //NativeMethods.ShowWindowAsync(Handle, NativeMethods.WS_SHOWNORMAL);
            //NativeMethods.SetForegroundWindow(Handle);

            NativeMethods.SwitchToThisWindow(this.Handle, true); 
            #endregion
        }
    }
}
