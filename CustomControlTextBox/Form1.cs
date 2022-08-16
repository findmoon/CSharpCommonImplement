using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlTextBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var test = new PlaceHolderTest();
            test.Show();

            textBoxProNo2.Parent = panel1;
            textBox1.Multiline = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollBars = ScrollBars.Vertical;
        }
    }
}
