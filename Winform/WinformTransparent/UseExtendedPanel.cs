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
    public partial class UseExtendedPanel : Form
    {
        public UseExtendedPanel()
        {
            InitializeComponent();

            extendedPanel1.BackColor = Color.Red;

     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("3");
        }
    }
}
