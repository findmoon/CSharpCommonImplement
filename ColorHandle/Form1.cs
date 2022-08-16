using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorHandle
{
    public partial class Form1 : Form
    {
        ColorDialog colorDialog = new ColorDialog();

        public Form1()
        {
            InitializeComponent();

            InitControls();
        }

        private void InitControls()
        {
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.BackColor = Color.MediumPurple;
            button1.ForeColor = Color.White;
            button1.Font = new Font("SimHei", 11f, FontStyle.Regular);

            // 指定一个自定义颜色数组
            colorDialog.CustomColors= new int[]{6916092, 15195440, 16107657, 1836924,
               3758726, 12566463, 7526079, 7405793, 6945974, 241502, 2296476, 5130294,
               3102017, 7324121, 14993507, 11730944 };
            // Allows the user to select or edit a custom color.
            colorDialog.AllowFullOpen = true;
            // Allows the user to get help. (The default is false.)
            colorDialog.ShowHelp = true;

            rgbTxt.ReadOnly = rgbaTxt.ReadOnly = hexTxt.ReadOnly = hexNumberSignTxt.ReadOnly = decTxt.ReadOnly = hslTxt.ReadOnly = true;
            rgbTxt.BackColor = rgbaTxt.BackColor = hexTxt.BackColor = hexNumberSignTxt.BackColor = decTxt.BackColor = hslTxt.BackColor = SystemColors.Window;

            colorResultTxt.ReadOnly = true;
            colorResultTxt.BackColor= SystemColors.Window;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog()== DialogResult.OK)
            {
                var color= colorDialog.Color;
                colorBgLabel.BackColor = color;

                rgbTxt.Text = $"{color.R},{color.G},{color.B}";
                rgbaTxt.Text = $"{color.R},{color.G},{color.B},{color.A}";

                var hexStr = $"{Convert.ToString(color.R, 16)}{Convert.ToString(color.G, 16)}{Convert.ToString(color.B, 16)}";
                hexTxt.Text = hexStr;
                hexNumberSignTxt.Text = $"#{hexStr}";

                decTxt.Text = $"{color.R},{color.G},{color.B}";

                rgbTxt.Text = $"{color.R},{color.G},{color.B}";
                rgbTxt.Text = $"{color.R},{color.G},{color.B}";
            }
        }
    }
}
