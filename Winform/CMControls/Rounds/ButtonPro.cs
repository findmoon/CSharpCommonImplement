using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CMControls
{
    public class ButtonPro : Button
    {
        private int roundRadius;//圆角半径 

        private bool showCusp = false;//显示尖角
        private RectangleAlign cuspAlign = RectangleAlign.RightTop;//三角尖角位置

        private Color roundBorderColor = Color.Black;//边框颜色
        private int roundBorderSize = 0;//边框宽度

        private Color roundHoverColor = Color.FromArgb(220, 80, 80);//鼠标位于控件时颜色
        private Color roundNormalColor = Color.FromArgb(51, 161, 224);//基颜色
        private Color roundPressedColor = Color.FromArgb(251, 161, 0);//鼠标按下控件时基颜色

        // 鼠标相对控件的状态位置，对应上面不同颜色
        private MouseControlState mouseControlState = MouseControlState.Normal;

        private bool regionNewModel = false; // 创建新Region的模式，使用"绘制范围"创建新的Region，实现控件区域贴合绘制范围，实现图形外的部分"正确的透明"，但相对会有些锯齿

        private Color beginBGColor= Color.FromArgb(90, 143, 0);//渐变开始色
        private Color endBGColor= Color.FromArgb(41, 67, 0);//渐变结束色
        private bool enableBGGradient = false; //使用渐变色
        private LinearGradientMode gradientModel = LinearGradientMode.Vertical; //线性渐变的模式

        private Region originRegion;

        /// <summary>
        /// 圆角按钮的半径属性
        /// </summary>
        [CategoryAttribute("高级"), DefaultValue(20), Description("圆角半径，>=0时启用圆角按钮，等于0为直角(但可使用背景色等所有Round圆角相关属性)，<0时使用默认Button样式")]
        public int RoundRadius
        {
            set
            {
                roundRadius = value;
                if (value < 0 )
                {
                    this.FlatStyle = FlatStyle.Flat;
                    this.FlatAppearance.BorderSize = 0;

                    FlatAppearance.MouseDownBackColor = Color.Transparent;
                    FlatAppearance.MouseOverBackColor = Color.Transparent;
                    FlatAppearance.CheckedBackColor = Color.Transparent;
                }
                // 使控件的整个画面无效并重绘控件
                this.Invalidate();
            }
            get
            {
                return roundRadius;
            }
        }
        /// <summary>
        /// 是否启用创建新Region的模式
        /// </summary>
        [CategoryAttribute("高级"), DefaultValue(false), Description("创建新Region的模式，使用'绘制范围'创建新的Region，实现控件区域贴合绘制范围，实现'正确的透明'，但相对会有些的锯齿")]
        public bool RegionNewModel
        {
            set
            {
                regionNewModel = value;
                this.Invalidate();
            }
            get
            {
                return regionNewModel;
            }
        }

        /// <summary>
        /// 三角尖角位置，当启用圆角
        /// </summary>
        [CategoryAttribute("高级"), Description("(三角)尖角的显示位置，当启用圆角按钮(RoundRadius>=0)，且显示尖角时有效"), DefaultValue(RectangleAlign.RightTop)]
        public RectangleAlign CuspAlign
        {
            set
            {
                cuspAlign = value;
                this.Invalidate();
            }
            get
            {
                return cuspAlign;
            }
        }
        [CategoryAttribute("高级"), Description("是否显示尖角，默认不显示，当启用Radius圆角(RoundRadius>=0)时才有效"), DefaultValue(false)]
        public bool ShowCusp
        {
            set
            {
                showCusp = value;
                this.Invalidate();
            }
            get
            {
                return showCusp;
            }
        }

        [CategoryAttribute("高级"), DefaultValue(0), Description("启用Radius圆角(RoundRadius>=0)时边框宽度，默认0")]
        public int RoundBorderSize
        {
            set
            {
                roundBorderSize = value;
                this.Invalidate();
            }
            get
            {
                return roundBorderSize;
            }
        }

        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "0, 0, 0"), Description("启用Radius圆角(RoundRadius>=0)时边框颜色，默认黑色")]
        public Color RoundBorderColor
        {
            get
            {
                return this.roundBorderColor;
            }
            set
            {
                this.roundBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否启用背景渐变色，启用后RoundNormalColor、RoundHoverColor、RoundPressedColor颜色无效
        /// </summary>
        [CategoryAttribute("高级"), DefaultValue(false), Description("启用渐变背景色(需要RoundRadius>=0)，启用后RoundNormalColor、RoundHoverColor、RoundPressedColor颜色无效")]
        public bool EnableBGGradient
        {
            get
            {
                return this.enableBGGradient;
            }
            set
            {
                this.enableBGGradient = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 线性渐变的模式，默认垂直渐变
        /// </summary>
        [CategoryAttribute("高级"), DefaultValue(LinearGradientMode.Vertical), Description("线性渐变的模式，默认垂直渐变")]
        public LinearGradientMode GradientModel
        {
            get
            {
                return this.gradientModel;
            }
            set
            {
                this.gradientModel = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 背景渐变色
        /// </summary>
        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "90, 143, 0"), Description("渐变开始色")]
        public Color BGColorBegin
        {
            get
            {
                return this.beginBGColor;
            }
            set
            {
                this.beginBGColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 背景渐变色
        /// </summary>
        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "41, 67, 0"), Description("渐变结束色")]
        public Color BGColorEnd
        {
            get
            {
                return this.endBGColor;
            }
            set
            {
                this.endBGColor = value;
                this.Invalidate();
            }
        }



        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "51, 161, 224"), Description("启用Radius圆角(RoundRadius>=0)时按钮标准颜色")]
        public Color RoundNormalColor
        {
            get
            {
                return this.roundNormalColor;
            }
            set
            {
                this.roundNormalColor = value;
                this.Invalidate();
            }
        }
        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "220, 80, 80"), Description("启用Radius圆角(RoundRadius>=0)鼠标位于按钮上时的按钮颜色")]
        public Color RoundHoverColor
        {
            get
            {
                return this.roundHoverColor;
            }
            set
            {
                this.roundHoverColor = value;
                this.Invalidate();
            }
        }

        [CategoryAttribute("高级"), DefaultValue(typeof(Color), "251, 161, 0"), Description("启用Radius圆角(RoundRadius>=0)鼠标按下时的按钮颜色")]
        public Color RoundPressedColor
        {
            get
            {
                return this.roundPressedColor;
            }
            set
            {
                this.roundPressedColor = value;
                this.Invalidate();
            }
        }

        [Browsable(false), Description("外层的背景设置为透明，不可见、不能修改")]
        public override Color BackColor
        {
            get { return base.BackColor; }
        }

        protected override void OnMouseEnter(EventArgs e)//鼠标进入时
        {
            mouseControlState = MouseControlState.Hover;//Hover
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)//鼠标离开
        {
            mouseControlState = MouseControlState.Normal;//正常
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)//鼠标按下
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)//鼠标左键且点击次数为1
            {
                mouseControlState = MouseControlState.Pressed;//按下的状态
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)//鼠标弹起
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))//控件区域包含鼠标的位置
                {
                    mouseControlState = MouseControlState.Hover;
                }
                else
                {
                    mouseControlState = MouseControlState.Normal;
                }
            }
            base.OnMouseUp(e);
        }
        public ButtonPro()
        {
            ForeColor = Color.White;

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;

            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.CheckedBackColor = Color.Transparent;
        
            BackColor = Color.Transparent;

            RoundRadius = 20; // 似乎当值为默认20时重新生成设计器或者重新打开项目后，此属性就会变为0，必须在构造函数中指定20来解决

            this.mouseControlState = MouseControlState.Normal;

            // 原始Region
            originRegion = Region;
        }

        public override void NotifyDefault(bool value)
        {
            base.NotifyDefault(false); // 去除窗体失去焦点时最新激活的按钮边框外观样式
        }

        #region //重写OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //base.OnPaintBackground(e);

            // 不能使用 e.ClipRectangle.GetRoundedRectPath(_radius) 计算控件全部的Region区域，e.ClipRectangle 似乎是变化的，必须使用固定的Width和Height，包括下面的绘制也不能使用e.ClipRectangle
            // 在Paint事件中也不推荐使用e.ClipRectangle时没问题的
            Rectangle controlRect = new Rectangle(0, 0, this.Width, this.Height);

            // roundRadius 修改回来是要还原
            if (roundRadius >= 0 && regionNewModel) // 圆角下创建新Region模式，使用自定义Region
            {
                using (var controlPath = controlRect.GetRoundedRectPath(roundRadius))
                {
                    // 要在绘制之前指定Region，否则无效
                    this.Region = new Region(controlPath);
                }

                //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, roundRadius, roundRadius)); //同样无法抗锯齿
            }
            else // 修改对应调整
            {
                //this.Region = new Region(controlRect);//也属于重新修改
                this.Region = originRegion;
            }

            if (roundRadius >= 0)
            {
                Rectangle rect;

                // e.Graphics 可以直接绘制Region，但是如果这样，就要分出完整的“真正的图形”作为Region区域，然后直接填充【这样又是否会消除锯齿，否则将会产生更多锯齿】

                if (enableBGGradient)
                {
                    //rect = e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle, roundRadius, beginBGColor,endBGColor, showCusp, CuspAlign, gradientModel);
                    if (roundBorderSize > 0)
                    {
                        using (var borderPen = new Pen(roundBorderColor, roundBorderSize))
                        {
                            //borderPen.Alignment = PenAlignment.Inset;
                            //rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, beginBGColor, endBGColor, showCusp, CuspAlign, gradientModel, borderPen);
                            rect = e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(controlRect.X + roundBorderSize / 2, controlRect.Y + roundBorderSize / 2, controlRect.Width - roundBorderSize, controlRect.Height - roundBorderSize), roundRadius, beginBGColor, endBGColor, showCusp, CuspAlign, gradientModel, borderPen);
                        }
                    }
                    else
                    {
                        rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, beginBGColor, endBGColor, showCusp, CuspAlign, gradientModel);
                    }
                }
                else
                {
                    Color baseColor;

                    switch (mouseControlState)
                    {
                        case MouseControlState.Hover:
                            baseColor = this.roundHoverColor;
                            break;
                        case MouseControlState.Pressed:
                            baseColor = this.roundPressedColor;
                            break;
                        case MouseControlState.Normal:
                            baseColor = this.roundNormalColor;
                            break;
                        default:
                            baseColor = this.roundNormalColor;
                            break;
                    }
                    //rect = e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle, roundRadius, baseColor, showCusp, cuspAlign);
                    if (roundBorderSize > 0)
                    {
                        using (var borderPen = new Pen(roundBorderColor, roundBorderSize))
                        {
                            //borderPen.Alignment = PenAlignment.Inset;
                            //rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, baseColor, showCusp, cuspAlign, borderPen);
                            rect = e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(controlRect.X + roundBorderSize / 2, controlRect.Y + roundBorderSize / 2, controlRect.Width - roundBorderSize, controlRect.Height - roundBorderSize), roundRadius, baseColor, showCusp, cuspAlign, borderPen);
                        }
                    }
                    else
                    {
                        rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, baseColor, showCusp, cuspAlign);
                    }
                }

                // 使用合适的区域
                e.Graphics.DrawText(rect, Text, ForeColor, Font, TextAlign);
            }
        }
        #endregion
        #region 直接重写OnPaintBackground，无效，使Button变为空白
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    //OnPaintBackground(e);
        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);

        //    // 不能使用 e.ClipRectangle.GetRoundedRectPath(_radius) 计算控件全部的Region区域，e.ClipRectangle 似乎是变化的，必须使用固定的Width和Height，包括下面的绘制也不能使用e.ClipRectangle
        //    // 在Paint事件中也不推荐使用e.ClipRectangle时没问题的
        //    Rectangle controlRect = new Rectangle(0, 0, this.Width, this.Height);

        //    // roundRadius 修改回来是要还原
        //    if (roundRadius >= 0 && regionNewModel) // 圆角下创建新Region模式，使用自定义Region
        //    {
        //        using (var controlPath = controlRect.GetRoundedRectPath(roundRadius))
        //        {
        //            // 要在绘制之前指定Region，否则无效
        //            this.Region = new Region(controlPath);
        //        }

        //        //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, roundRadius, roundRadius)); //同样无法抗锯齿
        //    }
        //    else // 修改对应调整
        //    {
        //        //this.Region = new Region(controlRect);//也属于重新修改
        //        this.Region = originRegion;
        //    }

        //    if (roundRadius >= 0)
        //    {
        //        Rectangle rect;

        //        // e.Graphics 可以直接绘制Region，但是如果这样，就要分出完整的“真正的图形”作为Region区域，然后直接填充【这样又是否会消除锯齿，否则将会产生更多锯齿】

        //        if (enableBGGradient)
        //        {
        //            //rect = e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle, roundRadius, beginBGColor,endBGColor, showCusp, CuspAlign, gradientModel);
        //            if (roundBorderSize > 0)
        //            {
        //                using (var borderPen = new Pen(roundBorderColor, roundBorderSize))
        //                {
        //                    //borderPen.Alignment = PenAlignment.Inset;
        //                    //rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, beginBGColor, endBGColor, showCusp, CuspAlign, gradientModel, borderPen);
        //                    rect = e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(controlRect.X + roundBorderSize / 2, controlRect.Y + roundBorderSize / 2, controlRect.Width - roundBorderSize, controlRect.Height - roundBorderSize), roundRadius, beginBGColor, endBGColor, showCusp, CuspAlign, gradientModel, borderPen);
        //                }
        //            }
        //            else
        //            {
        //                rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, beginBGColor, endBGColor, showCusp, CuspAlign, gradientModel);
        //            }
        //        }
        //        else
        //        {
        //            Color baseColor;

        //            switch (mouseControlState)
        //            {
        //                case MouseControlState.Hover:
        //                    baseColor = this.roundHoverColor;
        //                    break;
        //                case MouseControlState.Pressed:
        //                    baseColor = this.roundPressedColor;
        //                    break;
        //                case MouseControlState.Normal:
        //                    baseColor = this.roundNormalColor;
        //                    break;
        //                default:
        //                    baseColor = this.roundNormalColor;
        //                    break;
        //            }
        //            //rect = e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle, roundRadius, baseColor, showCusp, cuspAlign);
        //            if (roundBorderSize > 0)
        //            {
        //                using (var borderPen = new Pen(roundBorderColor, roundBorderSize))
        //                {
        //                    //borderPen.Alignment = PenAlignment.Inset;
        //                    //rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, baseColor, showCusp, cuspAlign, borderPen);
        //                    rect = e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(controlRect.X + roundBorderSize / 2, controlRect.Y + roundBorderSize / 2, controlRect.Width - roundBorderSize, controlRect.Height - roundBorderSize), roundRadius, baseColor, showCusp, cuspAlign, borderPen);
        //                }
        //            }
        //            else
        //            {
        //                rect = e.Graphics.DrawFillRoundRectAndCusp(controlRect, roundRadius, baseColor, showCusp, cuspAlign);
        //            }
        //        }
        //    }
        //} 
        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

    }
}