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
    /// 直接继承TextBox无法很好的进行重写
    /// </summary>
    [Obsolete("直接继承TextBox无法很好的进行重写")]
    public class RoundTextBoxNo : TextBox
    {
        private Color roundNormalColor = SystemColors.Window;
        private int roundBorderSize = 2;
        private Color roundBorderColor = Color.MediumSlateBlue;
        private bool underlinedStyle = false;
        private int roundRadius = 20;
        private Region originRegion;

        /// <summary>
        /// 圆角按钮的半径属性
        /// </summary>
        [Category("Layout"), DefaultValue(20), Description("圆角半径，>=0时启用圆角按钮，等于0为直角，<0时使用默认控件样式")]
        public int RoundRadius
        {
            get { return roundRadius; }
            set
            {
                roundRadius = value;
                if (value < 0 && BackColor == Color.Transparent)
                {
                    BackColor = SystemColors.Window;
                }
                else if (value >= 0 && BackColor != Color.Transparent)
                {
                    BackColor = Color.Transparent;
                }
                this.Invalidate();//Redraw control
            }
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                //if (RoundRadius >= 0)
                //{
                //    base.BackColor = Color.Transparent;
                //}
                //else
                //{
                //    base.BackColor = value;
                //}
                base.BackColor = value;
            }
        }

        [CategoryAttribute("Appearance"), DefaultValue(typeof(Color), "SystemColors.Window"), Description("启用Radius圆角(RoundRadius>=0)时按钮标准颜色")]
        public Color RoundNormalColor
        {
            get { return roundNormalColor; }
            set
            {
                roundNormalColor = value;
                this.Invalidate();
            }
        }

        [CategoryAttribute("Appearance"), DefaultValue(0), Description("启用Radius圆角(RoundRadius>=0)时边框宽度，默认0")]
        public int RoundBorderSize
        {
            get { return roundBorderSize; }
            set
            {
                roundBorderSize = value;
                this.Invalidate();

            }
        }
        [CategoryAttribute("Appearance"), DefaultValue(typeof(Color), "Color.MediumSlateBlue"), Description("启用Radius圆角(RoundRadius>=0)时边框颜色")]
        public Color RoundBorderColor
        {
            get => roundBorderColor;
            set
            {
                roundBorderColor = value;
                this.Invalidate();
            }
        }

        [CategoryAttribute("Appearance"), DefaultValue(false), Description("圆角边框宽度>0时，是否启用Underline，默认false")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        public RoundTextBoxNo()
        {
            this.SetStyle(
             ControlStyles.UserPaint |  //控件自行绘制，而不使用操作系统的绘制
             ControlStyles.AllPaintingInWmPaint | //忽略背景擦除的Windows消息，减少闪烁，只有UserPaint设为true时才能使用。
             ControlStyles.OptimizedDoubleBuffer |//在缓冲区上绘制，不直接绘制到屏幕上，减少闪烁。
             ControlStyles.ResizeRedraw | //控件大小发生变化时，重绘。                  
             ControlStyles.SupportsTransparentBackColor, //支持透明背景颜色
             true);
            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.None;
            //AutoSize = false;
            originRegion = Region;

        }

        private int WM_PAINT = 0x000F;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            //if (m.Msg == WM_PAINT)
            //{
            //    if (RoundRadius >= 0)
            //    {
            //        var controlRect = ClientRectangle;
            //        using (Graphics g = this.CreateGraphics())
            //        {
            //            if (roundBorderSize > 0)
            //            {
            //                using (var borderPen = new Pen(roundBorderColor, roundBorderSize))
            //                {
            //                    if (underlinedStyle)
            //                    {
            //                        g.DrawLine(borderPen, new Point(0, this.Size.Height - 1), new Point(this.Size.Width, this.Size.Height - 1));
            //                    }
            //                    else
            //                    {
            //                        g.DrawFillRoundRectAndCusp(new Rectangle(controlRect.X + roundBorderSize / 2, controlRect.Y + roundBorderSize / 2, controlRect.Width - roundBorderSize, controlRect.Height - roundBorderSize), RoundRadius, roundNormalColor, borderPen: borderPen);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                g.DrawFillRoundRectAndCusp(controlRect, RoundRadius, roundNormalColor);
            //            }
            //        }
            //    }
            //}
        }

       

        //TextBox重写OnPaint会导致覆盖文本框，其需要启用ControlStyles.UserPaint
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //var controlRect = ClientRectangle;
            //Region = new Region(controlRect.GetRoundedRectPath(roundRadius));
            var controlRect = new Rectangle(0, 0, Width, Height);
            var rect = controlRect;
            if (RoundRadius >= 0)
            {
                if (roundBorderSize > 0)
                {
                    using (var borderPen = new Pen(roundBorderColor, roundBorderSize))
                    {
                        if (underlinedStyle) //Line Style
                        {
                            e.Graphics.DrawLine(borderPen, 0, this.Height - roundBorderSize / 2, this.Width, this.Height - roundBorderSize / 2);
                        }
                        else
                        {
                            //borderPen.Alignment = PenAlignment.Inset;
                            rect = e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(controlRect.X + roundBorderSize / 2, controlRect.Y + roundBorderSize / 2, controlRect.Width - roundBorderSize, controlRect.Height - roundBorderSize), RoundRadius, roundNormalColor, borderPen: borderPen);
                        }
                    }
                }
                else
                {
                    rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, RoundRadius, roundNormalColor);
                }

            }

            //// 使用合适的区域绘制文本
            //var textAlign = ContentAlignment.TopLeft;
            //switch (TextAlign)
            //{
            //    case HorizontalAlignment.Right:
            //        textAlign = ContentAlignment.TopRight;
            //        break;
            //    case HorizontalAlignment.Center:
            //        textAlign = ContentAlignment.TopCenter;
            //        break;
            //    case HorizontalAlignment.Left:
            //    default:
            //        break;
            //}
            //e.Graphics.DrawText(rect, Text, Color.White, Font, textAlign);
        }
    }
}
