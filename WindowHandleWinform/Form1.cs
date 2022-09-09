using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowHandleWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            button3.KeyUp += Button3_KeyUp;

            var frmHwnd = this.Handle;

            
        }

        private void Button3_KeyUp(object sender, KeyEventArgs e)
        {
            //e.KeyValue
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击测试");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("点击测试，是/否", "测试", MessageBoxButtons.YesNo)== DialogResult.No)
            {
                MessageBox.Show("点击了No");
            };
        }
    }
}
