using System.Drawing;

namespace System.Windows.Forms
{
    public static class ControlExten
    {
        /// <summary>
        /// 什么作用也没有
        /// </summary>
        /// <param name="Button"></param>
        /// <param name="TransparentColor"></param>
        public static void ToTransparent(this Button Button,Color TransparentColor)
        {
            Bitmap bmp;
            if (Button.Image==null)
            {
                bmp = new Bitmap(Button.Width/2, Button.Height/2);
            }
            else
            {
                bmp = (Bitmap)Button.Image;
            }
            bmp.MakeTransparent(TransparentColor);
            int x = (Button.Width - bmp.Width) / 2;
            int y = (Button.Height - bmp.Height) / 2;
            Graphics gr = Button.CreateGraphics();
            gr.DrawImage(bmp, x, y);
        }
    }
}
