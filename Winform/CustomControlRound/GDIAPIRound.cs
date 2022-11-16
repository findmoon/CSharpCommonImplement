using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{

    public partial class GDIAPIRound : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern IntPtr BeginPath(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int SetBkMode(IntPtr hdc, int nBkMode);
        const int TRANSPARENT = 1;
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern IntPtr EndPath(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern IntPtr PathToRegion(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int Ellipse(IntPtr hdc, int x1, int y1, int x2, int y2);
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern IntPtr SetWindowRgn(IntPtr hwnd, IntPtr hRgn, bool bRedraw);
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        public GDIAPIRound()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            IntPtr dc;
            IntPtr region;

            dc = GetDC(this.Handle);
            BeginPath(dc);
            SetBkMode(dc, TRANSPARENT);
            Ellipse(dc, 0, 0, this.Width - 3, this.Height - 2);
            EndPath(dc);
            region = PathToRegion(dc);
            SetWindowRgn(this.Handle, region, false);
        }

    }
}
