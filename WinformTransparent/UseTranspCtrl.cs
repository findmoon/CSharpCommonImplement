using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTransparent
{
    public partial class UseTranspCtrl : Form
    {
        public UseTranspCtrl()
        {
            InitializeComponent();

            transpCtrl1.drag = true;
            transpCtrl1.BackColor = Color.Red;
            transpCtrl1.Opacity = 50;

        }
    }
}
