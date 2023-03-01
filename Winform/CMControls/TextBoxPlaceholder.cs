using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMControls
{
    public class TextBoxPlaceholder : TextBox
    {
        //public string PlaceHolderText { get; set; }
        public Color PlaceHolderColor { get; set; } = Color.LightGray;

        #region method1
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == 0xF || m.Msg == 0x133) // 15 WM_PAINT 消息 和 307 WM_CTLCOLOREDIT 消息(ctlcoloredit)
        //    {
        //        WmPaint(ref m);
        //    }
        //}
        //private void WmPaint(ref Message m)
        //{
        //    //this.CreateGraphics();
        //    if (!String.IsNullOrEmpty(this.PlaceHolderText) && string.IsNullOrEmpty(this.Text))
        //    {
        //        using (Graphics g = Graphics.FromHwnd(base.Handle)) // 从指定的窗口句柄创建新的Graphics // 或 Graphics g = this.CreateGraphics()
        //        {
        //            g.DrawString(this.PlaceHolderText, this.Font, new SolidBrush(PlaceHolderColor), 0, (Height - FontHeight) / 2); // 
        //        }
        //    }
        //} 
        #endregion

        #region 无效
        ///// <summary>
        ///// OnPaint中直接绘制无效果
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    if (!string.IsNullOrEmpty(PlaceHolderText)&& string.IsNullOrEmpty(Text))
        //    {
        //        //坐标位置 0,0 需要根据对齐方式重新计算.
        //        // e.Graphics.DrawString(PlaceHolderText, Font, new SolidBrush(PlaceHolderColor), 0, 0);
        //        using (Graphics g = Graphics.FromHwnd(base.Handle))
        //            g.DrawString(PlaceHolderText, Font, new SolidBrush(PlaceHolderColor), 0, 0);
        //    }
        //} 
        #endregion

        #region Method2
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        private string placeHolderText = string.Empty;
        public string PlaceHolderText
        {
            get { return placeHolderText; }
            set
            {
                placeHolderText = value;
                if (!string.IsNullOrWhiteSpace(placeHolderText))
                {
                    SendMessage(Handle, EM_SETCUEBANNER, 0, placeHolderText);
                }
            }
        } 
        #endregion
    }
}
