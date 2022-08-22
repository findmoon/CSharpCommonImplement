using System.Windows.Forms;

namespace WinformTransparent.Controls
{
    /// <summary>
    /// A transparent control.  实现了透明，控件本身、所占位置均存在，且鼠标不能透过操作底层显示出的控件
    /// </summary>
    public class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }
    }
}