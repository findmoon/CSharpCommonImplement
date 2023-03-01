using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMControls.Layer
{   
    /// <summary>
    /// 自定义控件:半透明控件
    /// </summary>
    /* 
     * [ToolboxBitmap(typeof(MyOpaqueLayer))]
     * 用于指定当把你做好的自定义控件添加到工具栏时，工具栏显示的图标。
     * 正确写法应该是
     * [ToolboxBitmap(typeof(XXXXControl),"xxx.bmp")]
     * 其中XXXXControl是你的自定义控件，"xxx.bmp"是你要用的图标名称。
    */
    [ToolboxBitmap(typeof(CMLayer))]
    public class CMLayer : Control
    {
        private int _alpha;//设置透明度 255表示不透明

        private PictureBox loadingPicBox;// 加载图片
        private Control controlOnLayer;//遮罩上的控件
        private Color layerColor;

        [Category("高级"), DefaultValue(125), Description("设置遮罩层透明度，默认125。255表示不透明")]
        public int Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 图片背景透明的效果和base.BackColor有关（控件背景，因此需要透明）。但并未做到真正透明底部的内容
        /// </summary>
        [Category("高级"), Description("遮罩层中间显示的图片，默认无。通常为加载效果的gif图片")]
        public Image LayerImage
        {
            get => loadingPicBox?.Image;
            set
            {
                if (value != null && loadingPicBox == null)
                {
                    loadingPicBox = new PictureBox()
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent,
                        //Margin = new Padding(0)
                    };
                    //loadingPicBox.Width = Width * 2 / 5;
                    //loadingPicBox.Height = Height * 2 / 5;
                    loadingPicBox.Width = value.Width;
                    loadingPicBox.Height = value.Height;
                    loadingPicBox.Location = new Point((Width - loadingPicBox.Width) / 2, (Height - loadingPicBox.Height) / 2);
                    Controls.Add(loadingPicBox);
                }
                if (loadingPicBox != null)
                {
                    loadingPicBox.Image = value;

                    if (controlOnLayer != null)
                    {
                        loadingPicBox.Visible = false;
                    }
                }
            }
        }
        [Category("高级"), Description("遮罩层中间显示的图片的宽度，默认为图片大小")]
        public int LayerImageWidth { get { return loadingPicBox?.Width ?? 0; } set { if (loadingPicBox != null) loadingPicBox.Width = value; } }
        [Category("高级"), Description("遮罩层中间显示的图片的高度，默认为图片大小")]
        public int LayerImageHeight { get { return loadingPicBox?.Height ?? 0; } set { if (loadingPicBox != null) loadingPicBox.Height = value; } }

        [Category("高级"), Description("遮罩层中间显示的控件，默认无。如果有控件，则中间的图片将不显示")]
        public Control ControlOnLayer
        {
            get => controlOnLayer;
            set
            {
                if (value == null) return;
                controlOnLayer = value;
                controlOnLayer.Location = new Point((Width - controlOnLayer.Width) / 2, (Height - controlOnLayer.Height) / 2);
                Controls.Add(controlOnLayer);
                controlOnLayer.BringToFront(); // 非必须
                if (loadingPicBox != null && loadingPicBox.Visible)
                {
                    loadingPicBox.Visible = false;
                }
            }
        }
        [Category("高级"), DefaultValue(typeof(Color), "Gray"), Description("遮罩层的颜色")]
        public Color LayerColor
        {
            get
            {
                return Color.FromArgb(_alpha, layerColor);
            }
            set
            {
                layerColor = value;
                Invalidate();
            }
        }

        public new Control Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                base.Parent = value;
                if (value != null)
                {
                    Dock = DockStyle.Fill;
                    BringToFront();
                }
            }
        }
        [Browsable(false), Description("隐藏背景色的更改")]
        public override Color BackColor => base.BackColor;

        public CMLayer() : this(125)
        {
        }
        public CMLayer(int alpha)
        {
            SetStyle(System.Windows.Forms.ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, true);
            base.CreateControl();

            this._alpha = alpha;

            base.BackColor = Color.Transparent;
            LayerColor = Color.Gray;
            Dock = DockStyle.Fill;
            BringToFront();
        }
        // 无效
        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    Invalidate();
        //}

        /// <summary>
        /// 自定义绘制背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            SolidBrush backBrush = new SolidBrush(LayerColor);

            //if (_transparentBG)
            //{
            //    Color drawColor = Color.FromArgb(this._alpha, this.BackColor);
            //    backBrush = new SolidBrush(drawColor);
            //}
            //else
            //{
            //    backBrush = new SolidBrush(this.BackColor);
            //}
            //e.Graphics.Clear(Color.Transparent); // 绘制前清理，无效，导致显示前景色 应该清理控件的Region区域再填充

            // 这样的绘制方式(CreateParams、ControlStyles.Opaque)，每次都是在之前的颜色上叠加绘制，如果多次BackColor赋值绘制，颜色逐渐加深
            e.Graphics.FillRectangle(backBrush, ClientRectangle);
        }

        // 不需要  会有一些不同的透明效果
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x00000020; //0x20;  // 开启 WS_EX_TRANSPARENT,使控件支持透明
        //        return cp;
        //    }
        //}

    }
}
