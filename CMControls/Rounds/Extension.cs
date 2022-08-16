using CMControls;
using System.Drawing;
using System.Windows.Forms;

namespace System.Drawing.Drawing2D
{
    public static class GraphicsExtension
    {
        /// <summary> 
        /// C# GDI+ 绘制圆角实心矩形 ，填充
        /// </summary> 
        /// <param name="g">Graphics 对象</param> 
        /// <param name="rectangle">要填充的矩形</param> 
        /// <param name="backColor">填充背景色</param> 
        /// <param name="r">圆角半径</param> 
        public static void FillRoundRectangle(this Graphics g, Rectangle rectangle, Color backColor, int radius, RoundMode roundMode = RoundMode.All)
        {
            using (Brush b = new SolidBrush(backColor))
            {
                g.FillRoundRectangle(rectangle, b, radius, roundMode);
            }
        }
        /// <summary> 
        /// C# GDI+ 绘制圆角实心矩形 ，填充
        /// </summary> 
        /// <param name="g">Graphics 对象</param> 
        /// <param name="rectangle">要填充的矩形</param> 
        /// <param name="backColor">填充背景色</param> 
        /// <param name="r">圆角半径</param> 
        public static void FillRoundRectangle(this Graphics g, Rectangle rectangle, Brush brush, int radius, RoundMode roundMode = RoundMode.All)
        {
            ////抗锯齿 尽可能高质量绘制
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            //rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            g.FillPath(brush, rectangle.GetRoundedRectPath(radius, roundMode)); // 填充路径，而不是DrawPath
        }

        /// <summary> 
        /// 根据普通矩形得到圆角矩形的路径 【根据矩形区域rect，计算呈现radius圆角的Graphics路径】
        /// </summary> 
        /// <param name="rect">原始矩形</param> 
        /// <param name="radius">半径</param> 
        /// <returns>图形路径</returns> 
        public static GraphicsPath GetRoundedRectPath(this Rectangle rect, int radius, RoundMode roundMode= RoundMode.All)
        {
            #region 正确绘制圆角矩形区域
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(rect);
            }
            else
            {
                int R = radius * 2;
                Rectangle arcRect = new Rectangle(rect.Location, new Size(R, R));

                switch (roundMode)
                {
                    // 利用路径的闭合方法，可以少画好几条线
                    case RoundMode.LeftTop:
                        PathLeftTopArc(rect, path, arcRect);
                        //path.AddLine(rect.Left + arcRect.Width, rect.Top, rect.Right, rect.Top);
                        path.AddLine(rect.Right, rect.Top,rect.Right,rect.Bottom);
                        path.AddLine(rect.Right,rect.Bottom,rect.Left,rect.Bottom);
                        //path.AddLine(rect.Left,rect.Bottom,rect.Left,rect.Top+arcRect.Height);
                        break;
                    case RoundMode.RightTop:
                        PathRightTopArc(rect, path, arcRect);
                        //path.AddLine(rect.Left , rect.Top, rect.Right - arcRect.Width, rect.Top);
                        //path.AddLine(rect.Right, rect.Top + arcRect.Height, rect.Right, rect.Bottom);
                        path.AddLine(rect.Right, rect.Bottom, rect.Left, rect.Bottom);
                        path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top);
                        break;
                    case RoundMode.RightBottom:
                        //PathRightBottomArc(rect, path, arcRect); // 添加弧线或直线的顺序不同，得到的闭合路径也有可能不同(有差别，不同的形状)

                        path.AddLine(rect.Left, rect.Top, rect.Right, rect.Top);
                        //path.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom - arcRect.Height);
                        PathRightBottomArc(rect, path, arcRect);
                        //path.AddLine(rect.Right - arcRect.Width, rect.Bottom, rect.Left, rect.Bottom);
                        path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top );
                        
