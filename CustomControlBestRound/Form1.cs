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

namespace CustomControlBestRound
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //// 需要在无边框下实现
            //this.FormBorderStyle = FormBorderStyle.None;
            //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // button1 需处理为无边框
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            //button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            //button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatAppearance.CheckedBackColor = Color.Transparent;

            // 2
            button1.Paint += Button_Paint;
            panel1.Paint += Panel_Paint; ;

            label1.Paint += Label1_Paint;
            //// 不能直接赋值处理，会导致显示的Region固定，应该是可以变化的（发生重绘时），若大小、位置不会改变，也可这么处理
            //button1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button1.Width, button1.Height, 30, 30));
            //panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 30, 30));
            //panel1.BackColor = Color.Red;
        }

        private void Label1_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Label)sender;
            pnl.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, pnl.Width, pnl.Height, 30, 30));
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            pnl.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, pnl.Width, pnl.Height, 30, 30));
        }

        private void Button_Paint(object sender, PaintEventArgs e)
        {
            var btn = (Button)sender;
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 30, 30));
      
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
                int nLeftRect,     // x-coordinate of upper-left corner
                int nTopRect,      // y-coordinate of upper-left corner
                int nRightRect,    // x-coordinate of lower-right corner
                int nBottomRect,   // y-coordinate of lower-right corner
                int nWidthEllipse, // height of ellipse
                int nHeightEllipse // width of ellipse
            );

        // 绘制圆球按钮
        //public bool DrawSphere(long hdc, long lColor, long X, long Y, long Width, long Height, bool bDrawShadow = true, long lAlpha = 100)
        //{
        //    long hGraphics;
        //    long hBrush;
        //    long mPath;
        //    Rectangle mRect;
        //    long[] col = new long[3];
        //    float[] pos = new float[3];

        //    // crea un grafico a partir de un hdc
        //    if (GdipCreateFromHDC(hdc, hGraphics) == 0)
        //    {

        //        // aplica el modo antialias
        //        GdipSetSmoothingMode(hGraphics, SmoothingModeAntiAlias);


        //        // ----------------------------- Shadow -------------------------------------
        //        if (bDrawShadow)
        //        {
        //            GdipCreatePath(0x0, mPath);                                                         // Crea un Path
        //            GdipAddPathEllipseI(mPath, X, Y + Height / 1.1, Width, Height / (double)4); GdipCreatePathGradientFromPath(mPath, hBrush);
        //            GdipSetPathGradientCenterColor(hBrush, ConvertColor(lColor, lAlpha / (double)3)); GdipSetPathGradientSurroundColorsWithCount(hBrush, 0, 1);
        //            GdipFillEllipseI(hGraphics, hBrush, X, Y + Height / 1.1, Width, Height / (double)4);        // dibuja la sombra en el grafico

        //            GdipDeleteBrush(hBrush);                                                            // Descarga la brocha
        //            GdipDeletePath(mPath);                                                              // Descarga el Path
        //        }

        //        // ----------------------------- Sphere -------------------------------------

        //        GdipCreatePath(0x0, mPath);                                                              // Crea un Path

        //        GdipAddPathEllipseI(mPath, X - (Width / 1.75), Y - Height / (double)2, Width * 2, Height * 2); GdipCreatePathGradientFromPath(mPath, hBrush); GdipSetPathGradientCenterColor(hBrush, ConvertColor(lColor, lAlpha)); GdipSetPathGradientSurroundColorsWithCount(hBrush, ConvertColor(ShiftColor(lColor, vbBlack, 100), lAlpha), 1);
        //        GdipFillEllipseI(hGraphics, hBrush, X, Y, Width, Height);                                // Dibuja un circulo en el grafico
        //        GdipDeleteBrush(hBrush);                                                                 // Descarga la brocha
        //        GdipDeletePath(mPath);                                                                   // Descarga el Path

        //        // ----------------------------- Light -------------------------------------

        //        mRect.Left = X + Width / (double)10;
        //        mRect.Top = Y + Height / (double)50;
        //        mRect.Width = Width - Width / (double)5;
        //        mRect.Height = Height / 1.5;

        //        GdipCreateLineBrushFromRectI(mRect, 0, 0, LinearGradientModeVertical, WrapModeTileFlipy, hBrush);
        //        col[0] = ConvertColor(vbWhite, lAlpha / 1.25);           // Primer color
        //        col[1] = 0;                                              // segundo color transparente
        //        col[2] = 0;                                              // tercer color transparente

        //        pos[0] = 0;
        //        pos[1] = 0.6;                                            // El 60% de la brocha va a ser transparente
        //        pos[2] = 1;

        //        GdipSetLinePresetBlend(hBrush, col[0], pos[0], 3);  // Asigna los valores para la brocha
        //        GdipFillEllipseI(hGraphics, hBrush, mRect.Left, mRect.Top, mRect.Width, mRect.Height - 1); // dibuja un circulo aplastado semi transparente
        //        GdipDeleteBrush(hBrush);                            // Elimina la brocha

        //        // ------------------------------------------------------------------------

        //        GdipDeleteGraphics(hGraphics);                       // Elimina el grafico.
        //    }
        //}

    }

}
