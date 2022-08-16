using CustomControlRound;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMControls
{
    /// <summary>
    ///通过 base.CreateParams.ExStyle |= 0x00080000; 扩展样式，启用 WS_EX_LAYERED ，相当于在原有的控件Region基础上重新绘制一个Layer层，原有窗体内容均被覆盖，需要获取其子控件并绘制到新的layer层   
    /// </summary>
    public class RoundedForm : Form
    {
        private Panel panel1;
        private Button button1;
        private Button button2;
        private MenuStrip menuStrip1;
        private TransparentsControl transparentsControl1;
        private ToolStripMenuItem 你好ToolStripMenuItem;
        //private Timer drawTimer = new Timer();

        public RoundedForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

            // 居中无效
            //StartPosition = FormStartPosition.CenterScreen;

            StartPosition = FormStartPosition.Manual;
            Location = new Point(200, 200);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                //drawTimer.Interval = 1000 / 60;
                //drawTimer.Tick += DrawForm;
                //drawTimer.Start();                
            }
            DrawForm(null, null);
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            DrawForm(null, null);
            base.OnResize(e);
        }

        private void DrawForm(object pSender, EventArgs pE)
        {
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
                    using (Brush b = new LinearGradientBrush(gradientRectangle, Color.DarkSlateBlue, Color.MediumPurple, 0.0f))
                    {
                        graphics.FillRoundRectangle(gradientRectangle,b, 35);

                        foreach (Control ctrl in this.Controls)
                        {
                            using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height, PixelFormat.Format32bppArgb))
                            {
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

                                ctrl.DrawToBitmap(bmp, ctrl.ClientRectangle); // 结合OnPaint中的绘制，能完美实现ctrl圆角的边角透明底层，原因Bitmap没有指定颜色，控件之外的部分透明
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

                        PerPixelAlphaBlend.SetBitmap(backImage, Left, Top, Handle);//不执行将无法显示
                        //backImage.MakeTransparent(Color.White);
                    }
                }
            }
        }

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

                using (Brush b = new LinearGradientBrush(gradientRectangle, Color.DarkSlateBlue, Color.MediumPurple, 0.0f))
                {
                    graphics.FillRoundRectangle(gradientRectangle, b, 35);
                };
            }
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000;
                return cp;
            }
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.transparentsControl1 = new CustomControlRound.TransparentsControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.你好ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.transparentsControl1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(73, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // transparentsControl1
            // 
            this.transparentsControl1.BackColor = System.Drawing.Color.Transparent;
            this.transparentsControl1.Location = new System.Drawing.Point(51, 62);
            this.transparentsControl1.Name = "transparentsControl1";
            this.transparentsControl1.Size = new System.Drawing.Size(75, 23);
            this.transparentsControl1.TabIndex = 1;
            this.transparentsControl1.Text = "transparentsControl1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(63, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(73, 223);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.你好ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(352, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 你好ToolStripMenuItem
            // 
            this.你好ToolStripMenuItem.Name = "你好ToolStripMenuItem";
            this.你好ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.你好ToolStripMenuItem.Text = "你好";
            // 
            // RoundedForm
            // 
            this.ClientSize = new System.Drawing.Size(352, 332);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RoundedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你好");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你好呀");
        }
    }



    internal static class PerPixelAlphaBlend
    {
        public static void SetBitmap(Bitmap bitmap, int left, int top, IntPtr handle)
        {
            SetBitmap(bitmap, 255, left, top, handle);
        }

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
}