                        break;
                    case RoundMode.LeftBottom:
                        PathLeftBottomArc(rect, path, arcRect);
                        path.AddLine(rect.Left, rect.Top, rect.Right, rect.Top);
                        path.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom);
                        //path.AddLine(rect.Right, rect.Bottom, rect.Left + arcRect.Width, rect.Bottom);
                        //path.AddLine(rect.Left, rect.Bottom - arcRect.Height, rect.Left, rect.Top);
                        break;
                    case RoundMode.LeftTop | RoundMode.RightTop:
                        PathLeftTopArc(rect, path, arcRect);
                        PathRightTopArc(rect, path, arcRect);

                        //path.AddLine(rect.Left + arcRect.Width, rect.Top, rect.Right- arcRect.Width, rect.Top);
                        //path.AddLine(rect.Right, rect.Top+ arcRect.Height, rect.Right, rect.Bottom);
                        path.AddLine(rect.Right, rect.Bottom, rect.Left, rect.Bottom);
                        //path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top + arcRect.Height);
                        break;
                    case RoundMode.LeftTop | RoundMode.RightBottom:
                        PathLeftTopArc(rect, path, arcRect);

                        path.AddLine(rect.Left + arcRect.Width, rect.Top, rect.Right , rect.Top);
                        //path.AddLine(rect.Right, rect.Top + arcRect.Height, rect.Right, rect.Bottom - arcRect.Height);
                        PathRightBottomArc(rect, path, arcRect);
                        path.AddLine(rect.Right - arcRect.Width, rect.Bottom, rect.Left, rect.Bottom);
                        //path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top + arcRect.Height);
                        break;
                    case RoundMode.LeftTop | RoundMode.LeftBottom:
                        PathLeftTopArc(rect, path, arcRect);

