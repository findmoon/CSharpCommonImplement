using System;
using System.Drawing;
using System.Windows.Forms;

namespace NotificationCustom
{
    public enum NotificationFormAction
    {
        start,
        wait,
        close
    }

    public enum MsgType
    {
        Success,
        Warning,
        Error,
        Info
    }
    public partial class Form_Alert : Form
    {
  
        //public new int Height
        //{
        //    get
        //    {
        //        return base.Height;
        //    }
        //    set
        //    {
        //        base.Height=value;
        //        if (!(alertFormNum <= Screen.PrimaryScreen.WorkingArea.Height / (Height + 5) && alertFormNum > 0)) // 无法实时处理
        //        {
        //            alertFormNum = Screen.PrimaryScreen.WorkingArea.Height / (Height + 5);
        //        }
        //    }
        //}

        /// <summary>
        /// 通知窗体的数量，默认为垂直屏幕几乎占满的数量
        /// </summary>
        private static int alertFormNum = Screen.PrimaryScreen.WorkingArea.Height / (75 + 5); // 75为窗体高度，如果调整窗体高度，记得修改此处（默认在构造函数中已处理）

        /// <summary>
        /// 通知窗体的数量，默认为垂直屏幕几乎占满的数量，手动修改的数量不能超出屏幕和低于1，否则设置无效
        /// </summary>
        public static int AlertFormNum
        {
            get => alertFormNum;
            set
            {
                if (value <= Screen.PrimaryScreen.WorkingArea.Height / (75 + 5) && value > 0)
                {
                    alertFormNum = value;                 
                }
            }
        }
        /// <summary>
        /// 自定义通知的显示时间，单位为毫秒，默认为3分钟，之后开始消失。可根据需要修改
        /// </summary>
        public static int ShowTime { get; set; } = 3000;
        /// <summary>
        /// 是否移动进入，默认true
        /// </summary>
        public static bool MoveEntry { get; set; } = true;

        /// <summary>
        /// 创建通知窗体
        /// </summary>
        /// <param name="name">窗体名称，必须指定</param>
        public Form_Alert(string name)
        {
            InitializeComponent();

            Name = name;
            this.Opacity = 0.0;
            //ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            if (!(alertFormNum <= Screen.PrimaryScreen.WorkingArea.Height / (Height + 5) && alertFormNum > 0)) // 调整可能的高度不正确
            {
                alertFormNum = Screen.PrimaryScreen.WorkingArea.Height / (Height + 5);
            }

            // 关闭按钮 显示小手、背景变化
            closePictureBox.Cursor = Cursors.Hand;
            closePictureBox.MouseEnter += ClosePictureBox_MouseEnter;
            closePictureBox.MouseLeave += ClosePictureBox_MouseLeave;
        }
        private void ClosePictureBox_MouseLeave(object sender, EventArgs e)
        {
            closePictureBox.BackColor = Color.Transparent;
        }

        private void ClosePictureBox_MouseEnter(object sender, EventArgs e)
        {
            closePictureBox.BackColor = Color.FromArgb(Math.Min(BackColor.R + 60, 255), Math.Min(BackColor.G + 60, 255), Math.Min(BackColor.B + 60, 255));
        }

        private NotificationFormAction action = NotificationFormAction.start;
        /// <summary>
        /// 当前消息框的标准位置
        /// </summary>
        private int x, y;


