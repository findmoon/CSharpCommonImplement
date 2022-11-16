using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RightTopButton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //MaximizeBox = false;
            //MinimizeBox = false;

            //FormBorderStyle = FormBorderStyle.None;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;

            //ControlBox = false;
            //Text = "";

            // FormClosing += MainForm_FormClosing;
            
        }
        /// <summary>
        /// 禁用关闭
        /// </summary>
        /// <param name="m"></param>
        //private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true; // 取消关闭
        //}

        /// <summary>
        /// 禁用关闭
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            // 消息类型
            const int WM_SYSCOMMAND = 0x0112;
            // 关闭按钮对应的消息值
            const int SC_CLOSE = 0xF060;
            if ((m.Msg== WM_SYSCOMMAND) && ((int)m.WParam==SC_CLOSE))
            {
                return; // 不处理关闭消息
            }
            // 调用基类方法
            base.WndProc(ref m);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
