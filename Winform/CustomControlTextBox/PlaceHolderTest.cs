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
    public partial class PlaceHolderTest : Form
    {
        string placeHolderTxt = "我是提示信息，光标进入就消失";
        Color placeHolderColor = Color.DarkGray;

        public PlaceHolderTest()
        {
            InitializeComponent();

            // 初始化 simplePlaceHolderTxt
            simplePlaceHolderTxt.Text = placeHolderTxt;
            simplePlaceHolderTxt.Tag = simplePlaceHolderTxt.ForeColor;
            simplePlaceHolderTxt.ForeColor = placeHolderColor;
            simplePlaceHolderTxt.Enter += textBox_Enter;
            simplePlaceHolderTxt.Leave += textBox_Leave;
        }
        private void textBox_Enter(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == placeHolderTxt)
            {
                textBox.Text = "";
                textBox.ForeColor = (Color)textBox.Tag;
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeHolderTxt;
                textBox.Tag = textBox.ForeColor;
                textBox.ForeColor = placeHolderColor;
            }
        }
    }
}
