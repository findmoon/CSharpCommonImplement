using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinimizeSystemTray
{
    public partial class MinimizeSystemTrayForm : Form
    {
        public MinimizeSystemTrayForm()
        {
            InitializeComponent();

            #region 最小化到任务托盘所有的处理
            FormClosing += MainForm_FormClosing;
            SizeChanged += MainForm_SizeChanged;

            notifyIcon.Text = "MyApp-" + Text;
            notifyIcon.MouseDoubleClick += ShowNomalFormEvent;
            notifyIcon.ContextMenuStrip = notifyCtxMenuStrip;
            notifyCtxMenuStrip.Items["exit"].Click += Notity_ContextMenu_Exit_Click;
            notifyCtxMenuStrip.Items["showForm"].Click += ShowNomalFormEvent;
            #endregion
            
        }

        #region 最小化到任务托盘所有的处理
        /// <summary>
        /// 主窗口关闭，最小化到任务托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //取消关闭，进入任务托盘
            if (e.CloseReason == CloseReason.UserClosing)//判断关闭事件来源 当通过点击窗口的关闭按钮时最小化到任务托盘
            {
                e.Cancel = true;
                // this.WindowState = FormWindowState.Minimized;
                Hide();

                ShowInTaskbar = false;
            }
        }
        /// <summary>
        /// 主窗口大小变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //判断是否是最小化，隐藏ShowInTaskbar，否则从ShowInTaskbar关闭将无法恢复显示
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
        }

        /// <summary>
        /// 右键任务托盘 退出
        /// </summary>
        private void Notity_ContextMenu_Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出?", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //必须先释放资源，然后Close关闭，否则会会触发FormClosing事件(因为FormClosing事件方法中已经取消关闭)。
                //使用 Application.Exit必须判断关闭事件的来源，窗口的关闭取消
                Application.Exit();
                //释放资源
                //Dispose();
                //Close();
            }
        }

        /// <summary>
        /// 双击托盘的图标和右键菜单 显示窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowNomalFormEvent(object sender, EventArgs e)
        {
            ShowNomalForm();
        }

        //显示主界面
        void ShowNomalForm()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            //任务栏区显示图标。必须设置任务栏图标显示，否则窗口可能不显示
            ShowInTaskbar = true;
            Show();
            //激活窗体并给予焦点
            Activate();
        }
        #endregion

        private void MinimizeSystemTrayForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("1");
        }
    }
}
