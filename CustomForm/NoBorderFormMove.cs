using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomForm
{
    public partial class NoBorderFormMove : Form
    {

        public NoBorderFormMove()
        {
            InitializeComponent();

            #region 方法0：Win32 API ReleaseCapture 和 SendMessage 实现    方法5
            //MouseDown += Form_MouseDown;
            //MouseDown += WindowMove_Win32_2_MouseDown;
            #endregion

            #region 方法1： 鼠标按下、移动和抬起事件中，Left、Top直接变化
            MouseDown += WindowMove_LeftTop_MouseDown;
            MouseMove += WindowMove_LeftTop_MouseMove;
            MouseUp += WindowMove_LeftTop_MouseUp;

            // 子控件拖动
            roundPanel1.MouseDown += WindowMove_LeftTop_MouseDown;
            roundPanel1.MouseMove += WindowMove_LeftTop_MouseMove;
            roundPanel1.MouseUp += WindowMove_LeftTop_MouseUp;
            #endregion

            #region 方法2：鼠标按下、移动和抬起事件中，计算移动后的Location
            //MouseDown += WindowMove_Location_MouseDown;
            //MouseMove += WindowMove_Location_MouseMove;
            //MouseUp += WindowMove_Location_MouseUp; 

            //roundPanel1.MouseDown += WindowMove_Location_MouseDown;
            //roundPanel1.MouseMove += WindowMove_Location_MouseMove;
            //roundPanel1.MouseUp += WindowMove_Location_MouseUp;
            #endregion

            #region 方法3 计算鼠标相对于(窗体)左上角的位置，借助 Offset 鼠标屏幕坐标点平移(相对位置) 实现
            //MouseDown += FrmMain_MouseDown;
            //MouseMove += FrmMain_MouseMove;
            //MouseUp += FrmMain_MouseUp;

            //roundPanel1.MouseDown += WindowMove_Location_MouseDown;
            //roundPanel1.MouseMove += WindowMove_Location_MouseMove;
            //roundPanel1.MouseUp += WindowMove_Location_MouseUp;
            #endregion

            #region 方法4
            //MouseDown += WindowMove4_MouseDown;
            //MouseMove += WindowMove4_MouseMove;

            //roundPanel1.MouseDown += WindowMove4_MouseDown;
            //roundPanel1.MouseMove += WindowMove4_MouseMove;
            #endregion
        }


        #region 方法0：窗体移动，借助Win32 API实现移动
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); // 释放鼠标捕获
                // 向Windows发送拖动窗体的消息，下面的消息仅在左键按下时有效
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }


        #region Win32 API函数
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();//从当前线程中的窗口释放鼠标捕获（释放被当前线程中某个窗口捕获的光标），并恢复正常的鼠标输入处理
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);//向指定的窗体发送Windows消息
        #endregion
        #region SendMessage需要的变量
        public const int WM_SYSCOMMAND = 0x0112;//该变量表示将向Windows发送的消息类型
        public const int SC_MOVE = 0xF010;//该变量表示发送消息的附加消息
        public const int HTCAPTION = 0x0002;//该变量表示发送消息的附加消息
        #endregion 
        #endregion

        #region 方法1：窗体移动,直接变化Left、Top
        private Point originMouseLocation;
        private bool isMove = false;

        // 无效
        //private int originLeft;
        //private int originTop;

        private void WindowMove_LeftTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMove = false;
            }
        }

        private void WindowMove_LeftTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originMouseLocation.X;
                Top += e.Location.Y - originMouseLocation.Y;
                #endregion

                #region 通过原始的鼠标按下时的Left和Top计算移动的变化量 反而不行
                ////Left = originLeft + (e.Location.X - originMouseLocation.X);
                ////Top = originTop + (e.Location.Y - originMouseLocation.Y);
                #endregion

                #region 每次计算最新的originMouseLocation为当前e.Location 也不行
                ////Left += e.Location.X - originMouseLocation.X;
                ////Top += e.Location.Y - originMouseLocation.Y;
                ////originMouseLocation = e.Location; 
                #endregion

                //// originMouseLocation位置不变
                //originLocationLbl.Text = $"{originMouseLocation.X},{originMouseLocation.Y}；Left:{Left},Top:{Top}";
            }
        }

        private void WindowMove_LeftTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originMouseLocation = e.Location;
                isMove = true;

                //originLeft = Left;
                //originTop = Top;
            }
        }
        #endregion

        #region 方法1：简化方法 窗体移动,直接变化Left、Top
        private Point originMouseLocation_Simplify;

        private void WindowMove_LeftTop_Simplify_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originMouseLocation_Simplify.X;
                Top += e.Location.Y - originMouseLocation_Simplify.Y;
                #endregion
            }
        }

        private void WindowMove_LeftTop_Simplify_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originMouseLocation_Simplify = e.Location;
            }
        }
        #endregion

        #region 方法2：窗体移动，通过计算Location位置
        // 鼠标按下
        private bool isMouse = false; // 鼠标是否按下
        // 原点位置
        private int originX = 0;
        private int originY = 0;
        // 鼠标按下位置
        private int mouseX = 0;
        private int mouseY = 0;
        private void WindowMove_Location_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            { // 判断鼠标按键
                isMouse = true;
                // 屏幕坐标位置
                originX = this.Location.X;
                originY = this.Location.Y;
                // 鼠标按下位置
                mouseX = originX + e.X;
                mouseY = originY + e.Y;
            }
        }

        // 鼠标移动
        private void WindowMove_Location_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouse)
            {
                // 移动距离
                int moveX = (e.X + this.Location.X) - mouseX;
                int moveY = (e.Y + this.Location.Y) - mouseY;
                int targetX = originX + moveX;
                int targetY = originY + moveY;
                this.Location = new Point(targetX, targetY);
            }
        }

        // 鼠标释放
        private void WindowMove_Location_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouse = false;
            }
        }
        #endregion

        #region 方法3 计算鼠标相对于(窗体)左上角的位置，借助 Offset 鼠标屏幕坐标点平移(相对位置) 实现
        private Point mouseOff; //抓取窗体Form中的鼠标的坐标,需要设置一个参数
        private bool leftFlag;  //标签，用来标记鼠标的左键的状态
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)  //鼠标左键按下后触发的MouseDown事件
        {
            if (e.Button == MouseButtons.Left)   //判断鼠标左键是否被按下
            {
                mouseOff = new Point(e.X, e.Y); //通过结构，将鼠标在窗体中的坐标（e.X,e.Y）赋值给mouseOff参数
                leftFlag = true;    //标记鼠标左键的状态
            }
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)  //鼠标移动触发的MouseMove事件
        {
            if (leftFlag)    //判断，鼠标左键是否被按下
            {
                Point mouseSet = Control.MousePosition; //抓取屏幕中鼠标光标所在的位置
                mouseSet.Offset(-mouseOff.X, -mouseOff.Y);  //Offset按指定量平移坐标，两个坐标相减，得到窗体左上角相对于屏幕的坐标
                Location = mouseSet;    //将上面得到的坐标赋值给窗体Form的Location属性
            }
        }

        private void FrmMain_MouseUp(object sender, MouseEventArgs e)    //鼠标释放按键后触发的MouseUp事件
        {
            if (e.Button == MouseButtons.Left)
            {
                leftFlag = false;
            }
        }
        #endregion

        #region 方法4，2、3、4原理一致，只是使用的方法不一样
        Point mPoint;
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMove4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mPoint = new Point(e.X, e.Y); // 鼠标相对于(窗体)左上角位置
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMove4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }
        #endregion

        #region 方法5：Win32 API SendMessage 发送 0XA1 消息
        private const int VM_NCLBUTTONDOWN_ = 0XA1; //VM_NCLBUTTONDOWN //定义鼠标左键按下
        private const int HTCAPTION_ = 2; // HTCAPTION
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMove_Win32_2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //为当前应用程序释放鼠标捕获
                ReleaseCapture();
                //发送消息 让系统误以为在标题栏上按下鼠标
                SendMessage(this.Handle, VM_NCLBUTTONDOWN_, HTCAPTION_, 0);
            }
        }
        #endregion
    }
}
