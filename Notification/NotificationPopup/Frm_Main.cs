using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NotificationPopup
{
    public partial class Frm_Main : Form
    {
        public Frm_Main()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
        }

        private void display_Click(object sender, EventArgs e)
        {
            PopupFrm.PopupForm.AutoClose = false;
            PopupFrm.PopupForm.ShowForm("Popup消息框", "欢迎您订阅我们的消息\r\n如有相关问题，可随时联系！");//显示窗体

            //var pop = new PopupFrm();
            //pop.ShowForm("新消息","您好，欢迎来到xxx");
        }

        private void close_Click(object sender, EventArgs e)
        {
            PopupFrm.PopupForm.CloseForm();//隐藏窗体
        }
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private void Frm_Main_Load(object sender, EventArgs e)
        {
            int AW_HIDE = 0x00010000; //该变量表示动画隐藏窗体
        int AW_SLIDE = 0x00040000;//该变量表示出现滑行效果的窗体
        int AW_VER_NEGATIVE = 0x00000008;//该变量表示从下向上开屏
        int AW_VER_POSITIVE = 0x00000004;//该变量表示从上向下开屏
        AnimateWindow(this.Handle, 800, AW_SLIDE + AW_VER_NEGATIVE);
        }
    }
}
