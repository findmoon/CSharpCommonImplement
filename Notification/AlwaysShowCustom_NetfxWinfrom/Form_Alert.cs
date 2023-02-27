using System;
using System.Drawing;
using System.Windows.Forms;

namespace NotificationCustom
{

    public enum MsgType
    {
        Success,
        Warning,
        Error,
        Info
    }
    public partial class Form_Alert : Form
    {
        /// <summary>
        /// 设置消息窗置前的间隔，单位ms，默认每1000ms置前一次
        /// </summary>
        public int Interval { get=> execStateTimer.Interval; set=> execStateTimer.Interval=value; }

        /// <summary>
        /// 创建通知窗体，默认为info消息
        /// </summary>
        /// <param name="name">窗体名称</param>
        /// <param name="msg">窗体消息</param>
        public Form_Alert( string msg, string name) :this(msg)
        {
            Name = name;
        }
        /// <summary>
        /// 创建通知窗体，默认为info消息
        /// </summary>
        /// <param name="msg">窗体消息</param>
        public Form_Alert(string msg) :this(msg, MsgType.Info)
        {
        }
        /// <summary>
        /// 创建通知窗体
        /// </summary>
        public Form_Alert(string msg, MsgType msgType, Font msgFont = null)
        {
            InitializeComponent();

            closePictureBox.Cursor= Cursors.Hand;
            closePictureBox.MouseEnter += ClosePictureBox_MouseEnter;
            closePictureBox.MouseLeave += ClosePictureBox_MouseLeave;

            ShowInTaskbar = false;
            
            StartPosition = FormStartPosition.CenterScreen;

            InitType(msg, msgType, msgFont);
        }

        private void ClosePictureBox_MouseLeave(object sender, EventArgs e)
        {
            closePictureBox.BackColor = Color.Transparent;
        }

        private void ClosePictureBox_MouseEnter(object sender, EventArgs e)
        {
            closePictureBox.BackColor = Color.FromArgb(Math.Min(BackColor.R + 60,255), Math.Min(BackColor.G + 60, 255), Math.Min(BackColor.B + 60, 255));
        }



        /// <summary>
        /// 静态方法时的显示
        /// </summary>
        private static Form_Alert showOneForm;

        private void StateTimer_Tick(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void ClosePictureBox_Click(object sender, EventArgs e)
        {
            execStateTimer.Stop();
            execStateTimer.Dispose();
            Close();
        }

        /// <summary>
        /// 设置消息、类型
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <param name="msgFont">字体，默认不指定即可</param>
        private void InitType(string msg, MsgType msgType, Font msgFont = null)
        {
            switch (msgType)
            {
                case MsgType.Success:
                    msgTypePictureBox.Image = AlwaysShowCustom_NetfxWinfrom.Properties.Resources.success;
                    BackColor = Color.SeaGreen;
                    break;
                case MsgType.Error:
                    msgTypePictureBox.Image = AlwaysShowCustom_NetfxWinfrom.Properties.Resources.error;
                    BackColor = Color.DarkRed;
                    break;
                case MsgType.Info:
                    msgTypePictureBox.Image = AlwaysShowCustom_NetfxWinfrom.Properties.Resources.info;
                    BackColor = Color.RoyalBlue;
                    break;
                case MsgType.Warning:
                    msgTypePictureBox.Image = AlwaysShowCustom_NetfxWinfrom.Properties.Resources.warning;
                    BackColor = Color.DarkOrange;
                    break;
            }
            if (msgFont!=null)
            {
                lblMsg.Font = msgFont;
            }
            lblMsg.Text = msg;

        }
        /// <summary>
        /// 显示自定义通知框
        /// </summary>
        /// <param name="msg">通知的消息</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="msgFont">消息文本的字体</param>
        public static void ShowNotice(string msg, MsgType msgType= MsgType.Error,Font msgFont=null)
        {
            if (showOneForm==null || showOneForm.IsDisposed || showOneForm.Disposing)
            {
                showOneForm = new Form_Alert(msg, msgType, msgFont);
                showOneForm.ShowNotice();
            }
            else
            {
                if (msgFont != null)
                {
                    showOneForm.lblMsg.Font = msgFont;
                }
                showOneForm.ShowNotice(msg, msgType);
            }
        }

        
        /// <summary>
        /// 显示自定义通知框
        /// </summary>
        /// <param name="msg">通知的消息</param>
        /// <param name="msgType">消息类型</param>
        public void ShowNotice(string msg, MsgType msgType= MsgType.Info)
        {
            InitType(msg, msgType);
            ShowNotice();
        }

        /// <summary>
        /// 显示自定义通知框
        /// </summary>
        /// <param name="msg">通知的消息</param>
        /// <param name="msgType">消息类型</param>
        public void ShowNotice()
        {
            Show();
            execStateTimer.Start();
        }

    }
}
