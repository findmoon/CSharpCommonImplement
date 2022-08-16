using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomForm
{
    public partial class NoBorderFormDragResize : Form
    {
        public NoBorderFormDragResize()
        {
            InitializeComponent();

            #region 鼠标事件实现移动窗体
            ////MouseDown += Main_MouseDown;
            ////MouseUp += Main_MouseUp;


            //MouseMove += Main_MouseMove;
            //MouseLeave += Main_Leave; // 有控件在边缘时，处理一下更好一些 
            #endregion
        }

        #region 通过重写 WndProc 方法实现拖拽调整窗体大小、拖拽移动窗体

        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            #region 实现鼠标点击移动窗体
            if (m.Msg == 0x84)
            {
                // 拖拽调整窗体大小
                Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);

                vPoint = PointToClient(vPoint);

                if (vPoint.X <= 5)
                    if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOPLEFT;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                    else m.Result = (IntPtr)HTLEFT;
                else if (vPoint.X >= ClientSize.Width - 5)
                    if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOPRIGHT;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else m.Result = (IntPtr)HTRIGHT;
                else if (vPoint.Y <= 5)
                    m.Result = (IntPtr)HTTOP;
                else if (vPoint.Y >= ClientSize.Height - 5)
                    m.Result = (IntPtr)HTBOTTOM;

                // 鼠标左键按下实现拖动窗口功能
                if (m.Result.ToInt32() == 1)
                {
                    m.Result = new IntPtr(2);
                }
            }
            #endregion

            #region switch形式
            //switch (m.Msg)
            //{

            //    case 0x0084:

            //        Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);

            //        vPoint = PointToClient(vPoint);

            //        if (vPoint.X <= 5)

            //            if (vPoint.Y <= 5)

            //                m.Result = (IntPtr)HTTOPLEFT;

            //            else if (vPoint.Y >= ClientSize.Height - 5)

            //                m.Result = (IntPtr)HTBOTTOMLEFT;

            //            else m.Result = (IntPtr)HTLEFT;

            //        else if (vPoint.X >= ClientSize.Width - 5)

            //            if (vPoint.Y <= 5)

            //                m.Result = (IntPtr)HTTOPRIGHT;

            //            else if (vPoint.Y >= ClientSize.Height - 5)

            //                m.Result = (IntPtr)HTBOTTOMRIGHT;

            //            else m.Result = (IntPtr)HTRIGHT;

            //        else if (vPoint.Y <= 5)

            //            m.Result = (IntPtr)HTTOP;

            //        else if (vPoint.Y >= ClientSize.Height - 5)

            //            m.Result = (IntPtr)HTBOTTOM;

            //        // 鼠标左键按下实现拖动窗口功能
            //        if (m.Result.ToInt32() == 1)
            //        {
            //            m.Result = new IntPtr(2);
            //        }

            //        break;

            //    #region 无效
            //    //case 0x0201://鼠标左键按下的消息 用于实现拖动窗口功能

            //    //    m.Msg = 0x00A1;//更改消息为非客户区按下鼠标

            //    //    m.LParam = IntPtr.Zero;//默认值

            //    //    m.WParam = new IntPtr(2);//鼠标放在标题栏内

            //    //    break; 
            //    #endregion

            //    default:
            //        break;

            //}

            #endregion
        }

        #endregion

        #region 拖拽调整窗体大小 必须先进行调整大小，再处理鼠标状态。否则快速拖动（尤其向窗体内）可能导致鼠标位置变化（鼠标变更状态），但此时应该是调整窗体大小的过程中（鼠标状态变更导致大小调整终止）
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//左键按下移动，拖拽调整大小
            {
                // MousePosition的参考点是屏幕的左上角，表示鼠标当前相对于屏幕左上角的坐标。this.Left和this.Top的参考点也是屏幕
                if (Cursor == Cursors.SizeNWSE) // 倾斜拖拽 
                {
                    // 改变窗体宽和高的代码，其宽高为鼠标屏幕位置减去窗体的Left，Top距离
                    this.Width = MousePosition.X - this.Left;
                    this.Height = MousePosition.Y - this.Top;
                }
                else if (Cursor == Cursors.SizeWE) // 水平拖拽
                {
                    Width = MousePosition.X - this.Left;
                }
                else if (Cursor == Cursors.SizeNS) // 垂直拖拽
                {
                    Height = MousePosition.Y - this.Top;
                }
            }

            //鼠标移动过程中，坐标时刻在改变 
            //当鼠标移动时横坐标距离窗体右边缘5像素以内且纵坐标距离下边缘也在5像素以内时，要将光标变为倾斜的箭头形状
            if (e.Location.X >= this.Width - 5 && e.Location.Y > this.Height - 5)
            {
                this.Cursor = Cursors.SizeNWSE; // 右下角 双向对角线光标
            }
            //当鼠标移动时横坐标距离窗体右边缘5像素以内时，要将光标变为双向水平箭头形状
            else if (e.Location.X >= this.Width - 5)
            {
                this.Cursor = Cursors.SizeWE; // 双向水平光标
            }
            //当鼠标移动时纵坐标距离窗体下边缘5像素以内时，要将光标变为垂直水平箭头形状
            else if (e.Location.Y >= this.Height - 5)
            {
                this.Cursor = Cursors.SizeNS; // 双向垂直光标

            }
            //否则，以外的窗体区域，鼠标星座均为单向箭头（默认）             
            else this.Cursor = Cursors.Arrow;

        }

        private void Main_Leave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;// 移出窗体变为正常
        }
        #endregion



        #region 拖拽调整窗体大小。有些啰嗦了
        //enum MouseDirection
        //{
        //    None,
        //    Declining,
        //    Herizontal,
        //    Vertical
        //}
        //bool isMouseDown = false; //表示鼠标当前是否处于按下状态，初始值为否 
        //MouseDirection direction = MouseDirection.None;//表示拖动的方向，起始为None，表示不拖动
        //private void Main_MouseMove(object sender, MouseEventArgs e)
        //{
        //    //如果鼠标按下，同时有方向箭头那么直接调整大小,这里是改进的地方，不然斜角拉的过程中，会有问题
        //    if (isMouseDown && direction != MouseDirection.None)
        //    {
        //        //设定好方向后，调用下面方法，改变窗体大小  
        //        ResizeWindow();
        //    }

        //    //鼠标移动过程中，坐标时刻在改变 
        //    //当鼠标移动时横坐标距离窗体右边缘5像素以内且纵坐标距离下边缘也在5像素以内时，要将光标变为倾斜的箭头形状，同时拖拽方向direction置为MouseDirection.Declining 
        //    if (e.Location.X >= this.Width - 5 && e.Location.Y > this.Height - 5)
        //    {
        //        this.Cursor = Cursors.SizeNWSE;
        //        direction = MouseDirection.Declining;
        //    }
        //    //当鼠标移动时横坐标距离窗体右边缘5像素以内时，要将光标变为倾斜的箭头形状，同时拖拽方向direction置为MouseDirection.Herizontal
        //    else if (e.Location.X >= this.Width - 5)
        //    {
        //        this.Cursor = Cursors.SizeWE;
        //        direction = MouseDirection.Herizontal;
        //    }
        //    //同理当鼠标移动时纵坐标距离窗体下边缘5像素以内时，要将光标变为倾斜的箭头形状，同时拖拽方向direction置为MouseDirection.Vertical
        //    else if (e.Location.Y >= this.Height - 5)
        //    {
        //        this.Cursor = Cursors.SizeNS;
        //        direction = MouseDirection.Vertical;

        //    }
        //    //否则，以外的窗体区域，鼠标星座均为单向箭头（默认）             
        //    else
        //    {
        //        this.Cursor = Cursors.Arrow;
        //        //direction = MouseDirection.None; 
        //    }
        //}

        //private void ResizeWindow()
        //{
        //    //MousePosition的参考点是屏幕的左上角，表示鼠标当前相对于屏幕左上角的坐标this.left和this.top的参考点也是屏幕，属性MousePosition是该程序的重点
        //    if (direction == MouseDirection.Declining)
        //    {
        //        //下面是改变窗体宽和高的代码，不明白的可以仔细思考一下
        //        this.Width = MousePosition.X - this.Left;
        //        this.Height = MousePosition.Y - this.Top;
        //    }
        //    //以下同理
        //    if (direction == MouseDirection.Herizontal)
        //    {
        //        this.Width = MousePosition.X - this.Left;
        //    }
        //    else if (direction == MouseDirection.Vertical)
        //    {
        //        this.Height = MousePosition.Y - this.Top;
        //    }

        //}

        //private void Main_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isMouseDown = true;
        //}

        //private void Main_MouseUp(object sender, MouseEventArgs e)
        //{
        //    isMouseDown = false;
        //    direction = MouseDirection.None;
        //}
        #endregion

    }
}
