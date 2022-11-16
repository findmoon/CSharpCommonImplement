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
    /// <summary>
    /// 阴影一定也不影，而且不能改变颜色
    /// </summary>
    public partial class ShadowForm : Form
    {
        public ShadowForm()
        {
            InitializeComponent();

            m_aeroEnabled = false;

            this.FormBorderStyle = FormBorderStyle.None;
        }

        //[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        //private static extern IntPtr CreateRoundRectRgn
        //(
        //    int nLeftRect, // x-coordinate of upper-left corner
        //    int nTopRect, // y-coordinate of upper-left corner
        //    int nRightRect, // x-coordinate of lower-right corner
        //    int nBottomRect, // y-coordinate of lower-right corner
        //    int nWidthEllipse, // height of ellipse
        //    int nHeightEllipse // width of ellipse
        // );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        const int HTLEFT = 10;   // variables for dragging resize the form
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 8,
                            leftWidth = 8,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
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

                if ((int)m.Result == HTCLIENT) // drag the form
                {
                    m.Result = (IntPtr)HTCAPTION;
                }
            }

        }

    }
}