                        //path.AddLine(rect.Left + arcRect.Width, rect.Top, rect.Right , rect.Top);
                        path.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom);
                        //path.AddLine(rect.Right, rect.Bottom, rect.Left+ arcRect.Width, rect.Bottom);
                        PathLeftBottomArc(rect, path, arcRect); // 顺序影响最终的闭合路径
                        //path.AddLine(rect.Left, rect.Bottom- arcRect.Height, rect.Left, rect.Top + arcRect.Height);
                        break;
                    case RoundMode.RightTop | RoundMode.RightBottom:
                        PathRightTopArc(rect, path, arcRect);
                        PathRightBottomArc(rect, path, arcRect);

                        //path.AddLine(rect.Left, rect.Top, rect.Right - arcRect.Width, rect.Top);
                        //path.AddLine(rect.Right, rect.Top + arcRect.Height, rect.Right, rect.Bottom- arcRect.Height);
                        //path.AddLine(rect.Right - arcRect.Width, rect.Bottom, rect.Left, rect.Bottom);
                        path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top );
                        break;
                    case RoundMode.RightTop | RoundMode.LeftBottom:
                        path.AddLine(rect.Left, rect.Top, rect.Right - arcRect.Width, rect.Top);
                        PathRightTopArc(rect, path, arcRect);

                        //path.AddLine(rect.Right, rect.Top + arcRect.Height, rect.Right, rect.Bottom);
                        path.AddLine(rect.Right, rect.Bottom, rect.Left+ arcRect.Width, rect.Bottom);
                        PathLeftBottomArc(rect, path, arcRect);
                        //path.AddLine(rect.Left, rect.Bottom- arcRect.Height, rect.Left, rect.Top);
                        break;
                    case RoundMode.RightBottom | RoundMode.LeftBottom:
                        PathRightBottomArc(rect, path, arcRect);
                        PathLeftBottomArc(rect, path, arcRect);

                        path.AddLine(rect.Left, rect.Top, rect.Right, rect.Top);
                        //path.AddLine(rect.Right, rect.Top, rect.Right, rect.Bottom - arcRect.Height);
                        //path.AddLine(rect.Right - arcRect.Width, rect.Bottom, rect.Left+ arcRect.Width, rect.Bottom);
                        //path.AddLine(rect.Left, rect.Bottom- arcRect.Height, rect.Left, rect.Top);
                        break;

                    case RoundMode.LeftTop | RoundMode.RightTop | RoundMode.RightBottom:
                        PathLeftTopArc(rect, path, arcRect);
                        PathRightTopArc(rect, path, arcRect);
                        PathRightBottomArc(rect, path, arcRect);

                        path.AddLine(rect.Right - arcRect.Width, rect.Bottom, rect.Left, rect.Bottom);
                        break;
                    case RoundMode.LeftTop | RoundMode.RightTop | RoundMode.LeftBottom:
                        PathLeftTopArc(rect, path, arcRect);
                        PathRightTopArc(rect, path, arcRect);

                        path.AddLine(rect.Right, rect.Top + arcRect.Height, rect.Right, rect.Bottom);
                        PathLeftBottomArc(rect, path, arcRect);
                        break;
                    case RoundMode.LeftTop | RoundMode.RightBottom | RoundMode.LeftBottom:
                        PathLeftTopArc(rect, path, arcRect);

                        path.AddLine(rect.Left+arcRect.Width , rect.Top, rect.Right, rect.Top);
                        PathRightBottomArc(rect, path, arcRect);
                        PathLeftBottomArc(rect, path, arcRect);
                        break;
                    case RoundMode.RightTop | RoundMode.RightBottom | RoundMode.LeftBottom:
                        PathRightTopArc(rect, path, arcRect);
                        PathRightBottomArc(rect, path, arcRect);
                        PathLeftBottomArc(rect, path, arcRect);

                        path.AddLine(rect.Left , rect.Top, rect.Right- arcRect.Width, rect.Top);
                        break;


                    case RoundMode.All:
                    default:
                        // 左上圆弧 左手坐标系，顺时针为正 从180开始，转90度
                        path.AddArc(arcRect, 180, 90);
                        // 右上圆弧
                        arcRect.X = rect.Right - R;
                        path.AddArc(arcRect, 270, 90);
                        // 右下圆弧
                        arcRect.Y = rect.Bottom - R;
                        path.AddArc(arcRect, 0, 90);
                        // 左下圆弧
                        arcRect.X = rect.Left;
                        path.AddArc(arcRect, 90, 90);
                        break;
                }

            }

            //path.CloseFigure();
            // 闭合路径中所有开放图形，并形成新图形
            path.CloseAllFigures();
            return path;
            #endregion

            #region 另一种实现
            //var rectangle = rect;
            //var r = radius;
            //int l = 2 * radius;
            //// 把圆角矩形分成四段直线、弧的组合，依次加到路径中 
            //GraphicsPath gp = new GraphicsPath();
            //gp.AddLine(new Point(rectangle.X + r, rectangle.Y), new Point(rectangle.Right - r, rectangle.Y));
            //gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Y, l, l), 270F, 90F);

            //gp.AddLine(new Point(rectangle.Right, rectangle.Y + r), new Point(rectangle.Right, rectangle.Bottom - r));
            //gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Bottom - l, l, l), 0F, 90F);

            //gp.AddLine(new Point(rectangle.Right - r, rectangle.Bottom), new Point(rectangle.X + r, rectangle.Bottom));
            //gp.AddArc(new Rectangle(rectangle.X, rectangle.Bottom - l, l, l), 90F, 90F);

            //gp.AddLine(new Point(rectangle.X, rectangle.Bottom - r), new Point(rectangle.X, rectangle.Y + r));
            //gp.AddArc(new Rectangle(rectangle.X, rectangle.Y, l, l), 180F, 90F);
            //return gp; 
            #endregion
        }
        /// <summary>
        /// 左下角的圆弧
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="path"></param>
        /// <param name="arcRect"></param>
        private static void PathLeftBottomArc(Rectangle rect, GraphicsPath path, Rectangle arcRect)
        {
            // 左下圆弧
            arcRect.X = rect.Left;
            arcRect.Y = rect.Bottom - arcRect.Height;
            path.AddArc(arcRect, 90, 90);
        }

        private static void PathRightBottomArc(Rectangle rect, GraphicsPath path, Rectangle arcRect)
        {
            // 右下圆弧
            arcRect.Y = rect.Bottom - arcRect.Height;
            arcRect.X = rect.Right - arcRect.Width;
            path.AddArc(arcRect, 0, 90);
        }

        private static void PathRightTopArc(Rectangle rect, GraphicsPath path, Rectangle arcRect)
        {
            // 右上圆弧
            arcRect.X = rect.Right - arcRect.Width;
            arcRect.Y = rect.Top;
            path.AddArc(arcRect, 270, 90);
        }

        private static void PathLeftTopArc(Rectangle rect, GraphicsPath path, Rectangle arcRect)
        {
            // 左上圆弧 左手坐标系，顺时针为正 从180开始，转90度
            arcRect.X = rect.Left;
            arcRect.Y = rect.Top;
            path.AddArc(arcRect, 180, 90);
        }

        /// <summary> 
        /// 获取圆角矩形的路径 
        /// </summary> 
        /// <param name="rect">原始矩形</param> 
        /// <param name="radius">半径</param> 
        /// <returns>图形路径</returns> 
        public static GraphicsPath GetRoundedRectPath(int x, int y, int width, int height, int radius, RoundMode roundMode = RoundMode.All)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            return rect.GetRoundedRectPath(radius, roundMode);
        }

        /// <summary>
        /// AddClosedCurve 方法的圆角路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="p_1">曲线到直线的交点</param>
        /// <param name="tension">表示圆角曲线的弯曲程度，0~1，0表示最小弯角(最锐拐角)，1表示最平滑弯曲</param>
        public static GraphicsPath CurveRoundPath(this Rectangle rect, int p_1, float tension)
        {
            GraphicsPath oPath = new GraphicsPath();
            oPath.AddClosedCurve(new Point[] {
                                new Point(0, rect.Height / p_1),
                                new Point(rect.Width / p_1, 0),
                                new Point(rect.Width - rect.Width / p_1, 0),
                                new Point(rect.Width, rect.Height / p_1),
                                new Point(rect.Width, rect.Height - rect.Height / p_1),
                                new Point(rect.Width - rect.Width / p_1, rect.Height),
                                new Point(rect.Width / p_1, rect.Height),
                                new Point(0, rect.Height - rect.Height / p_1)
                            }, tension);
            return oPath;
        }

        /// <summary>
        /// 绘制可渐变的圆角矩形，并指定是否有三角小尖及其位置
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle">矩形区域</param>
        /// <param name="_radius">圆角半径</param>
        /// <param name="begin_bgcolor">背景渐变开始色</param>
        /// <param name="end_bgcolor">背景渐变结束色</param>
        /// <param name="cusp">是否有三角小尖，默认无</param>
        /// <param name="rectAlign">三角小尖的位置，默认右上</param>
        /// <param name="gradientMode">渐变模式，默认垂直方向渐变</param>
        /// <param name="borderPen">绘制border的Pen对象</param>
        /// <returns>绘制的主体矩形区域</returns>
        public static Rectangle DrawFillRoundRectAndCusp(this Graphics g, Rectangle rectangle, int _radius, Color begin_bgcolor, Color end_bgcolor, bool cusp = false, RectangleAlign rectAlign = RectangleAlign.RightTop, LinearGradientMode gradientMode = LinearGradientMode.Vertical, Pen borderPen = null, bool onlyDrawLine = false, RoundMode roundMode = RoundMode.All)
        {
            //渐变填充
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, begin_bgcolor, end_bgcolor, gradientMode))
            {
                return g.DrawFillRoundRectAndCusp(rectangle, _radius, linearGradientBrush, cusp, rectAlign, borderPen, onlyDrawLine, roundMode);
            }
        }

        /// <summary>
        /// 绘制指定背景的圆角矩形，并指定是否有三角小尖及其位置
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle">矩形区域</param>
        /// <param name="_radius">圆角半径</param>
        /// <param name="bgcolor">指定背景色</param>
        /// <param name="cusp">是否有三角小尖，默认无</param>
        /// <param name="rectAlign">三角小尖的位置，默认右上</param>
        /// <returns>绘制的主体矩形区域</returns>
        public static Rectangle DrawFillRoundRectAndCusp(this Graphics g, Rectangle rectangle, int _radius, Color bgcolor, bool cusp = false, RectangleAlign rectAlign = RectangleAlign.RightTop, Pen borderPen = null, bool onlyDrawLine = false, RoundMode roundMode = RoundMode.All)
        {
            using (var brush = new SolidBrush(bgcolor))
            {
                return g.DrawFillRoundRectAndCusp(rectangle, _radius, brush, cusp, rectAlign, borderPen, onlyDrawLine, roundMode);
            }
        }

        /// <summary>
        /// 绘制Brush画刷的圆角矩形，并指定是否有三角小尖及其位置
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle">矩形区域</param>
        /// <param name="_radius">圆角半径</param>
        /// <param name="bgbrush">指定背景画刷</param>
        /// <param name="cusp">是否有三角小尖，默认无</param>
        /// <param name="rectAlign">三角小尖的位置，默认右上</param>
        /// <returns>绘制的主体矩形区域</returns>
        public static Rectangle DrawFillRoundRectAndCusp(this Graphics g, Rectangle rectangle, int _radius, Brush bgbrush, bool cusp = false, RectangleAlign rectAlign = RectangleAlign.RightTop, Pen borderPen = null, bool onlyDrawLine = false,RoundMode roundMode = RoundMode.All)
        {
            ////抗锯齿 尽可能高质量绘制
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            var rect = rectangle;
            //画尖角 对应的变更rect区域
            if (cusp)
            {
                // 尖角的大小 默认为 开始位置为_radius 底边为20，高度为13的等腰三角形
                var cuspHemlineStart = _radius;
                var cuspHemlineLength = 20;
                var cuspHeight = 13;

                // 让位出来的间隔暂时为尖角高度-1
                var span = cuspHeight - 1;

                // 三角顶点
                PointF p1, p2, p3;

                switch (rectAlign)
                {
                    case RectangleAlign.AboveLeft:
                        p1 = new PointF(rectangle.X + cuspHemlineStart, rectangle.Y + cuspHeight);
                        p2 = new PointF(rectangle.X + cuspHemlineStart + cuspHemlineLength, rectangle.Y + cuspHeight);
                        p3 = new PointF(rectangle.X + cuspHemlineStart + cuspHemlineLength / 2, rectangle.Y);
                        rect = new Rectangle(rectangle.X, rectangle.Y + span, rectangle.Width, rectangle.Height - span);
                        break;
                    case RectangleAlign.AboveRight:
                        p1 = new PointF(rectangle.Right - cuspHemlineStart, rectangle.Y + cuspHeight);
                        p2 = new PointF(rectangle.Right - cuspHemlineStart - cuspHemlineLength, rectangle.Y + cuspHeight);
                        p3 = new PointF(rectangle.Right - cuspHemlineStart - cuspHemlineLength / 2, rectangle.Y);
                        rect = new Rectangle(rectangle.X, rectangle.Y + span, rectangle.Width, rectangle.Height - span);
                        break;
                    case RectangleAlign.RightBottom:
                        p1 = new PointF(rectangle.Right - cuspHeight, rectangle.Bottom - cuspHemlineStart);
                        p2 = new PointF(rectangle.Right - cuspHeight, rectangle.Bottom - cuspHemlineStart - cuspHemlineLength);
                        p3 = new PointF(rectangle.Right, rectangle.Bottom - cuspHemlineStart - cuspHemlineLength / 2);
                        rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height);
                        break;
                    case RectangleAlign.BelowRight:
                        p1 = new PointF(rectangle.Right - cuspHemlineStart, rectangle.Bottom - cuspHeight);
                        p2 = new PointF(rectangle.Right - cuspHemlineStart - cuspHemlineLength, rectangle.Bottom - cuspHeight);
                        p3 = new PointF(rectangle.Right - cuspHemlineStart - cuspHemlineLength / 2, rectangle.Bottom);
                        rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height - span);
                        break;
                    case RectangleAlign.BelowLeft:
                        p1 = new PointF(rectangle.X + cuspHemlineStart, rectangle.Bottom - cuspHeight);
                        p2 = new PointF(rectangle.X + cuspHemlineStart + cuspHemlineLength, rectangle.Bottom - cuspHeight);
                        p3 = new PointF(rectangle.X + cuspHemlineStart + cuspHemlineLength / 2, rectangle.Bottom);
                        rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height - span);
                        break;
                    case RectangleAlign.LeftBottom:
                        p1 = new PointF(rectangle.X + cuspHeight, rectangle.Bottom - cuspHemlineStart);
                        p2 = new PointF(rectangle.X + cuspHeight, rectangle.Bottom - cuspHemlineStart - cuspHemlineLength);
                        p3 = new PointF(rectangle.X, rectangle.Bottom - cuspHemlineStart - cuspHemlineLength / 2);
                        rect = new Rectangle(rectangle.X + span, rectangle.Y, rectangle.Width - span, rectangle.Height);
                        break;
                    case RectangleAlign.LeftTop:
                        p1 = new PointF(rectangle.X + cuspHeight, rectangle.Y + cuspHemlineStart);
                        p2 = new PointF(rectangle.X + cuspHeight, rectangle.Y + cuspHemlineStart + cuspHemlineLength);
                        p3 = new PointF(rectangle.X, rectangle.Y + cuspHemlineStart + cuspHemlineLength / 2);
                        rect = new Rectangle(rectangle.X + span, rectangle.Y, rectangle.Width - span, rectangle.Height);
                        break;
                    case RectangleAlign.RightTop:
                    default:
                        p1 = new PointF(rectangle.Right - cuspHeight, rectangle.Y + cuspHemlineStart);
                        p2 = new PointF(rectangle.Right - cuspHeight, rectangle.Y + cuspHemlineStart + cuspHemlineLength);
                        p3 = new PointF(rectangle.Right, rectangle.Y + cuspHemlineStart + cuspHemlineLength / 2);
                        rect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height);
                        break;
                }

                PointF[] ptsArray = new PointF[] { p1, p2, p3 };

                // 填充参数点所指定的多边形内部
                if (!(onlyDrawLine && borderPen != null)) g.FillPolygon(bgbrush, ptsArray);
                if (borderPen != null)
                {
                    g.DrawPolygon(borderPen, ptsArray);
                }
            }

            using (var path = rect.GetRoundedRectPath(_radius, roundMode))
            {
                //填充
                if (!(onlyDrawLine && borderPen != null))
                {
                    g.FillPath(bgbrush, path);
                }
                if (borderPen != null)
                {
                    g.DrawPath(borderPen, path);
                }
            }

            return rect;
        }

        /// <summary>
        /// 填充region，依旧会有锯齿
        /// </summary>
        /// <param name="g"></param>
        /// <param name="region"></param>
        public static void FillRegion(this Graphics g, Region region, Color bgcolor)
        {
            ////抗锯齿 尽可能高质量绘制
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias; // SmoothingMode.HighQuality 
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            using (var brush = new SolidBrush(bgcolor))
            {
                g.FillRegion(brush, region);
            }
        }

        ///// <summary>
        ///// 绘制(控件区域)文本内容
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="rect"></param>
        ///// <param name="text"></param>
        ///// <param name="color"></param>
        ///// <param name="font"></param>
        ///// <param name="_textAlign">文字布局，默认居中。仍然会垂直偏上，使用微软雅黑会好点</param>
        //public static void DrawText(this Graphics g, Rectangle rect, string text, Color color, Font font, ContentAlignment _textAlign = ContentAlignment.MiddleCenter)
        //{
        //    using (Brush brush = new SolidBrush(color))
        //    {
        //        var textSize = new SizeF(rect.Width, rect.Height);
        //        //文本布局对象
        //        using (StringFormat strF = new StringFormat())
        //        {
        //            // 文字布局
        //            switch (_textAlign)
        //            {
        //                case ContentAlignment.TopLeft:
        //                    strF.Alignment = StringAlignment.Near;
        //                    strF.LineAlignment = StringAlignment.Near;
        //                    break;
        //                case ContentAlignment.TopCenter:
        //                    strF.Alignment = StringAlignment.Center;
        //                    strF.LineAlignment = StringAlignment.Near;
        //                    break;
        //                case ContentAlignment.TopRight:
        //                    strF.Alignment = StringAlignment.Far;
        //                    strF.LineAlignment = StringAlignment.Near;
        //                    break;
        //                case ContentAlignment.MiddleLeft:
        //                    strF.Alignment = StringAlignment.Near;
        //                    strF.LineAlignment = StringAlignment.Center;
        //                    break;
        //                case ContentAlignment.MiddleCenter:
        //                    strF.Alignment = StringAlignment.Center; //居中
        //                    strF.LineAlignment = StringAlignment.Center;//垂直居中
        //                    break;
        //                case ContentAlignment.MiddleRight:
        //                    strF.Alignment = StringAlignment.Far;
        //                    strF.LineAlignment = StringAlignment.Center;
        //                    break;
        //                case ContentAlignment.BottomLeft:
        //                    strF.Alignment = StringAlignment.Near;
        //                    strF.LineAlignment = StringAlignment.Far;
        //                    break;
        //                case ContentAlignment.BottomCenter:
        //                    strF.Alignment = StringAlignment.Center;
        //                    strF.LineAlignment = StringAlignment.Far;
        //                    break;
        //                case ContentAlignment.BottomRight:
        //                    strF.Alignment = StringAlignment.Far;
        //                    strF.LineAlignment = StringAlignment.Far;
        //                    break;
        //                default:
        //                    strF.Alignment = StringAlignment.Center; //居中
        //                    strF.LineAlignment = StringAlignment.Center;//垂直居中
        //                    break;
        //            }
        //            textSize=g.MeasureString(text, font, textSize, strF);
        //        }


        //        var x = rect.X + (rect.Width- textSize.Width) / 2;
        //        var y = rect.Y + (rect.Height - textSize.Height) / 2;
        //        // 文字布局位置
        //        switch (_textAlign)
        //        {
        //            case ContentAlignment.TopLeft:
        //                x = rect.X;
        //                y = rect.Y;
        //                break;
        //            case ContentAlignment.TopCenter:
        //                x = rect.X + (rect.Width - textSize.Width) / 2;
        //                y = rect.Y;
        //                break;
        //            case ContentAlignment.TopRight:
        //                x = rect.X + rect.Width - textSize.Width;
        //                y = rect.Y;
        //                break;
        //            case ContentAlignment.MiddleLeft:
        //                x = rect.X;
        //                y = rect.Y + (rect.Height - textSize.Height) / 2;
        //                break;
        //            case ContentAlignment.MiddleCenter:
        //                break;
        //            case ContentAlignment.MiddleRight:
        //                x = rect.X + rect.Width - textSize.Width;
        //                y = rect.Y + (rect.Height - textSize.Height) / 2;
        //                break;
        //            case ContentAlignment.BottomLeft:
        //                x = rect.X;
        //                y = rect.Y + rect.Height - textSize.Height;
        //                break;
        //            case ContentAlignment.BottomCenter:
        //                x = rect.X + (rect.Width - textSize.Width) / 2;
        //                y = rect.Y + rect.Height - textSize.Height;
        //                break;
        //            case ContentAlignment.BottomRight:
        //                x = rect.X + rect.Width - textSize.Width;
        //                y = rect.Y + rect.Height - textSize.Height;
        //                break;
        //            default:
        //                break;
        //        }

        //        g.DrawString(text, font, brush, x, y);

        //    }
        //}

        /// <summary>
        /// 绘制(控件区域)文本内容【推荐】
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        /// <param name="_textAlign">文字布局，默认居中。实际测试并未真正的居中，垂直方向偏上，改为通过计算rect的中心位置实现，使用微软雅黑还好点，字体大小最好偶数</param>
        /// <param name="rtl">是否RightToLeft 无效果，不推荐使用</param>
        public static void DrawText(this Graphics g, Rectangle rect, string text, Color color, Font font, ContentAlignment _textAlign = ContentAlignment.MiddleCenter, bool rtl=false)
        {             
                var formatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter; // 默认居中
                switch (_textAlign)
                {
                    case ContentAlignment.TopLeft:
                        formatFlags = TextFormatFlags.Top | TextFormatFlags.Left;
                        break;
                    case ContentAlignment.TopCenter:
                        formatFlags = TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                        break;
                    case ContentAlignment.TopRight:
                        formatFlags = TextFormatFlags.Top | TextFormatFlags.Right;
                        break;
                    case ContentAlignment.MiddleLeft:
                        formatFlags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                        break;
                    case ContentAlignment.MiddleRight:
                        formatFlags = TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                        break;
                    case ContentAlignment.BottomLeft:
                        formatFlags = TextFormatFlags.Bottom | TextFormatFlags.Left;
                        break;
                    case ContentAlignment.BottomCenter:
                        formatFlags = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                        break;
                    case ContentAlignment.BottomRight:
                        formatFlags = TextFormatFlags.Bottom | TextFormatFlags.Right;
                        break;
                    case ContentAlignment.MiddleCenter:
                    default:
                        break;
                }
                if (rtl)
                {
                    formatFlags |= TextFormatFlags.RightToLeft; // 无效果
                }
                TextRenderer.DrawText(g, text, font, rect, color, formatFlags);            
        }
        /// <summary>
        /// 绘制(控件区域)文本内容，推荐Graphics.DrawText
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        /// <param name="_textAlign">文字布局，默认居中。实际测试并未真正的居中，垂直方向偏上，改为通过计算rect的中心位置实现，使用微软雅黑还好点，字体大小最好偶数</param>
        /// <param name="rtl">是否RightToLeft</param>
        /// <param name="vertical">是否文字为垂直方向</param>
        [Obsolete("推荐使用Graphics.DrawText绘制文本(尤其是控件上)，DrawString绘制的文本字体大小有偏差、并垂直居中时文字有些偏上，但能处理文字垂直、RightToLeft")]
        public static void DrawString(this Graphics g, Rectangle rect, string text, Color color, Font font, ContentAlignment _textAlign = ContentAlignment.MiddleCenter, bool rtl = false,bool vertical=false)
        {
            using (Brush brush = new SolidBrush(color))
            {
                #region StringFormat
                // 文本布局对象
                //using (StringFormat strF = new StringFormat()) //直接使用new StringFormat()创建的对象会在垂直居中时文字偏上，使用StringFormat.GenericTypographic会好些，猜测和StringFormatFlags.有关
                //new StringFormat( StringFormatFlags.)
                //using (StringFormat strF = StringFormat.GenericDefault)// 同样垂直偏上
                using (StringFormat strF = StringFormat.GenericTypographic)
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

                    if (rtl)
                    {
                        strF.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    }
                    if (vertical)
                    {
                        strF.FormatFlags |= StringFormatFlags.DirectionVertical;
                    }
                    g.DrawString(text, font, brush, rect, strF); // 使用TextRenderer.DrawText绘制，文本更清晰、居中效果更好
                    //g.DrawString(text, font, brush, rect, StringFormat.GenericTypographic);
                }
                #endregion
            }
        }
    }
}
