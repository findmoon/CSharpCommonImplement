using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{
    public class TransparentsControl: Control
    {
        public TransparentsControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }
        public TransparentsControl(int width,int height):this()
        {
            Width=width;
            Height=height;
        }
    }
}
