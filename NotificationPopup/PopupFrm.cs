using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NotificationPopup
{
    public partial class PopupFrm : Form
    {
        #region 属性
        /// <summary>
        /// Pupup消息框窗体，静态唯一的主PopupForm窗体，默认将只显示这一个，也可以通过构造函数创建新的消息窗
        /// </summary>
        public static PopupFrm PopupForm
        {
            get
            {
                if (popupForm == null)
                {
                    popupForm = new PopupFrm();//实例化当前窗体;
                }
                return popupForm; //返回当前窗体的实例化对象 
            }
        }
        /// <summary>
        /// 是否自动关闭消息框，默认true
        /// </summary>
        public bool AutoClose { get; set; } = true;
        /// <summary>
        /// 显示时间，单位毫秒，默认3000
        /// </summary>
        public int ShowTime
        {
            get => timer.Interval;
            set
            {
                if (value > 0)
                {
                    timer.Interval = value;
                }
            }
        }
        #endregion
        #region 声明的变量
        //private System.Drawing.Rectangle Rect;//定义一个存储矩形框的数组
        private bool isAnimateShow = false;//当前消息窗是否显示，默认false

        // 静态唯一的主PopupForm窗体，默认将只显示这一个，也可以通过构造函数创建新的消息窗
        static private PopupFrm popupForm = null;

        private static int AW_HIDE = 0x00010000; //该变量表示动画隐藏窗体
        private static int AW_SLIDE = 0x00040000;//该变量表示出现滑行效果的窗体
        private static int AW_VER_NEGATIVE = 0x00000008;//该变量表示从下向上开屏
        private static int AW_VER_POSITIVE = 0x00000004;//该变量表示从上向下开屏
        #endregion

        #region 该窗体的构造方法
        public PopupFrm()
        {
            InitializeComponent();
            ////初始化工作区大小
            //System.Drawing.Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(this);//实例化一个当前窗口的对象
            //this.Rect = new System.Drawing.Rectangle(rect.Right - this.Width - 1,rect.Bottom - this.Height - 1,this.Width,this.Height);//为实例化的对象创建工作区域

            var workRect = Screen.GetWorkingArea(this);
            StartPosition = FormStartPosition.Manual;
            Location = new Point(workRect.Width - Width - 2, workRect.Height - Height - 2);

            closePicBox.Image = imageList1.Images[1];
            closePicBox.MouseEnter += pictureBox1_MouseEnter;
            closePicBox.MouseLeave += pictureBox1_MouseLeave;
            closePicBox.Click += PictureBox1_Click;

            timer.Interval = 3000;
            timer.Tick += Timer_Tick;

            Show();//必须调用一次Show，否则窗体内的PictureBox、button等控件不显示，如果放在ShowForm会导致提前显示或动画之后才会显示内部的控件，不优雅。采用Show再Hide的方式。
            Hide();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            CloseForm();
        }
        #endregion

        #region 调用API函数显示窗体
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        #endregion

        #region 鼠标控制图片的变化
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            closePicBox.Image = imageList1.Images[0];//设定当鼠标进入PictureBox控件时PictureBox控件的图片
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            closePicBox.Image = imageList1.Images[1]; //设定当鼠标离开PictureBox控件时PictureBox控件的图片
        }
        #endregion

        #region 显示窗体
        public void ShowForm(string title, string msg)
        {
            titleLbl.Text = title;
            msgLbl.Text = msg;
            if (!isAnimateShow)
            {
                AnimateWindow(this.Handle, 800, AW_SLIDE + AW_VER_NEGATIVE);//动态显示本窗体
                isAnimateShow = true;
                if (AutoClose)
                {
                    timer.Start();
                }
            }
        }
        #endregion

        #region 关闭窗体
        public void CloseForm()
        {
            AnimateWindow(this.Handle, 800, AW_SLIDE + AW_VER_POSITIVE + AW_HIDE);//动画隐藏窗体
            isAnimateShow = false;//设定当前窗体为非动画显示(隐藏)

            if (popupForm != this)
            {
                Close();
            }
            if (AutoClose)
            {
                timer.Stop();
            }
        }
        #endregion
    }
}
