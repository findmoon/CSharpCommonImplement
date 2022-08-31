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

namespace GDIForArc
{
    public partial class Form1 : Form
    {
        Color RedColor = Color.Red;
        Color BalckColor = Color.Black;
        Color GreenColor = Color.Green;
        Color HotPink = Color.HotPink;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pn = new Pen(Color.Blue,5);
            Rectangle rect = new Rectangle(50, 50, 150, 150);
            g.DrawRectangle(pn, rect);

            Rectangle rect2 = new Rectangle(300, 50, 150, 350);
            g.DrawRectangle(pn, rect2);

            // 红色 0-90deg的圆弧
            pn.Color = RedColor;
            g.DrawArc(pn, rect, 0, 90);

            g.DrawArc(pn, rect2, 0, 90);

            // 黑色 90-180deg的圆弧
            pn.Color = BalckColor;
            g.DrawArc(pn, rect, 90, 90);

            g.DrawArc(pn, rect2, 90, 90);

            // 绿色 180-270deg的圆弧
            pn.Color = GreenColor;
            g.DrawArc(pn, rect, 180, 90);

            g.DrawArc(pn, rect2, 180, 90);

            // 粉色 270-360deg的圆弧
            pn.Color = HotPink;
            g.DrawArc(pn, rect, 270, 90);

            g.DrawArc(pn, rect2, 270, 90);

            #region 绘制圆角矩形
            pn.Color = Color.MediumVioletRed;

            // 指定连接处的连接点
            pn.LineJoin = LineJoin.Round;

            Rectangle roundRect = new Rectangle(500, 50, 150, 80);
            var radius = 20;
            var R = radius * 2;
            Rectangle arcRect = new Rectangle(roundRect.X, roundRect.Y, R, R);


            // 左上角
            g.DrawArc(pn, arcRect, 180, 90);
            // 右上角
            arcRect.X = roundRect.Right - R;
            g.DrawArc(pn, arcRect, 270, 90);
            // 右下角
            arcRect.Y = roundRect.Bottom - R;
            g.DrawArc(pn, arcRect, 0, 90);
            // 左下角
            arcRect.X = roundRect.Left;
            g.DrawArc(pn, arcRect, 90, 90);

            // 用曲线闭合点
            //g.DrawClosedCurve(pn, new Point[] {
            //    new Point(roundRect.X + radius, roundRect.Y),
            //    new Point(roundRect.Right - radius, roundRect.Y),
            //    new Point(roundRect.Right, roundRect.Y + radius),
            //    new Point(roundRect.Right, roundRect.Bottom - radius),
            //    new Point(roundRect.Right - radius, roundRect.Bottom),
            //    new Point(roundRect.Left + radius, roundRect.Bottom),
            //    new Point(roundRect.Left, roundRect.Bottom - radius),
            //    new Point(roundRect.Left, roundRect.Y + radius),
            //});

            //g.DrawLines(pn, new Point[] {
            //    new Point(roundRect.X + radius, roundRect.Y),
            //    new Point(roundRect.Right - radius, roundRect.Y),
            //});
            //g.DrawLines(pn, new Point[] {

            //    new Point(roundRect.Right, roundRect.Y + radius),
            //    new Point(roundRect.Right, roundRect.Bottom - radius),
            //});
            //g.DrawLines(pn, new Point[] {
            //    new Point(roundRect.Right - radius, roundRect.Bottom),
            //    new Point(roundRect.Left + radius, roundRect.Bottom),
            //});
            //g.DrawLines(pn, new Point[] {
            //    new Point(roundRect.Left, roundRect.Bottom - radius),
            //    new Point(roundRect.Left, roundRect.Y + radius),
            //});

            #region 单独绘制线条，需要处理1像素间隔问题，且连接处不平滑
            //g.DrawLine(pn, roundRect.X + radius, roundRect.Y, roundRect.Right - radius + 1, roundRect.Y);

