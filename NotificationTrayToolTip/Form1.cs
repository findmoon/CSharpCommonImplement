using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationTrayToolTip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon.Visible = false;
            
        }

        private void buttonPro1_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = true;
            //notifyIcon.ShowBalloonTip(0, "消息标题-你好", "我是气泡提示框的消息内容", ToolTipIcon.Info);// timoeout参数已经无效，通知的显示时间基于系统的辅助功能设置
            //notifyIcon.ShowBalloonTip(0, "消息标题-Error", "这是一个错误类型的消息内容", ToolTipIcon.Error);

            notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
            notifyIcon.BalloonTipText = "测试";
            notifyIcon.BalloonTipTitle = "标题";
            notifyIcon.ShowBalloonTip(0);
        }

        private void buttonPro2_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            //notifyIcon.Visible = true;
        }
    }
}
