using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTransparent
{
    public partial class CodeSetBtnBackColorImg : Form
    {
        public CodeSetBtnBackColorImg()
        {
            InitializeComponent();
            Load += CodeSetBtnBackColorImg_Load;

            //button1.Paint += Button1_Paint;

            
        }

        private void CodeSetBtnBackColorImg_Load(object sender, EventArgs e)
        {
            //button1.Visible = false;
            //button1.BackgroundImage = takeComponentScreenShot(button1);
            //button1.Visible = true;

    
        }

        private void Button1_Paint(object sender, PaintEventArgs e)
        {
            var btn = (Button)sender;

            ButtonTransparent(btn);
        }


        private Bitmap takeComponentScreenShot(Control control)
        {
            // 计算指定工作区矩形的大小和位置（以屏幕坐标表示）
            Rectangle rect = control.RectangleToScreen(control.DisplayRectangle); // 或者 control.Bounds
            if (rect.Width == 0 || rect.Height == 0)
            {
                return null;
            }
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);

            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            return bmp;
        }

        private void ButtonTransparent(Button btn)
        {
            // bmp.Clone 时会报错out of memory，无法使用
            if (btn.Parent.BackgroundImage != null)
            {

                // 封装一个图像对象，该对象即主窗体或父容器背景图片
                Bitmap bmp = new Bitmap(btn.Parent.BackgroundImage, btn.Width, btn.Height);

                // 绘制矩形，定义你需要截取的图像起始位置和宽高
                Rectangle r = new Rectangle(btn.Left, btn.Top, btn.Width, btn.Height);

                // 按矩形尺寸和起始位置截取bm的一部分
                //bmp = bmp.Clone(r, System.Drawing.Imaging.PixelFormat.Undefined); // out of memory
                bmp = bmp.Clone(r, btn.Parent.BackgroundImage.PixelFormat); // out of memory

                // 把截取到的图片设置为需要透明的控件背景，达到与主窗体背景完美契合的效果
                btn.BackgroundImage = bmp;
            }
            else
            {
                btn.BackColor = btn.Parent.BackColor;
            }
        }
    }
}
