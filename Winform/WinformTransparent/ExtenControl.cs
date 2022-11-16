using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
    class ExtenControl
    {        

/// <summary>
/// 根据图片计算GraphicsPath路径
/// </summary>
/// <param name="img">gif或者png图片</param>
/// <returns>图片不透明区域路径</returns>
/// 用这个代码的时候需要选择项目属性->生成->勾选"允许不安全代码"
/// 因为这里用到了指针，C#用指针式不安全的，具体可查看msdn
private unsafe static GraphicsPath SubGraphicsPath(Image img) // 不安全代码只会在使用/unsafe编译的情况下出现
    {
        if (img == null) return null;

        // 建立GraphicsPath, 给我们的位图路径计算使用 
        GraphicsPath g = new GraphicsPath(FillMode.Alternate);

        Bitmap bitmap = new Bitmap(img);

        int width = bitmap.Width;
        int height = bitmap.Height;
        BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        byte* p = (byte*)bmData.Scan0;
        int offset = bmData.Stride - width * 3;
        int p0, p1, p2;         // 记录左上角0，0座标的颜色值
        p0 = p[0];
        p1 = p[1];
        p2 = p[2];

        int start = -1;
        // 行座标 ( Y col ) 
        for (int Y = 0; Y < height; Y++)
        {
            // 列座标 ( X row ) 
            for (int X = 0; X < width; X++)
            {
                if (start == -1 && (p[0] != p0 || p[1] != p1 || p[2] != p2))     //如果 之前的点没有不透明 且 不透明 
                {
                    start = X;                            //记录这个点
                }
                else if (start > -1 && (p[0] == p0 && p[1] == p1 && p[2] == p2))      //如果 之前的点是不透明 且 透明
                {
                    g.AddRectangle(new Rectangle(start, Y, X - start, 1));    //添加之前的矩形到
                    start = -1;
                }

                if (X == width - 1 && start > -1)        //如果 之前的点是不透明 且 是最后一个点
                {
                    g.AddRectangle(new Rectangle(start, Y, X - start + 1, 1));      //添加之前的矩形到
                    start = -1;
                }
                //if (p[0] != p0 || p[1] != p1 || p[2] != p2)
                //    g.AddRectangle(new Rectangle(X, Y, 1, 1));
                p += 3;                                   //下一个内存地址
            }
            p += offset;
        }
        bitmap.UnlockBits(bmData);
        bitmap.Dispose();
        // 返回计算出来的不透明图片路径 
        return g;
    }

    /// <summary>
    /// 调用此函数后使图片透明
    /// </summary>
    /// <param name="control">需要处理的控件</param>
    /// <param name="img">控件的背景或图片，如PictureBox.Image
    ///   或PictureBox.BackgroundImage</param>
    public static void ControlTrans(Control control, Image img)
    {
        GraphicsPath g;
        g = SubGraphicsPath(img);
        if (g == null)
            return;
        control.Region = new Region(g);
    }

}
}
