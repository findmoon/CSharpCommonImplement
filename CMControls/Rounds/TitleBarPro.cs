using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMControls
{
    public class TitleBarPro: RoundPanel
    {
        Label titleLabel = new Label();
        PictureBox ClosePicb = new PictureBox();
        PictureBox MaximizeNormalPicb = new PictureBox();
        PictureBox MinimizePicb = new PictureBox();
        PictureBox TitleIconPicb = new PictureBox();

        /// <summary>
        /// 重写 RoundRadius 属性，计算Panel内部根据圆角应该设置的padding
        /// </summary>
        public override int RoundRadius
        {
            get { return base.RoundRadius; }
            set
            {
                //Padding = new Padding(value);
                base.RoundRadius = value;
            }
        }

        public TitleBarPro()
        {
            Dock = DockStyle.Top;
            Height = 30;

            Padding = new Padding(0);

            ClosePicb.SizeMode = MaximizeNormalPicb.SizeMode = MinimizePicb.SizeMode = TitleIconPicb.SizeMode = PictureBoxSizeMode.Zoom;
            ClosePicb.BackColor = MaximizeNormalPicb.BackColor = MinimizePicb.BackColor = TitleIconPicb.BackColor = Color.Transparent;


        }
    }
}
