using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTransparent
{
    public class TransparentLabel : Label
    {
        public TransparentLabel()
        {
            this.AutoSize = true;
            this.Visible = false;

            this.ImageAlign = ContentAlignment.TopLeft;
            this.Visible = true;

            this.Resize += TransparentLabelControl_Resize;
            this.LocationChanged += TransparentLabelControl_LocationChanged;
            this.TextChanged += TransparentLabelControl_TextChanged;
            this.ParentChanged += TransparentLabelControl_ParentChanged;
            
        }

        #region Events
        private void TransparentLabelControl_ParentChanged(object sender, EventArgs e)
        {
            SetTransparent();
            if (this.Parent != null)
            {
                this.Parent.ControlAdded += Parent_ControlAdded;
                this.Parent.ControlRemoved += Parent_ControlRemoved;
            }
        }

        private void Parent_ControlRemoved(object sender, ControlEventArgs e)
        {
            SetTransparent();
        }

        private void Parent_ControlAdded(object sender, ControlEventArgs e)
        {
            if (this.Bounds.IntersectsWith(e.Control.Bounds))
            {
                SetTransparent();
            }
        }

        private void TransparentLabelControl_TextChanged(object sender, EventArgs e)
        {
            SetTransparent();
        }

        private void TransparentLabelControl_LocationChanged(object sender, EventArgs e)
        {

            SetTransparent();
        }

        private void TransparentLabelControl_Resize(object sender, EventArgs e)
        {
            SetTransparent();
        }
        #endregion

        public void SetTransparent()
        {
            if (this.Parent != null)
            {
                this.Visible = false;
                this.Image = this.takeComponentScreenShot(this.Parent);
                this.Visible = true;
            }
        }

        private Bitmap takeComponentScreenShot(Control control)
        {
            Rectangle rect = control.RectangleToScreen(this.Bounds);
            if (rect.Width == 0 || rect.Height == 0)
            {
                return null;
            }
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);

            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            return bmp;
        }
    }
}
