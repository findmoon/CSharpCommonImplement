using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIDExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             
            var guid = Guid.NewGuid();
            var k=guid.ToString();
            var k1=guid.ToString("D");
            var k2=guid.ToString("N");
            var k3=guid.ToString("B");
            var k4=guid.ToString("P");
            var k5=guid.ToString("X");
            MessageBox.Show($@"{k}
{k1}
{k2}
{k3}
{k4}
{k5}");

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var guidConv = new GuidConverter();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var uniStr = Guid.NewGuid().ToString("N"); // 3414a7f91bf64522bf4f156cdf68f6f8
            var uniFile = Guid.NewGuid().ToString("N") + ".txt"; // 4b64c2d432c545b79a4faa525a63beda.txt
        }
    }
}
