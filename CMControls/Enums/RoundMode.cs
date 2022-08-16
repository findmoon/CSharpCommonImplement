using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMControls
{
    /// <summary>
    /// 圆角模式，定义的位置，取值None、左上、右上、右下、左下角
    /// 使用的.Net提供的System.Windows.Forms.Design.AnchorEditor编辑器，正确做法是自己仿照重写，在设计器中可以选择四个角及其组合
    /// </summary>
    [Editor("System.Windows.Forms.Design.AnchorEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [Flags]
    public enum RoundMode
    {
        Full=0, // 等同All，四角都圆角//None=0,
        LeftTop=1,
        RightTop=2,
        RightBottom=4,
        LeftBottom=8,

        All= LeftTop| RightTop| RightBottom| LeftBottom
    }
}
