using CMControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{
    /// <summary>
    /// 设置窗体Form透明，在`OnPaint`中绘制，无论执行或不执行SetBitmap()设置透明通道，在圆角边缘处都会有白边出现。
    /// </summary>
    [Obsolete("窗体Form透明,OnPaint绘制圆角会有白边")]
    public partial class RoundFormSmoothTransparencyFormNo : Form
    {
        public RoundFormSmoothTransparencyFormNo()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            this.BackColor = Color.Empty;
            this.TransparencyKey = BackColor;

            //Paint += RoundFormSmooth_Paint;
        }
        //private void DrawForm(object pSender, EventArgs pE)
        //{
        //    using (Bitmap backImage = new Bitmap(this.Width, this.Height))
        //    {
        //        using (Graphics graphics = Graphics.FromImage(backImage))
        //        {
        //            Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        //            using (Brush b = new LinearGradientBrush(gradientRectangle, Color.DarkSlateBlue, Color.MediumPurple, 0.0f))
        //            {
        //                graphics.SmoothingMode = SmoothingMode.HighQuality;

        //                RoundedRectangle.FillRoundedRectangle(graphics, b, gradientRectangle, 35);

        //                foreach (Control ctrl in this.Controls)
        //                {
        //                    using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height))
        //                    {
        //                        Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
        //                        ctrl.DrawToBitmap(bmp, rect);
        //                        graphics.DrawImage(bmp, ctrl.Location);
        //                    }
        //                }

        //                PerPixelAlphaBlend.SetBitmap(backImage, Left, Top, Handle);
        //            }
        //        }
        //    }
        //}

        private void RoundFormSmooth_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics graphics = e.Graphics)
            {
                Rectangle gradientRectangle = new Rectangle(0, 0, this.Width, this.Height);
                //Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1); // 推荐 圆角曲线和直线的连接更丝滑

                using (Brush b = new LinearGradientBrush(gradientRectangle, Color.DarkSlateBlue, Color.MediumPurple, 0.0f))
                {
                    graphics.FillRoundRectangle(gradientRectangle, b, 25);
                }
                Bitmap myBitmap = new Bitmap(this.Width, this.Height);
                SetBitmap(myBitmap);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics graphics = e.Graphics)
            {
                Rectangle gradientRectangle = new Rectangle(0, 0, this.Width, this.Height);
                //Rectangle gradientRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1); // 推荐 圆角曲线和直线的连接更丝滑 似乎每太差别

                using (Brush b = new LinearGradientBrush(gradientRectangle, Color.DarkSlateBlue, Color.MediumPurple, 0.0f))
                {
                    graphics.FillRoundRectangle(gradientRectangle, b, 25);
                }
                Bitmap myBitmap = new Bitmap(this.Width, this.Height);
                SetBitmap(myBitmap);
            }
        }

        /// <summary>
        /// 设置每个像素Alpha透明通道颜色（Per Pixel Alpha Blend 【Alpha混合】）
        /// </summary>
        /// <param name="bitmap"></param>
        public void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap, 255);
        }
        /// <summary>
        /// 设置每个像素Alpha透明通道颜色（Per Pixel Alpha Blend 【Alpha混合】）
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="opacity">透明度(0-255)</param>
        /// <exception cref="ApplicationException">像素格式必须为Format32bppArgb</exception>
        public void SetBitmap(Bitmap bitmap, byte opacity)
        {
            PerPixelAlphaBlend.SetBitmap(bitmap, opacity, Left, Top, Handle);
            #region 原始 混合了窗体的Left, Top, Handle属性的代码
            //if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            //    throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");


            //IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            //IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            //IntPtr hBitmap = IntPtr.Zero;
            //IntPtr oldBitmap = IntPtr.Zero;

            //try
            //{
            //    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
            //    oldBitmap = Win32.SelectObject(memDc, hBitmap);

            //    Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
            //    Win32.Point pointSource = new Win32.Point(0, 0);
            //    Win32.Point topPos = new Win32.Point(Left, Top);
            //    Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
            //    blend.BlendOp = Win32.AC_SRC_OVER;
            //    blend.BlendFlags = 0;
            //    blend.SourceConstantAlpha = opacity;
            //    blend.AlphaFormat = Win32.AC_SRC_ALPHA;

            //    Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
            //}
            //finally
            //{
            //    Win32.ReleaseDC(IntPtr.Zero, screenDc);
            //    if (hBitmap != IntPtr.Zero)
            //    {
            //        Win32.SelectObject(memDc, oldBitmap);
            //        Win32.DeleteObject(hBitmap);
            //    }

            //    Win32.DeleteDC(memDc);
            //} 
            #endregion
        }


        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;

        //        cp.ExStyle |= 0x00080000;

        //        return cp;
        //    }
        //}
    }

    #region 透明(通道)颜色相关的Win32 API
    class Win32
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
