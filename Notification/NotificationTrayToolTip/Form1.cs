using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NotificationTrayToolTip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon.Visible = false;

            //必须设置ShowToolTips为true，当鼠标位于tab时显示提示
            tabControl1.ShowToolTips = true;
            tabPage1.ToolTipText = "你好，我是tab提示";


            dataGridView1.ShowCellToolTips = true;
            dataGridView1[0, 0].ToolTipText = "单元格的tip";


            toolStrip1.ShowItemToolTips = true;
            toolStripLabel1.AutoToolTip = true;
            toolStripLabel1.ToolTipText = "toolStrip1子项的提示";

            menuStrip1.ShowItemToolTips = true;
            menuStrip按钮ToolStripMenuItem.AutoToolTip = true;
            menuStrip按钮ToolStripMenuItem.ToolTipText = "menuStrip1子项的提示";

            listView1.ShowItemToolTips = true;
            listView1.Items[0].ToolTipText = "我是listViewItem的提示";

            toolTip.SetToolTip(button1, "我是按钮1提示");
            toolTip.SetToolTip(radioButton1, "单选提示，必需选择");
            toolTip.SetToolTip(buttonPro1, "使用ToolTip的提示");


            toolTip.AutomaticDelay = 300;
            toolTip.AutoPopDelay = 10000;

            //toolTip.ToolTipIcon = ToolTipIcon.Error;
            //toolTip.ToolTipTitle = "错误提示！";

            // 气泡提示框
            toolTip.IsBalloon = true;

            // 父控件不激活也显示
            toolTip.ShowAlways = true;

            button1.KeyUp += Button1_KeyDown;
            button1.MouseEnter += Button1_MouseEnter;


            toolTip.SetToolTip(label1, "我是一段内容非常长的提示框信息，\r\n主要是测试消息提示框的大小修改或者\r\n宽度需改的情况，可以做到根据消息\r\n多少很好的显示消息框");
            toolTip.Popup += ToolTip_Popup;
        }

        private void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            if (e.AssociatedControl==label1)
            {
                e.ToolTipSize = new Size(230, 90);
            }
        }

        private void Button1_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(button1, "鼠标进入提示");
        }

        private void Button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Enter)
            {
                //toolTip.Hide(button1);
                //toolTip.Show("密码不能为空，邮箱格式不正确等", button1); // 一直显示
                //toolTip.Show("密码不能为空，只显示5秒", button1,5000); // 显示5000毫秒
                toolTip.Show("密码不能为空，在上方显示", button1, 0, -button1.Height, 5000);
            }
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
