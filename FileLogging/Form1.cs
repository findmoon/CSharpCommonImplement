using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileLogging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Log.LogTxtBox = textBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log.LogTxtBoxMaxLines = 1;
            var count = new Random().Next(10, 50);
            for (int i = 0; i < count; i++)
            {
                Log.DealLogInfoAsync(RandomStr.GetRandomGBStr(10, 30));
            }
        }
    }
}
