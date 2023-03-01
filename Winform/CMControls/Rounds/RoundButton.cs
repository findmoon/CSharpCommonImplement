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
    [Obsolete("推荐使用ButtonPro控件")]
    public class RoundButton : Button
    {
        private int _radius;//半径 

        private Color _borderColor = Color.Empty;//边框颜色
        private int _borderSize = 0;//边框宽度

        private Color _hoverColor = Color.FromArgb(220, 80, 80);//基颜色
        private Color _normalColor = Color.FromArgb(51, 161, 224);//基颜色
        private Color _pressedColor = Color.FromArgb(251, 161, 0);//基颜色

        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;

        public override ContentAlignment TextAlign
        {
            set
            {
                _textAlign = value;
                this.Invalidate();
            }
            get
            {
                return _textAlign;
            }
        }

        /// <summary>
        /// 圆形按钮的半径属性
        /// </summary>
        [CategoryAttribute("高级"), BrowsableAttribute(true), ReadOnlyAttribute(false)]
        public int Radius
        {
            set
            {
                _radius = value;
                // 使控件的整个画面无效并重绘控件
                this.Invalidate();
            }
            get
            {
                return _radius;
            }
        }

        [CategoryAttribute("高级"), DefaultValue(0)]
        public int BorderSize
        {
            set
            {
                _borderSize = value;
                // 使控件的整个画面无效并重绘控件
                this.Invalidate();
            }
            get
            {
                return _borderSize;
            }
        }

        [CategoryAttribute("高级")]
        public Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                this._borderColor = value;
                this.Invalidate();
            }
        }
        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "51, 161, 224")]
        public Color NormalColor
        {
            get
            {
                return this._normalColor;
            }
            set
            {
                this._normalColor = value;
                this.Invalidate();
            }
        }
        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "220, 80, 80")]
        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
            set
            {
                this._hoverColor = value;
                this.Invalidate();
            }
        }

        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "251, 161, 0")]
        public Color PressedColor
        {
            get
            {
                return this._pressedColor;
            }
            set
            {
                this._pressedColor = value;
                this.Invalidate();
            }
        }

        public MouseControlState ControlState { get; set; }

        protected override void OnMouseEnter(EventArgs e)//鼠标进入时
        {
            ControlState = MouseControlState.Hover;//Hover
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)//鼠标离开
        {
            ControlState = MouseControlState.Normal;//正常
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)//鼠标按下
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)//鼠标左键且点击次数为1
            {
                ControlState = MouseControlState.Pressed;//按下的状态
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)//鼠标弹起
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))//控件区域包含鼠标的位置
                {
                    ControlState = MouseControlState.Hover;
                }
                else
                {
                    ControlState = MouseControlState.Normal;
                }
            }
            base.OnMouseUp(e);
        }
        public RoundButton()
        {
            ForeColor = Color.White;
            Radius = 20;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.ControlState = MouseControlState.Normal;
            this.SetStyle(
             ControlStyles.UserPaint |  //控件自行绘制，而不使用操作系统的绘制
             ControlStyles.AllPaintingInWmPaint | //忽略背景擦除的Windows消息，减少闪烁，只有UserPaint设为true时才能使用。
             ControlStyles.OptimizedDoubleBuffer |//在缓冲区上绘制，不直接绘制到屏幕上，减少闪烁。
             ControlStyles.ResizeRedraw | //控件大小发生变化时，重绘。                  
             ControlStyles.SupportsTransparentBackColor, //支持透明背景颜色
             true);
            BackColor = Color.Transparent;
        }


        //重写OnPaint
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            //base.OnPaintBackground(e);
   
            // 尽可能高质量绘制
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            var path = rect.GetRoundedRectPath(_radius);


            #region 外层圆角矩形
            //Rectangle outRect = new Rectangle(0, 0, this.Width, this.Height);
            //var outPath = outRect.GetRoundedRectPath(_radius);

            //Rectangle rect = new Rectangle(_borderSize, _borderSize, this.Width - _borderSize*2, this.Height - _borderSize*2);
            //var path = rect.GetRoundedRectPath(_radius);

            //// 必须正确指定外层路径outPath的全部区域，否则无法显示完全填充的全部
            //this.Region = new Region(outPath);

            //using (SolidBrush borderB = new SolidBrush(_borderColor))
            //{
            //    e.Graphics.FillPath(borderB, outPath);
            //}
            #endregion

            this.Region = new Region(path);

            Color baseColor;
            //Color borderColor;
            //Color innerBorderColor = this._baseColor;//Color.FromArgb(200, 255, 255, 255); ;

            switch (ControlState)
            {
                case MouseControlState.Hover:
                    baseColor = this._hoverColor;
                    break;
                case MouseControlState.Pressed:
                    baseColor = this._pressedColor;
                    break;
                case MouseControlState.Normal:
                    baseColor = this._normalColor;
                    break;
                default:
                    baseColor = this._normalColor;
                    break;
            }
            
            using (SolidBrush b = new SolidBrush(baseColor))
            {
                e.Graphics.FillPath(b, path); // 填充路径，而不是DrawPath 
            }
            // 绘制边框
            using (Pen pen = new Pen(_borderColor,_borderSize*2))
            {
                e.Graphics.DrawPath(pen, path);
                // 绘制路径上，会有一半位于路径外层，即Region外面，无法显示出来。因此设置为双borderSize

                //e.Graphics.DrawLine(pen, rect.X + _borderSize, rect.Y + _borderSize, rect.X + _borderSize + rect.Width - _borderSize * 2, rect.Y + _borderSize + rect.Height - _borderSize * 2);
            }
            using (Brush brush = new SolidBrush(this.ForeColor))
            {
                // 文本布局对象
                using (StringFormat strF = new StringFormat())
                {
                    // 文字布局
                    switch (_textAlign)
                    {
                        case ContentAlignment.TopLeft:
                            strF.Alignment = StringAlignment.Near;
                            strF.LineAlignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.TopCenter:
                            strF.Alignment = StringAlignment.Center;
                            strF.LineAlignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.TopRight:
                            strF.Alignment = StringAlignment.Far;
                            strF.LineAlignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.MiddleLeft:
                            strF.Alignment = StringAlignment.Near;
                            strF.LineAlignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.MiddleCenter:
                            strF.Alignment = StringAlignment.Center; //居中
                            strF.LineAlignment = StringAlignment.Center;//垂直居中
                            break;
                        case ContentAlignment.MiddleRight:
                            strF.Alignment = StringAlignment.Far;
                            strF.LineAlignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.BottomLeft:
                            strF.Alignment = StringAlignment.Near;
                            strF.LineAlignment = StringAlignment.Far;
                            break;
                        case ContentAlignment.BottomCenter:
                            strF.Alignment = StringAlignment.Center;
                            strF.LineAlignment = StringAlignment.Far;
                            break;
                        case ContentAlignment.BottomRight:
                            strF.Alignment = StringAlignment.Far;
                            strF.LineAlignment = StringAlignment.Far;
                            break;
                        default:
                            strF.Alignment = StringAlignment.Center; //居中
                            strF.LineAlignment = StringAlignment.Center;//垂直居中
                            break;
                    }
                    
                    //if (this.RightToLeft== RightToLeft.Yes)
                    //{
                    //    strF.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    //}   
                    //Font fo = new Font("宋体", 10.5F);
                    //e.Graphics.DrawString(this.Text, fo, brush, rect, strF);
                    //额外处理下文字区域，防止落在边框
                    e.Graphics.DrawString(this.Text, this.Font, brush, new Rectangle(rect.X+_borderSize,rect.Y+_borderSize, rect.Width-_borderSize*2,rect.Height-_borderSize*2), strF);
                    //e.Graphics.DrawString(this.Text, this.Font, brush, rect, strF);
                }
            }
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }


        #region 无用
        //private Color GetColor(Color colorBase, int a, int r, int g, int b)
        //{
        //    int a0 = colorBase.A;
        //    int r0 = colorBase.R;
        //    int g0 = colorBase.G;
        //    int b0 = colorBase.B;
        //    if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
        //    if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
        //    if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
        //    if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }

        //    return Color.FromArgb(a, r, g, b);
        //} 
        #endregion

    }
}