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
    [Obsolete("用户控件无法透明")]
    [DefaultEvent("TextChanged")]
    public partial class TextBoxProNo : UserControl
    {
        //Default Event
        public new event EventHandler TextChanged; // public event EventHandler TextChanged; 除非显式new重写
        //TextBox-> TextChanged event
        private void textBox_TextChanged(object sender, EventArgs e)
        {
                TextChanged?.Invoke(sender, e);
        }

        //Fields
        private Color borderColor = Color.MediumSlateBlue;
        private Color borderFocusColor = Color.HotPink;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private bool isFocused = false;

        private int borderRadius = 0;
        private Color placeholderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholder = false;
        private char passwordChar = default(char);


        #region -> Properties
        [Category("Advance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        [Category("Advance")]
        public Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
        }

        [Category("Advance")]
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

        [Category("Advance")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        //[Category("Advance")]
        //public bool PasswordChar
        //{
        //    get { return isPasswordChar; }
        //    set
        //    {
        //        isPasswordChar = value;
        //        if (!isPlaceholder)
        //            textBox.UseSystemPasswordChar = value;
        //    }
        //}

        [Category("Advance")]
        public bool Multiline
        {
            get { return textBox.Multiline; }
            set { textBox.Multiline = value; }
        }

        private Region originRegion;

        [Category("Advance")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                textBox.BackColor = value;
            }
        }

        [Category("Advance")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                textBox.ForeColor = value;
            }
        }

        [Category("Advance")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                textBox.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }

        [Category("Advance")]
        public string Texts
        {
            get
            {
                if (isPlaceholder) return "";
                else return textBox.Text;
            }
            set
            {
                textBox.Text = value;
                SetPlaceholder();
            }
        }

        [Category("Advance")]
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

        [Category("Advance")]
        public Color PlaceholderColor
        {
            get { return placeholderColor; }
            set
            {
                placeholderColor = value;
                if (isPlaceholder)
                    textBox.ForeColor = value;
            }
        }

        [Category("Advance")]
        public string PlaceholderText
        {
            get { return placeholderText; }
            set
            {
                placeholderText = value;
                textBox.Text = "";
                SetPlaceholder();
            }
        }
        #endregion

        public TextBoxProNo()
        {
            InitializeComponent();

            originRegion = Region;

            // 不支持透明背景
            ////SetStyle(ControlStyles.SupportsTransparentBackColor, //支持透明背景颜色
            //// true);
            ////BackColor = Color.Transparent;

            // 若能透明则不需要
            BackColorChanged += TextBoxPro_BackColorChanged;
            textBox.BackColor = BackColor;

            textBox.Enter += textBox_Enter;
            textBox.Leave += textBox_Leave;
            textBox.Click += textBox_Click;
            textBox.MouseEnter += textBox_MouseEnter;
            textBox.MouseLeave += textBox_MouseLeave;
            textBox.KeyPress += textBox_KeyPress;
        }

        private void TextBoxPro_BackColorChanged(object sender, EventArgs e)
        {
            textBox.BackColor = BackColor;
        }


        #region Event handler
        private void textBox_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }
        private void textBox_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            SetPlaceholder();
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
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }
        #endregion

        #region -> Overridden methods
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            this.Region = originRegion;

            Graphics graph = e.Graphics;
            ////抗锯齿 尽可能高质量绘制
            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.InterpolationMode = InterpolationMode.HighQualityBilinear;

            if (borderRadius >0)//Rounded TextBox
            {
                //-Fields
                var rectBorderSmooth = this.ClientRectangle;
                //var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                var rect = new Rectangle(rectBorderSmooth.X+ borderSize / 2, rectBorderSmooth.Y+borderSize / 2, rectBorderSmooth.Width - borderSize, rectBorderSmooth.Height - borderSize);
                // 由于用户控件不支持背景透明，绘制透明色的光滑区域
                int smoothSize = borderSize > 0 ? borderSize : 1;

                using (GraphicsPath pathBorderSmooth = rectBorderSmooth.GetRoundedRectPath(borderRadius))
                //using (GraphicsPath pathBorder = rectBorder.GetRoundedRectPath(borderRadius - borderSize))
                //using (GraphicsPath pathBorder = ClientRectangle.GetRoundedRectPath(borderRadius))
                using (Pen penBorderSmooth = new Pen(Color.Transparent, smoothSize)) // 似乎使用Color.Transparent透明无效
                //using (Pen penBorderSmooth = new Pen(Parent.BackColor, smoothSize))
                {
                    
                    if (borderSize>0)
                    {
                        using (Pen penBorder = new Pen(borderColor, borderSize))
                        {
                            //-Drawing
                            //this.Region = new Region(pathBorderSmooth);//Set the rounded region of UserControl
                            //if (borderRadius > 15) SetTextBoxRoundedRegion();//Set the rounded region of TextBox component
                            //graph.SmoothingMode = SmoothingMode.AntiAlias;
                            //penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                            if (isFocused) penBorder.Color = borderFocusColor;

                            //Draw border smoothing
                            graph.DrawPath(penBorderSmooth, pathBorderSmooth);

                            if (underlinedStyle) //Line Style
                            {
                                //Draw border
                                //graph.SmoothingMode = SmoothingMode.None;
                                graph.DrawLine(penBorder, 0, this.Height - borderSize, this.Width, this.Height - borderSize);
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
                    else
                    {
                        using (GraphicsPath path = rect.GetRoundedRectPath(borderRadius))
                        {
                            Region = new Region(path);
                            //Draw border smoothing
                            graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        }
                    }
                }
               
            }
            else //Square/Normal TextBox
            {
                //Draw border
                if (borderSize>0)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                        if (isFocused) penBorder.Color = borderFocusColor;

                        if (underlinedStyle) //Line Style  // graph.DrawLine(penBorder, 0, this.Height - borderSize, this.Width, this.Height - borderSize);
                            graph.DrawLine(penBorder, 0, this.Height, this.Width, this.Height);
                        else //Normal Style // graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                            graph.DrawRectangle(penBorder, 0, 0, this.Width, this.Height);
                    }
                }
            }
        }
        #endregion

        #region -> Private methods
        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) && placeholderText != "")
            {
                isPlaceholder = true;
                textBox.Text = placeholderText;
                textBox.ForeColor = placeholderColor;
                if (passwordChar != default(char))
                {
                    textBox.PasswordChar = default(char);
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }
        private void RemovePlaceholder()
        {
            if (isPlaceholder && placeholderText != "")
            {
                isPlaceholder = false;
                textBox.Text = "";
                textBox.ForeColor = this.ForeColor;
                if (passwordChar != default(char))
                {
                    textBox.UseSystemPasswordChar = true;
                    textBox.PasswordChar = passwordChar;
                }
            }
        }

        //private void SetTextBoxRoundedRegion()
        //{
        //    GraphicsPath pathTxt;
        //    if (Multiline)
        //    {
        //        pathTxt = textBox.ClientRectangle.GetRoundedRectPath(borderRadius - borderSize);
        //    }
        //    else
        //    {
        //        pathTxt = textBox.ClientRectangle.GetRoundedRectPath( borderSize * 2);                
        //    }
        //    textBox.Region = new Region(pathTxt);
        //    pathTxt.Dispose();
        //}
        private void UpdateControlHeight()
        {
            if (textBox.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText(textBox.Text, this.Font).Height + 1;
                textBox.Multiline = true;
                textBox.MinimumSize = new Size(0, txtHeight);
                textBox.Multiline = false;

                this.Height = textBox.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }
        #endregion
    }
}