            //g.DrawLine(pn, roundRect.Right, roundRect.Y + radius, roundRect.Right, roundRect.Bottom - radius + 1);

            //g.DrawLine(pn, roundRect.Right - radius + 1, roundRect.Bottom, roundRect.Left + radius, roundRect.Bottom);

            //g.DrawLine(pn, roundRect.Left, roundRect.Bottom - radius + 1, roundRect.Left, roundRect.Y + radius);

            /////---
            ////g.DrawLine(pn, roundRect.X + radius, roundRect.Y, roundRect.Right - radius, roundRect.Y);

            ////g.DrawLine(pn, roundRect.Right, roundRect.Y + radius, roundRect.Right, roundRect.Bottom - radius);

            ////g.DrawLine(pn, roundRect.Right - radius, roundRect.Bottom, roundRect.Left + radius, roundRect.Bottom);

            ////g.DrawLine(pn, roundRect.Left, roundRect.Bottom - radius, roundRect.Left, roundRect.Y + radius);
            #endregion

            #endregion
            // --------------------------------------------------

            #region 等比缩放的绘制圆角矩形
            //// 等比缩放，绘制内层圆角矩形
            var scale = 0.8f;
            var radiusScale = radius * scale;
            var RScale = radiusScale * 2;
            var innerRoundRect = new RectangleF(roundRect.X + (roundRect.Width - roundRect.Width * scale) / 2, roundRect.Y + (roundRect.Height - roundRect.Height * scale) / 2, roundRect.Width * scale, roundRect.Height * scale);
            var arcRectScale = new RectangleF(innerRoundRect.X, innerRoundRect.Y, RScale, RScale);

            //var scale = 0.8f;
            //var radiusScale = Convert.ToInt32(radius * scale);
            //var RScale = radiusScale * 2;
            //var width = Convert.ToInt32(roundRect.Width * scale);
            //var height = Convert.ToInt32(roundRect.Height * scale);
            //var innerRoundRect = new Rectangle(roundRect.X + (roundRect.Width - width) / 2, roundRect.Y + (roundRect.Height - height) / 2, width, height);
            //var arcRectScale = new Rectangle(innerRoundRect.X, innerRoundRect.Y, RScale, RScale);

            pn.Color = Color.MediumPurple;
            arcRectScale.X = innerRoundRect.X;
            arcRectScale.Y = innerRoundRect.Y;

            // 左上角
            g.DrawArc(pn, arcRectScale, 180, 90);
            g.DrawLine(pn, innerRoundRect.X + radiusScale, innerRoundRect.Y, innerRoundRect.Right - radiusScale + 1, innerRoundRect.Y);
            // 右上角
            arcRectScale.X = innerRoundRect.Right - RScale;
            g.DrawArc(pn, arcRectScale, 270, 90);
            g.DrawLine(pn, innerRoundRect.Right, innerRoundRect.Y + radiusScale, innerRoundRect.Right, innerRoundRect.Bottom - radiusScale + 1);
            // 右下角
            arcRectScale.Y = innerRoundRect.Bottom - RScale;
            g.DrawArc(pn, arcRectScale, 0, 90);
            g.DrawLine(pn, innerRoundRect.Right - radiusScale + 1, innerRoundRect.Bottom, innerRoundRect.Left + radiusScale, innerRoundRect.Bottom);
            // 左下角
            arcRectScale.X = innerRoundRect.Left;
            g.DrawArc(pn, arcRectScale, 90, 90);
            g.DrawLine(pn, innerRoundRect.Left, innerRoundRect.Bottom - radiusScale + 1, innerRoundRect.Left, innerRoundRect.Y + radiusScale);
            #endregion

            #region MyRegion
            var inflateRect = Rectangle.Inflate(roundRect, -8, -8);
            // inflateRect.Inflate(-8, -8); // 实例方法，长宽缩小或放大指定的量，改变的实例本身
            #endregion
        }
    }
}
