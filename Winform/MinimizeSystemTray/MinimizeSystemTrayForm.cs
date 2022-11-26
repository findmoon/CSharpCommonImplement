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

            #region 最小化到系统托盘 和 程序退出处理 所有操作
            Application.ApplicationExit += Application_ApplicationExit;
            #region 最小化到任务托盘和显示的所有处理
            // 是否最小化时隐藏taskbar，注释掉则不隐藏
            SizeChanged += MainForm_SizeChanged;
            // 系统托盘中鼠标hover图标时的提示文本，默认为窗口标题
            notifyIcon.Text = Text;

            FormClosing += MainForm_FormClosing;
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            notifyIcon.ContextMenuStrip = contextMenuStrip;

            // 托盘右键菜单中的显示和退出
            exitAppMenuItem.Click += ExitAppMenuItem_Click;
            showWindowMenuItem.Click += ShowWindowMenuItem_Click;
            #endregion
            #endregion

        }

        #region 最小化到任务托盘所有的处理
        /// <summary>
        /// 应用程序退出处理 如有需要额外的处理
        /// 不能在 ApplicationExit 中操作控件相关，因为已经释放，无法使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            // 可以捕获 taskkill /im WebCfgManager.exe 的终止，但是taskkill /f /im WebCfgManager.exe不会捕获
            // 程序退出的一些清理操作。不能访问控件
            //ClearHandler();
            //watcher?.Dispose();
            //client?.Dispose();
        }
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
            else
            {
                // 关闭时额外的清理（如控件的一些清理）
            }
        }
        /// <summary>
        /// 主窗口大小变化，最小化时隐藏ShowInTaskbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //判断是否是最小化，隐藏ShowInTaskbar
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
        }

        /// <summary>
        /// 双击托盘的图标 显示窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowNomalForm();
        }

        /// <summary>
        /// 显示主界面 标准大小
        /// </summary>
        void ShowNomalForm()
        {
            //任务栏区显示图标。必须设置任务栏图标显示，否则窗口可能不显示
            ShowInTaskbar = true;
            Show();
            // Show 之后再设置 WindowState 是最正确的顺序，否则 如果最小化后再从Taskbar关闭会导致无法恢复显示出来       
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            //激活窗体并给予焦点
            Activate();
        }
        /// <summary>
        /// 右键菜单“退出”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitAppMenuItem_Click(object sender, EventArgs e)
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
        /// 右键菜单“显示”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowWindowMenuItem_Click(object sender, EventArgs e)
        {
            ShowNomalForm();
        }
        #endregion

        private void MinimizeSystemTrayForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("1");
        }
    }
}
