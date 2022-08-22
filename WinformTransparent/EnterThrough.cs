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
    public partial class EnterThrough : Form
    {
        public EnterThrough()
        {
            InitializeComponent();

            TopLevel=true;
            TopMost=true;

            BackColor = Color.Empty; 
            TransparencyKey = BackColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你好");
        }
    }
}
