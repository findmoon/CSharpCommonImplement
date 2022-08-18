using CMControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomForm
{
    public partial class CustomTitleBar : Form
    {
        public CustomTitleBar()
        {
            InitializeComponent();

            #region 拖动标题栏、窗体，移动窗体
            TitlePanelTitle.MouseMove += WindowMove_MouseMove;
            TitlePanelTitle.MouseDown += WindowMove_MouseDown;
            TitleIconPicb.MouseMove += WindowMove_MouseMove;
            TitleIconPicb.MouseDown += WindowMove_MouseDown;

            MouseMove += WindowMove_MouseMove;
            MouseDown += WindowMove_MouseDown;
            #endregion

            #region 自定义标题栏 标题、icon、最大、最小、还原、关闭按钮和图标
            TitlePanelTitle.ForeColor = Color.WhiteSmoke;

            MinimizePicb.MouseEnter += MinimizePicb_MouseEnter;
            MinimizePicb.MouseLeave += MinimizePicb_MouseLeave;
            MaximizeNormalPicb.MouseEnter += MaximizeNormalPicb_MouseEnter;
            MaximizeNormalPicb.MouseLeave += MaximizeNormalPicb_MouseLeave;
            ClosePicb.MouseEnter += ClosePicb_MouseEnter;
            ClosePicb.MouseLeave += ClosePicb_MouseLeave;

            MinimizePicb.Click += MinimizePicb_Click;
            MaximizeNormalPicb.Click += MaximizeNormalPicb_Click;
            ClosePicb.Click += ClosePicb_Click;


            // 处理无Icon时Title标题的位置和最大化范围
            Load += CustomTitleBar_Load;
            #endregion

            #region 标题栏双击窗体最大化或正常
            TitlePanelTitle.DoubleClick += MaximizeNormalPicb_Click; ;
            #endregion

            Paint += CustomTitleBar_Paint;
            Resize += CustomTitleBar_Resize;
        }


        #region 绘制渐变色的背景 不是必须，可指定一个单一的背景色
        /// <summary>
        /// Resize中强制重绘，防止大小变化，渐变没有正确及时显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomTitleBar_Resize(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void CustomTitleBar_Paint(object sender, PaintEventArgs e)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            {
                return;
            }
            using (Graphics graphics = e.Graphics)
            {
                var rect = ClientRectangle;
                //var rect = new Rectangle(0, TitlePnl.Height, Width, Height - TitlePnl.Height);
                using (Brush b = new LinearGradientBrush(rect, Color.RoyalBlue, Color.Purple, 52))
                {
                    graphics.FillRectangle(b, rect);
                };
            }
        } 
        #endregion

        #region 通过重写 WndProc 方法实现拖拽调整窗体大小
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

            #region 实现鼠标点击边缘调整窗体大小
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
            }
            #endregion
        }
        #endregion
        #region 方法1：简化方法 窗体移动,直接变化Left、Top
        private Point originMouseLocation_Simplify;
        private void WindowMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originMouseLocation_Simplify.X;
                Top += e.Location.Y - originMouseLocation_Simplify.Y;
                #endregion
            }
        }
        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originMouseLocation_Simplify = e.Location;
            }
        }
        #endregion

        #region 自定义标题栏操作 标题、icon、最大、最小、还原、关闭按钮和图标 
        private void ClosePicb_Click(object sender, EventArgs e)
        {
            Close();//Application.Exit();
        }

        private void MaximizeNormalPicb_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                //MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_White;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                //MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_White;
            }
        }

        private void MinimizePicb_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ClosePicb_MouseLeave(object sender, EventArgs e)
        {
            //ClosePicb.Image = Properties.Resources.Close_16_16_Gray;
            ClosePicb.Image = Properties.Resources.Close_16_16_White;
            ClosePicb.BackColor = Color.Transparent;
        }

        private void ClosePicb_MouseEnter(object sender, EventArgs e)
        {
            ClosePicb.Image = Properties.Resources.Close_16_16_White;
            ClosePicb.BackColor = Color.Crimson;
        }

        private void MaximizeNormalPicb_MouseLeave(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                //MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_White;
            }
            else
            {
                //MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_White;
            }
            MaximizeNormalPicb.BackColor = Color.Transparent;
        }

        private void MaximizeNormalPicb_MouseEnter(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_Black;
            }
            else
            {
                MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_Black;
            }
            MaximizeNormalPicb.BackColor = SystemColors.GradientActiveCaption;
        }

        private void MinimizePicb_MouseLeave(object sender, EventArgs e)
        {
            //MinimizePicb.Image = Properties.Resources.Minimize_16_16_Gray;
            MinimizePicb.Image = Properties.Resources.Minimize_16_16_White;
            MinimizePicb.BackColor = Color.Transparent;
        }

        private void MinimizePicb_MouseEnter(object sender, EventArgs e)
        {
            MinimizePicb.Image = Properties.Resources.Minimize_16_16_Black;
            MinimizePicb.BackColor = SystemColors.GradientActiveCaption;
        }
        #region 加载时处理标题栏icon、最大化显示任务栏
        private void CustomTitleBar_Load(object sender, EventArgs e)
        {
            if (TitleIconPicb.Image == null)
            {
                TitlePanelTitle.Left -= TitleIconPicb.Width - 3;
            }
            MaximizedBounds = Screen.GetWorkingArea(this); // 设置最大化时显示为窗体所在工作区(不包含任务栏)  Screen.PrimaryScreen.WorkingArea
        }
        #endregion
        #endregion

    }
}
