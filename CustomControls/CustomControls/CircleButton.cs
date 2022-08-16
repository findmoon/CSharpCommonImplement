using System.Drawing;

namespace System.Windows.Forms
{
    public class CircleButton : Button//继承按钮类    重新生成解决方案就能在工具箱看见我啦
    {
        //private new FlatStyle FlatStyle { get; set; }
        protected override void OnPaint(PaintEventArgs e)//重新设置控件的形状   protected 保护  override重新
        {
            base.OnPaint(e);//递归  每次重新都发生此方法,保证其形状为自定义形状
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(2, 2, this.Width - 6, this.Height - 6);
            // 取消圆形按钮边框，执行DrawEllipse会在圆形外部形成一个边框
            //Graphics g = e.Graphics;
            //g.DrawEllipse(new Pen(Color.Black, 2), 2, 2, Width - 6, Height - 6);
            Region = new Region(path);
        }
    }
}