        private void StateTimer_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case NotificationFormAction.wait:
                    execStateTimer.Interval = ShowTime;
                    action = NotificationFormAction.close;
                    break;
                case NotificationFormAction.start:
                    this.execStateTimer.Interval = 100;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left-=20; // 移动快点
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = NotificationFormAction.wait;
                        }
                    }
                    break;
                case NotificationFormAction.close:
                    execStateTimer.Interval = 100;
                    this.Opacity -= 0.1;

                    this.Left -= 20;
                    if (base.Opacity == 0.0)
                    {
                        execStateTimer.Stop();
                        base.Close();
                    }
                    break;
            }

            // tag记录下次执行的时间，用于后续的替换
            execStateTimer.Tag = DateTime.Now.AddMilliseconds(execStateTimer.Interval);
        }

        private void ClosePictureBox_Click(object sender, EventArgs e)
        {
            execStateTimer.Interval = 100;
            action = NotificationFormAction.close;
        }
        /// <summary>
        /// 设置完x、y之后执行初始化启动。设置位置、消息类型、显示、倒计时
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <param name="msgFont">字体，默认不指定即可</param>
        private void InitStart(string msg, MsgType msgType, Font msgFont = null)
        {
            //this.Location = new Point(frm.x, frm.y);
            this.Location = new Point(x + (MoveEntry?Width / 2:0), y);

            switch (msgType)
            {
                case MsgType.Success:
                    msgTypePictureBox.Image = Properties.Resources.success;
                    BackColor = Color.SeaGreen;
                    break;
                case MsgType.Error:
                    msgTypePictureBox.Image = Properties.Resources.error;
                    BackColor = Color.DarkRed;
                    break;
                case MsgType.Info:
                    msgTypePictureBox.Image = Properties.Resources.info;
                    BackColor = Color.RoyalBlue;
                    break;
                case MsgType.Warning:
                    msgTypePictureBox.Image = Properties.Resources.warning;
                    BackColor = Color.DarkOrange;
                    break;
            }
            if (msgFont!=null)
            {
                lblMsg.Font = msgFont;
            }
            lblMsg.Text = msg;

            Show();
            execStateTimer.Start();
            ShowInTaskbar = false;
        }
        /// <summary>
        /// 显示自定义通知框
        /// </summary>
        /// <param name="msg">通知的消息</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="msgFont">消息文本的字体</param>
        public static void ShowNotice(string msg, MsgType msgType,Font msgFont=null)
        {
            Form_Alert willDisappearFrm = null;
            for (int i = 1; i < alertFormNum+1; i++)
            {
                string fname = "alert" + i.ToString();
                Form_Alert frm = (Form_Alert)Application.OpenForms[fname];

                if (frm == null)
                {
                    frm = new Form_Alert(fname);

                    frm.x = Screen.PrimaryScreen.WorkingArea.Width - frm.Width - 5;
                    frm.y = Screen.PrimaryScreen.WorkingArea.Height - frm.Height * i - 5 * i;
                    // 设置完x、y之后执行初始化启动
                    frm.InitStart(msg, msgType, msgFont);

                    return;
                }
                else
                {
                    if (willDisappearFrm == null)
                    {
                        willDisappearFrm = frm;
                    }
                    else
                    {
                        if (willDisappearFrm.action < frm.action)
                        {
                            willDisappearFrm = frm;
                        }
                        else if (willDisappearFrm.action == frm.action)
                        {
                            // 不考虑一次没执行的情况
                            if (willDisappearFrm.execStateTimer.Tag!=null&& frm.execStateTimer.Tag != null)
                            {
                                if (willDisappearFrm.execStateTimer.Tag == null)
                                {
                                    willDisappearFrm = frm;
                                }
                                else if(frm.execStateTimer.Tag != null)
                                {
                                    if ((DateTime)willDisappearFrm.execStateTimer.Tag > (DateTime)frm.execStateTimer.Tag)
                                    {
                                        willDisappearFrm = frm;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 当前最早要消失的窗体willDisappearFrm被替换
            var newfrm = new Form_Alert(willDisappearFrm.Name);

            newfrm.x = Screen.PrimaryScreen.WorkingArea.Width - newfrm.Width - 5;
            newfrm.y = willDisappearFrm.Location.Y;

            // 必须立即替换name
            var totalNum = 0;
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form_Alert)
                {
                    totalNum += 1;
                }
            }
            willDisappearFrm.Name = $"Form_Alert{totalNum + 1}";
            willDisappearFrm.ClosePictureBox_Click(null, null);

            // 设置完x、y之后执行初始化启动
            newfrm.InitStart(msg, msgType, msgFont);
        }

        // 设计器中的修改并不会调用此方法调整alertFormNum
        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    if (!(alertFormNum <= Screen.PrimaryScreen.WorkingArea.Height / (Height + 5) && alertFormNum > 0))
        //    {
        //        alertFormNum = Screen.PrimaryScreen.WorkingArea.Height / (Height + 5);
        //    }
        //}
    }
}
