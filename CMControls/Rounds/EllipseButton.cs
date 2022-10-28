using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMControls.Rounds
{
    /// <summary>
    /// 椭圆或圆形按钮（长宽相同时为圆形）
    /// </summary>
    public class EllipseButton : Button
    {
        private Color ellipseBackColor = Color.FromArgb(51, 161, 224);
        /// <summary>
        /// 椭圆或圆形按钮的背景色
        /// </summary>
        [Description("椭圆或圆形按钮的背景色")]
        public Color EllipseBackColor
        {
            get => ellipseBackColor;
            set
            {
                ellipseBackColor = value;
                Invalidate();
            }
        }
        [Browsable(false), Description("外层的背景设置为透明，不可见、不能修改")]
        public override Color BackColor
        {
            get { return base.BackColor; }
        }
        public EllipseButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.CheckedBackColor = Color.Transparent;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(ClientRectangle);
                path.CloseFigure(); // 关不关闭路径均可
                var g = e.Graphics;
                
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear; 
                    g.FillPath(new SolidBrush(EllipseBackColor), path);
                g.DrawText(ClientRectangle, Text, ForeColor, Font);                
            }

            //g.DrawEllipse(new Pen(Color.Black, 2), 2, 2, Width - 6, Height - 6);
            //var mybit = new Bitmap("bai.png");
            //mybit.MakeTransparent(Color.White);


            #region 原来 创建Region的代码 又有DrawEllipse 又有AddEllipse，混乱
            //base.OnPaint(e);//递归  每次重新都发生此方法,保证其形状为自定义形状
            //Drawing.Drawing2D.GraphicsPath path = new Drawing.Drawing2D.GraphicsPath();
            //path.AddEllipse(2, 2, this.Width - 6, this.Height - 6);
            //Graphics g = e.Graphics;
            //g.DrawEllipse(new Pen(Color.Black, 2), 2, 2, Width - 6, Height - 6);
            //Region = new Region(path);
            #endregion
        }
    }
}
