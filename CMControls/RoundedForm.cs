using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CMControls
{
    /// <summary>
    ///通过 base.CreateParams.ExStyle |= 0x00080000; 扩展样式，启用 WS_EX_LAYERED ，相当于在原有的控件Region基础上重新绘制一个Layer层，原有窗体内容均被覆盖，需要获取其子控件并绘制到新的layer层   
    ///构造函数中设置Location无效
    /// </summary>
    public class RoundedForm : Form
    {
        #region 字段
        private Color bgStartColor = Color.DarkSlateBlue;
        private Color bgEndColor = Color.MediumPurple;
        private float linearGradient;

        #endregion
        #region 属性
        [Category("高级"), DefaultValue(true), Description("窗体是否固定大小，为true时，无法拖动边角调整窗体大小，默认true")]
        public bool FixedSize { get; set; } = true;
        [Category("高级"), DefaultValue(typeof(Color), "DarkSlateBlue"), Description("渐变背景开始的颜色，如果BgStartColor和BgEndColor颜色一样，则无渐变")]
        public Color BgStartColor
        {
            get => bgStartColor; set
            {
                bgStartColor = value;
                Validate();
            }
        }
        [Category("高级"), DefaultValue(typeof(Color), "MediumPurple"), Description("渐变背景结束的颜色，如果BgStartColor和BgEndColor颜色一样，则无渐变")]
        public Color BgEndColor
        {
            get => bgEndColor; set
            {
                bgEndColor = value;
                Validate();
            }
        }
        [Category("高级"), DefaultValue(0f), Description("背景颜色的渐变方向，默认0度，水平方向渐变")]
        public float LinearGradient
        {
            get => linearGradient; set
            {
                linearGradient = value;
                Validate();
            }
        }

        #endregion
        //private Timer drawTimer = new Timer();
        #region 构造函数
        public RoundedForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;

            #region 初始位置无效，Location设置在Load事件处理中有效
            // 居中无效
            //StartPosition = FormStartPosition.CenterScreen;
            // 手动设置一个默认位置 也无效  【构造函数中设置Location无效】
            //StartPosition = FormStartPosition.Manual;
            //Location = new Point(200, 200);

            // 无效
            //Left = 200;
            //Top = 200; 
            #endregion
        }
        #endregion
        #region override重写OnLoad显示、OnResize、OnShown后绘制Layered Windows【原代码是通过定时循环绘制窗体，感觉太频繁了，只需在OnLoad、OnResize中调用即可】；
        //protected override void OnLoad(EventArgs e)
        //{
        //    //if (!DesignMode)
        //    //{
        //    //    //drawTimer.Interval = 1000 / 60;
        //    //    //drawTimer.Tick += DrawRoundForm;
        //    //    //drawTimer.Start();                
        //    //}
        //    DrawRoundForm();
        //    base.OnLoad(e);
        //}

        protected override void OnResize(EventArgs e)
        {
            DrawRoundForm();
            base.OnResize(e);
        }
        protected override void OnShown(EventArgs e)
        {
            //if (DesignMode) // Form继承，在设计器中，可以在实时在Layered Windows显示出新增或调整的控件 【改为设计器中不使用Layered Windows】
            //{
            //    Timer drawTimer = new Timer();
            //    drawTimer.Interval = 1000 / 60;
            //    drawTimer.Tick += DrawRoundFormTick; ;
            //    drawTimer.Start();
            //}
            DrawRoundForm();
            base.OnShown(e);
        }

        //private void DrawRoundFormTick(object sender, EventArgs e)
        //{
        //    DrawRoundForm();
        //}

        /// <summary>
        /// 绘制圆角窗体，使Layered Windows显示出来
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pE"></param>
        //private void DrawRoundForm(object pSender, EventArgs pE)
        private void DrawRoundForm()
        {
            if (DesignMode) return;

            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            {
                return;
            }
            using (Bitmap backImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics graphics = Graphics.FromImage(backImage))
                {
                    //Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    Rectangle gradientRectangle = ClientRectangle;
                    using (Brush b = new LinearGradientBrush(gradientRectangle, BgStartColor, BgEndColor, LinearGradient))
                    {
                        graphics.FillRoundRectangle(gradientRectangle, b, 35);

                        foreach (Control ctrl in this.Controls)
                        {
                            using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height, PixelFormat.Format32bppArgb))
                            {
                                #region 一些测试
                                ////PerPixelAlphaBlend.SetBitmap(bmp, ctrl.Left, ctrl.Top, ctrl.Handle);
                                //// 设置每个像素透明，无效
                                ////for (int m = 0; m < bmp.Width; m++)
                                ////{
                                ////    for (int n = 0; n < bmp.Height; n++)
                                ////    {
                                ////        bmp.SetPixel(m, n, Color.FromArgb(0, 0, 0, 0));
                                ////    }
                                ////}

                                //Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
                                //ctrl.DrawToBitmap(bmp, rect);// 结合OnPaint中的绘制，能完美实现ctrl圆角的边角透明底层，原因Bitmap没有指定颜色，控件之外的部分透明（但又似乎和透明不一样，因为如果一样，直接绘制一个透明框就能漏出下面的控件，但实际只绘制框看不到控件）
                                //graphics.DrawImage(bmp, ctrl.Location);

                                ////PerPixelAlphaBlend.SetBitmap(bmp, ctrl.Left, ctrl.Top, ctrl.Handle); 
                                #endregion

                                ctrl.DrawToBitmap(bmp, ctrl.ClientRectangle); // 结合OnPaint中的绘制，能完美实现ctrl圆角的边角透明底层，原因（猜测可能是）Bitmap没有指定颜色，控件之外的部分透明
                                graphics.DrawImage(bmp, ctrl.Location);
                            }
                            #region 添加透明的控件，无法透明显示底层的背景，会是一个长方形的控件区域
                            //using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height, PixelFormat.Format32bppArgb))
                            //{
                            //    var control = new TransparentsControl(ctrl.Width, ctrl.Height);

                            //    control.DrawToBitmap(bmp, ctrl.ClientRectangle);
                            //    graphics.DrawImage(bmp, ctrl.Location);
                            //}
                            #endregion
                        }

                        PerPixelAlphaBlend.SetBitmap(backImage, Left, Top, Handle);//不执行将无法显示窗体
                        //backImage.MakeTransparent(Color.White);
                    }
                }
            }
        }
        /// <summary>
        /// 绘制圆角窗体，使窗体中的控件可以完美正确显示
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
            {
                return;
            }
            using (Graphics graphics = e.Graphics)
            {
                //Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                Rectangle gradientRectangle = ClientRectangle;

                using (Brush b = new LinearGradientBrush(gradientRectangle, BgStartColor, BgEndColor, LinearGradient))
                {
                    graphics.FillRoundRectangle(gradientRectangle, b, 35);
                };
            }
        }

        #endregion

        #region CreateParams中指定ExStyle
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode) 
                    cp.ExStyle |= 0x00080000;  // Form 添加 WS_EX_LAYERED 扩展样式 
                return cp;
            }
        }

        #endregion

        #region 通过重写 WndProc 实现拖拽调整窗体大小、拖拽移动窗体
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

            #region 实现鼠标拖拽调整窗体大【前面代码】、拖拽移动窗体【后面部分】
            if (m.Msg == 0x84)
            {
                if (!FixedSize)
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

                // 鼠标左键按下实现拖动窗口功能
                if (m.Result.ToInt32() == 1)
                {
                    m.Result = new IntPtr(2);
                }
            }
            #endregion
        }
        #endregion
    }


    #region 处理显示Layered Windows窗体，丝滑无锯齿。不知只调用Win32.UpdateLayeredWindow是否可以正确显示[应该不行，未测试]
    /// <summary>
    /// 设置每个像素Alpha透明通道颜色（Per Pixel Alpha Blend 【Alpha混合】）
    /// </summary>
    public static class PerPixelAlphaBlend
    {
        /// <summary>
        /// 设置每个像素Alpha透明通道颜色（Per Pixel Alpha Blend 【Alpha混合】）
        /// </summary>
        /// <param name="bitmap"></param>
        public static void SetBitmap(Bitmap bitmap, int left, int top, IntPtr handle)
        {
            SetBitmap(bitmap, 255, left, top, handle);
        }
        /// <summary>
        /// 设置每个像素Alpha透明通道颜色（Per Pixel Alpha Blend 【Alpha混合】）
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="opacity">透明度(0-255)</param>
        /// <exception cref="ApplicationException">像素格式必须为Format32bppArgb</exception>
        public static void SetBitmap(Bitmap bitmap, byte opacity, int left, int top, IntPtr handle)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBitmap = Win32.SelectObject(memDc, hBitmap);

                Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
                Win32.Point pointSource = new Win32.Point(0, 0);
                Win32.Point topPos = new Win32.Point(left, top);
                Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
                blend.BlendOp = Win32.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = Win32.AC_SRC_ALPHA;

                Win32.UpdateLayeredWindow(handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBitmap);
                    Win32.DeleteObject(hBitmap);
                }

                Win32.DeleteDC(memDc);
            }
        }
    }
    #endregion
    #region win32和gdi API的对颜色(透明通道)、图形处理一些封装
    internal class Win32
    {
        public enum Bool
        {
            False = 0,
            True
        };


        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(Int32 x, Int32 y) { this.x = x; this.y = y; }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 cx, Int32 cy) { this.cx = cx; this.cy = cy; }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }


        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;


        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

    }
    #endregion
}
