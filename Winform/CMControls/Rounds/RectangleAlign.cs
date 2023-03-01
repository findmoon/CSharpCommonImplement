using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing.Drawing2D
{
    /// <summary>
    /// 矩形区域的位置，分为上左、上右、右上、右下、下右、下左、左下、左上八个部分，也可以扩展这8个部分的内外
    /// </summary>
    public enum RectangleAlign
    {
        //
        // 摘要:上方并位于其左侧。
        //     
        AboveLeft = 0,
        //
        // 摘要:上方并位于其右侧。
        //
        AboveRight = 1,
        //
        // 摘要:右边并位于其上方。
        //
        RightTop = 2,
        //
        // 摘要:右边并位于其下方。
        //
        RightBottom = 3,
        //
        // 摘要: 下方并位于其右侧
        // 
        BelowRight = 4,
        //
        // 摘要: 下方并位于其左侧。
        //     
        BelowLeft = 5,

        //
        // 摘要: 左边并位于其下方。
        // 
        LeftBottom = 6,
        //
        // 摘要: 左边并位于其上方。
        // 
        LeftTop = 7,

    }
}
