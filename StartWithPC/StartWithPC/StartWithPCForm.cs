using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StartWithPC
{
    public partial class StartWithPCForm : Form
    {
        public StartWithPCForm()
        {
            InitializeComponent();
            Load += StartWithPCForm_Load;
        }

        private void StartWithPCForm_Load(object sender, EventArgs e)
        {
            //SetStartupAutoRunButton(runWithPCBtn);
            SetStartupAutoRunButton(runWithPC_Admin_Btn);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartWithPCAndAutoRun_Click(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartWithPCAndAutoRun_Click(sender);
        }
        #region 按钮状态变更的操作
        /// <summary>
        /// 设置 开启自启动且自动运行 按钮 的状态(文字、颜色)
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="startText"> 或 "设置开机启动且自动运行"</param>
        /// <param name="stopText"> 或 "关闭开机启动且自动运行"</param>
        /// <returns></returns>
        private bool SetStartupAutoRunButton(Control btn, string startText = "设置开机启动", string stopText = "关闭开机启动")
        {
            // 非管理员
            //if (System.StartWithPC.IsRunWithPC)

            // 管理员
            try
            {
                if (StartWithPC_Admin.IsRunWithPC)
                {
                    btn.Text = stopText;
                    btn.BackColor = SystemColors.ActiveBorder;
                    return true;
                }
                else
                {
                    btn.Text = startText;
                    btn.BackColor = Color.PaleTurquoise;
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("请以管理员权限运行！");
                return false;
            }
        }
        private void StartWithPCAndAutoRun_Click(object sender)
        {
            if (sender is Control btn)
            {
                #region // 非管理员

                //System.StartWithPC.SetMeAutoStart(btn.Text.StartsWith("设置"));
                //SetStartupAutoRunButton(btn, "设置为开机启动（非管理员）", "取消开机启动（非管理员）");

                #endregion

                #region 管理员
                try
                {
                    if (btn.Text.StartsWith("设置"))
                    {
                        StartWithPC_Admin.RunWithPC();
                    }
                    else
                    {
                        StartWithPC_Admin.CancleRunWithPC();
                    }
                    SetStartupAutoRunButton(btn, "设置为开机启动（管理员）", "取消开机启动（管理员）");

                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("请以管理员权限运行！");
                }
                #endregion
            }
        }
        #endregion
    }
}
