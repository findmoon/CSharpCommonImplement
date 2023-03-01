using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CMControls
{
    public class RoundPanel : Panel
    {
        private Color roundNormalColor = SystemColors.Control;
        private int roundBorderSize = 0;
        private Color roundBorderColor = Color.MediumSlateBlue;
        private bool underlinedStyle = false;
        private int roundRadius = 20;
        private RoundMode roundMode= RoundMode.All;

        /// <summary>
        /// 圆角Panel的半径属性
        /// </summary>
        [Category("高级"), DefaultValue(20), Description("圆角半径，>=0时启用圆角Panel，等于0为直角，<0时使用默认控件样式")]
        public virtual int RoundRadius
        {
            get { return roundRadius; }
            set
            {
                roundRadius = value;
                this.Invalidate();//Redraw control
            }
        }
        /// <summary>
        /// 圆角Panel的半径属性
        /// </summary>
        [Category("高级"), DefaultValue(RoundMode.All), Description("圆角模式，指定圆角的位置，默认ALL，四个角都为圆角")]
        public RoundMode RoundMode
        {
            get { return roundMode; }
            set
            {
                roundMode = value;
                this.Invalidate();//Redraw control
            }
        }
        
        [Category("Appearance"), Description("背景颜色，当RoundRadius>=0时取值为Color.Transparent，RoundRadius<0才可设置其他颜色")]
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                if (RoundRadius >= 0)
                {
                    base.BackColor = Color.Transparent;
                }
                else
                {
                    base.BackColor = value;
                }
            }
        }

        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "SystemColors.Control"), Description("启用Radius圆角(RoundRadius>=0)时背景标准颜色")]
        public Color RoundNormalColor
        {
            get { return roundNormalColor; }
            set
            {
                roundNormalColor = value;
                this.Invalidate();
            }
        }

        [CategoryAttribute("高级"), DefaultValue(0), Description("启用Radius圆角(RoundRadius>=0)时边框宽度，默认0")]
        public int RoundBorderSize
        {
            get { return roundBorderSize; }
            set
            {
                roundBorderSize = value;
                this.Invalidate();

            }
        }
        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "Color.MediumSlateBlue"), Description("启用Radius圆角(RoundRadius>=0)时边框颜色")]
        public Color RoundBorderColor
        {
            get => roundBorderColor;
            set
            {
                roundBorderColor = value;
                this.Invalidate();
            }
        }

        [CategoryAttribute("高级"), DefaultValue(false), Description("圆角边框宽度>0时，是否启用Underline，默认false")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        public RoundPanel()
        {
            BackColor = Color.Transparent;
        }

        //重写OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

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
                            e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(ClientRectangle.X + roundBorderSize / 2, ClientRectangle.Y + roundBorderSize / 2, ClientRectangle.Width - roundBorderSize, ClientRectangle.Height - roundBorderSize), RoundRadius, roundNormalColor, borderPen: borderPen,roundMode:roundMode);
                        }
                    }
                }
                else
                {
                    e.Graphics.DrawFillRoundRectAndCusp(ClientRectangle, RoundRadius, roundNormalColor, roundMode: roundMode);
                }
            }

        }
    }
}
