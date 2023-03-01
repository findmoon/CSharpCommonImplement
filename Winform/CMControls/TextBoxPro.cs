using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CMControls
{
    [DefaultEvent("TextChanged")]
    public partial class TextBoxPro : Panel
    {
        private TextBox textBox = new TextBox();

        //Default Event
        public new event EventHandler TextChanged; // public event EventHandler TextChanged; 除非显式new重写
        //TextBox-> TextChanged event
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (!isPlaceholder) // text保持同步
            {
                text = textBox.Text;
            }
            if (AutoScroll)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.ScrollBars = ScrollBars.None;
                }
                else
                {
                    //// 处理textBox的宽高，使Panel自动出现滚动条
                    //// 测量指定文本的宽高
                    var textSize = TextRenderer.MeasureText(textBox.Text, this.Font); // 无法正确的测量，尤其是在自动换行时，高度无法正确（有回车换行倒是测量正确）单行textSize.Width / textBox.Width 计算自动换行的行数
                                                                                      // 暂时无法直接处理TextBox的宽高，将导致对应父控件Panel的宽高受影响和发生不好控制的变换，要额外调整UpdateControlHeight对应的方法
                                                                                      //textBox.Width = textSize.Width;
                                                                                      //textBox.Height = textSize.Height;

                    // 多行模式下设置ScrollBars才生效

                    // 可以动态设置textBox是否显示滚动条
                    if (textSize.Width > textBox.Width && textSize.Height > textBox.Height)
                    {
                        // 避免重复赋值的闪烁
                        if (textBox.ScrollBars != ScrollBars.Both)
                        {
                            textBox.ScrollBars = ScrollBars.Both;

                        }
                    }
                    else if (textSize.Width > textBox.Width)
                    {
                        
                        var isVerticalBar = false;
                        if (WordWrap) // 如果开启了自动换行，则水平滚动条无效，需要添加垂直滚动条
                        {
                            var allTxtLine = 0;
                            foreach (var line in textBox.Lines)
                            {
                                var currSize = TextRenderer.MeasureText(line, Font);
                                allTxtLine += 1 + currSize.Width / textBox.Width;
                            }
                            allTxtLine -= 1; // -1似乎更符合高度计算
                            // [目前无法很好的获取TextBox行高？]PreferredHeight可以表示当前字体下的单行行高
                            if (textBox.PreferredHeight * allTxtLine > textBox.Height) //自动换行的高度高于实际
                            {
                                isVerticalBar = true;
                            }
                        }

                        if (isVerticalBar)
                        {
                            if (textBox.ScrollBars != ScrollBars.Both)
                            {
                                textBox.ScrollBars = ScrollBars.Both;
                            }
                        }
                        else
                        {
                            if (textBox.ScrollBars != ScrollBars.Horizontal)
                            {
                                textBox.ScrollBars = ScrollBars.Horizontal;

                            }
                        }

                    }
                    else if (textSize.Height > textBox.Height)
                    {
                        if (textBox.ScrollBars != ScrollBars.Vertical)
                        {
                            textBox.ScrollBars = ScrollBars.Vertical;
                        }
                    }
                    else
                    {
                        textBox.ScrollBars = ScrollBars.None;
                    }

                    if (textBox.ScrollBars != ScrollBars.None)
                    {
                        // 滚动条保持在最下面 内容不闪烁
                        textBox.SelectionStart = textBox.Text.Length;
                        textBox.SelectionLength = 0;
                        textBox.ScrollToCaret(); // 滚动到符号插入位置
                    }
                }
            }

            TextChanged?.Invoke(sender, e);
        }

        //Fields
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private bool isFocused = false;

        private int borderRadius = 10;
        private Color placeholderColor=Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholder = false;
        private char passwordChar = default(char);
        // 当前TextBoxPro的文本内容
        private string text;

        #region -> Properties
        [Category("高级"), DefaultValue(typeof(Color), "Color.MediumSlateBlue"), Description("文本边框的颜色，默认Color.MediumSlateBlue")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        [Category("高级"), DefaultValue(typeof(Color), "Color.HotPink"), Description("光标激活文本框时边框颜色，默认Color.HotPink")]
        public Color BorderFocusColor { get; set; } = Color.HotPink;

        [Category("高级"), DefaultValue(2), Description("Border边框大小，默认2，0表示无边框")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                if (value >= 0)
                {
                    borderSize = value;
                    this.Invalidate();
                }
            }
        }

        [Category("高级"), DefaultValue(false), Description("底部横线样式，横线大小为BorderSize大小，默认无")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        [Category("高级")]
        public char PasswordChar
        {
            get { return passwordChar; }
            set
            {                
                passwordChar = value;
                if (!isPlaceholder) // 非placeholder设置密码字符
                    textBox.PasswordChar = value;
            }
        }

        [Category("高级"), DefaultValue(false), Description("多行模式，默认为单行模式，无法直接调整宽高，单行模式下推荐通过设置FontSize字体大小实现高度调整，多行模式可自动调整宽高")]
        public bool Multiline
        {
            get { return textBox.Multiline; }
            set
            {
                textBox.Multiline = value;
                // 变更宽高度
                SetControlSize();
                Invalidate();
            }
        }
        [Category("高级"), DefaultValue(true), Description("指示多行文本框模式下是否自动换行到下一行的开始，自动换行时水平滚动条无效，默认自动换行")]
        public bool WordWrap
        {
            get { return textBox.WordWrap; }
            set { textBox.WordWrap = value; Invalidate(); }
        }

        /// <summary>
        /// 多行文本模式下的自动滚动条
        /// </summary>
        [Category("高级"), DefaultValue(true), Description("指示多行文本框模式下是否显示自动滚动条")]
        public bool MultilineAutoScroll
        {
            get { return AutoScroll; }
            set
            {
                AutoScroll = value;
                Invalidate();
            }
        }
        // 不可见，使用更贴切的MultilineAutoScroll
        [Browsable(false)]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set
            {
                base.AutoScroll = value;
            }
        }



        [Category("高级"), Description("文本框的背景颜色")]
        public Color TextBackColor
        {
            get { return textBox.BackColor; }
            set
            {
                textBox.BackColor = value;
            }
        }
        [Browsable(false), Description("外层的背景设置为透明，不可见、不能修改")]
        public override Color BackColor
        {
            get { return base.BackColor; }
        }
        [Browsable(false), Description("外层边框设置无，不可见、不能修改")]
        public new BorderStyle BorderStyle => base.BorderStyle;

        // 文本颜色
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                if (!isPlaceholder)
                {
                    textBox.ForeColor = value;
                }                
            }
        }

        // 文本字体
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                textBox.Font = value;
                //if (this.DesignMode)
                UpdateControlHeight();
            }
        }
        /// <summary>
        /// 文本框文本，文本框内的变更要同步更新到text(同步将会造成textBox.Text和PlaceholderText的混乱，或者在Textchanged中通过if (isPlaceholder)判断再同步)，【在变更textBox.Text到PlaceholderText之前备份也可以（会不实时）,Text未真正同步】
        /// </summary>
        [Browsable(true), Description("文本框文本")]
        public override string Text
        {
            get
            {
                //if (isPlaceholder) return ""; 
                //else return textBox.Text;
                // 文本是文本，Placeholder是Placeholder
                return text;
            }
            set
            {
                textBox.Text = value;
                text = value;
                RemovePlaceholder(); // 如果赋值内容则清除placeholder  先清除再设置
                SetPlaceholder(); // 如果赋值空则设置placeholder
            }
        }

        [Category("高级"), DefaultValue(10), Description("文本框的圆角，默认10，=0表示非圆角(直角)，不推荐设置为奇数，比如1")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    this.Invalidate();//Redraw control
                }
            }
        }

        [Category("高级"), DefaultValue(typeof(Color), "Color.DarkGray"), Description("Placeholder文本的颜色，默认Color.DarkGray")]
        public Color PlaceholderColor {
            get { return placeholderColor; }
            set
            {
                placeholderColor = value;

                SetPlaceholder();
            }
        }

        // 重新生成后在设计器中不显示Placeholder；启动后也不显示Placeholder；光标进入离开后可以显示Placeholder  SetPlaceholder() 均正常执行了
        // 重新生成后在设计器中不显示Placeholder；启动后也不显示Placeholder 的问题，可能是Text赋值为空导致PlaceholderText此处赋值的SetPlaceholder()被移除
        // Text属性中要添加SetPlaceholder()和RemovePlaceholder()两个处理
        [Category("高级"), Description("Placeholder提示文本")]
        public string PlaceholderText
        {
            get { return placeholderText; }
            set
            {
                placeholderText = value;
      
                SetPlaceholder();
            }
        }

        /// <summary>
        /// 处理设置Padding大小，添加BorderSize
        /// </summary>
        public new Padding Padding
        {
            get => new Padding(base.Padding.Left - borderSize, base.Padding.Top - borderSize, base.Padding.Right - borderSize, base.Padding.Bottom - borderSize);
            set
            {
                // 计算borderSize
                base.Padding = new Padding(value.Left + borderSize, value.Top + borderSize, value.Right + borderSize, value.Bottom + borderSize);
                // 设置正确的高度，要不显示时默认上下边距不一致（设计器初次拖拽到界面、代码初始化未调整时），改为使用函数
                //Height = textBox.Height + base.Padding.Top + base.Padding.Bottom;
                //Width = textBox.Width + base.Padding.Left + base.Padding.Right;
                SetControlSize();
            }
        }
        #endregion

        public TextBoxPro()
        {
            BackColor = Color.Transparent;

            base.BorderStyle = BorderStyle.None;
            Margin = new Padding(0);
            //// 设置自动滚动条，这是Panel的，和textBox滚动条不对应。在textBox.TextChange动态处理设置滚动条
            //AutoScroll = true;


            textBox.Parent = this;

            textBox.BorderStyle = BorderStyle.None;
            textBox.Dock = DockStyle.Fill;
            textBox.Enter += textBox_Enter;
            textBox.Leave += textBox_Leave;
            textBox.Click += textBox_Click;
            textBox.MouseEnter += textBox_MouseEnter;
            textBox.MouseLeave += textBox_MouseLeave;
            textBox.MouseHover += TextBox_MouseHover;
            textBox.MouseMove += TextBox_MouseMove;
            textBox.KeyPress += textBox_KeyPress;
            textBox.KeyDown += TextBox_KeyDown; ;
            textBox.KeyUp += TextBox_KeyUp;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;

            textBox.TextChanged += TextBox_TextChanged;            
            

            // 放在Dock设置之后Panel宽高
            // Padding属性中处理 结合borderSize设置宽高
            //Width = textBox.Width + 20 + borderSize * 2;
            //Height = textBox.Height + 6 + borderSize * 2;

            this.Padding = new Padding(10, 3, 10, 3 );           
        }

        #region Event handler
        private void textBox_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
            OnEnter(e);
        }
        private void textBox_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            SetPlaceholder();
            OnLeave(e);
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
        private void textBox_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }
        private void textBox_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }
        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void TextBox_MouseHover(object sender, EventArgs e)
        {
            OnMouseHover(e);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }
        
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }
        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }

        #endregion

        #region -> Overridden methods
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e); // OnResize 内如果宽高调整导致内存超出Stack Overflow，UpdateControlHeight只处理单行模式下高度
                              //if (this.DesignMode)
            UpdateControlHeight();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            ////抗锯齿 尽可能高质量绘制
            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.InterpolationMode = InterpolationMode.HighQualityBilinear;

            if (borderRadius > 0)//Rounded TextBox
            {
                //-Fields
                var rectBorderSmooth = this.ClientRectangle;
                var rect = new Rectangle(rectBorderSmooth.X + borderSize / 2, rectBorderSmooth.Y + borderSize / 2, rectBorderSmooth.Width - borderSize, rectBorderSmooth.Height - borderSize);
                // 由于用户控件不支持背景透明，绘制透明色的光滑区域
                int smoothSize = borderSize > 0 ? borderSize : 1;

                // 整个圆角路径
                using (GraphicsPath roundPath = rect.GetRoundedRectPath(borderRadius))
                {
                    // 填充圆角区域，然后在绘制边框
                    graph.FillPath(new SolidBrush(textBox.BackColor), roundPath);
                    if (borderSize > 0)
                    {
                        using (Pen penBorder = new Pen(borderColor, borderSize))
                        {
                            //-Drawing
                            if (isFocused) penBorder.Color = BorderFocusColor;


                            if (underlinedStyle) //Line Style
                            {
                                //Draw border
                                //graph.SmoothingMode = SmoothingMode.None;
                                graph.DrawLine(penBorder, 0, this.Height - borderSize / 2, this.Width, this.Height - borderSize / 2);
                            }
                            else //Normal Style
                            {
                                //Draw border
                                using (GraphicsPath pathBorder = rect.GetRoundedRectPath(borderRadius))
                                {
                                    graph.DrawPath(penBorder, pathBorder);
                                }
                            }
                        }
                    }
                }
            }
            else //Square/Normal TextBox
            {
                graph.FillRectangle(new SolidBrush(textBox.BackColor), 0, 0, this.Width, this.Height);// 填充TextBox外Panel的Padding空白
                //Draw border
                if (borderSize > 0)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        //penBorder.Alignment = PenAlignment.Inset;
                        if (isFocused) penBorder.Color = BorderFocusColor;

                        if (underlinedStyle) //Line Style  // graph.DrawLine(penBorder, 0, this.Height - borderSize, this.Width, this.Height - borderSize);
                            graph.DrawLine(penBorder, 0, this.Height - borderSize / 2, this.Width, this.Height - borderSize / 2);
                        else //Normal Style // graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                            graph.DrawRectangle(penBorder, borderSize / 2, borderSize / 2, this.Width - borderSize, this.Height - borderSize); // 底部的线差一像素
                    }
                }
            }
        }
        #endregion

        #region -> Private methods
        /// <summary>
        /// 设置Placeholder效果，在PlaceholderText、PlaceholderColor、Text修改时、textBox_Leave事件中执行设置
        /// </summary>
        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(placeholderText))
            {
                isPlaceholder = true;
                textBox.Text = placeholderText;
                textBox.ForeColor = PlaceholderColor;
                if (passwordChar != default(char))
                {
                    textBox.PasswordChar = default(char);
                    //textBox.UseSystemPasswordChar = false; // UseSystemPasswordChar 的优先权大于 PasswordChar，如果UseSystemPasswordChar设置为true将使用系统密码字符，而忽略PasswordChar的设置
                }
            }
        }
        /// <summary>
        /// 移除Placeholder效果，修改Text、textBox_Enter事件中执行移除
        /// </summary>
        private void RemovePlaceholder()
        {
            //if (isPlaceholder && placeholderText != "")
            if (isPlaceholder)
            {
                isPlaceholder = false;
                textBox.Text = text;
                textBox.ForeColor = this.ForeColor;
                if (passwordChar != default(char))
                {
                    //textBox.UseSystemPasswordChar = true;
                    textBox.PasswordChar = passwordChar;
                }
            }
        }
        /// <summary>
        /// 只在当行模式下才更新控件的高度，宽度、多行模式下的高度应该通过Dock保证同步大小(变更多行模式、padding时已经同步设置好正确位置(padding)，之后就是有Dock保证)
        /// </summary>
        private void UpdateControlHeight()
        {
            if (textBox.Multiline == false)
            {
                //int txtHeight = TextRenderer.MeasureText(textBox.Text, this.Font).Height + 1;
                //textBox.Multiline = true;
                //textBox.MinimumSize = new Size(0, txtHeight);
                //textBox.Multiline = false;
                // 单行模式下高度不变
                Height = textBox.Height + base.Padding.Top + base.Padding.Bottom;
            }
        }
        /// <summary>
        /// 设置宽高，目前仅在调整padding、修改单行多行模式后设置执行，保证TextBox显示在正确的位置和正确的边距，Panel有正确的大小 ||| 只有单行模式下高度才需要设置(宽度会自动)
        /// </summary>
        private void SetControlSize()
        {
            // base.Padding 已经是计算后正确的padding了
            //Height = textBox.Height + base.Padding.Top + base.Padding.Bottom + borderSize * 2;
            //Width = textBox.Width + base.Padding.Left + base.Padding.Right + borderSize * 2;
            Height = textBox.Height + base.Padding.Top + base.Padding.Bottom;
            Width = textBox.Width + base.Padding.Left + base.Padding.Right;
        }
        #endregion
    }
}
