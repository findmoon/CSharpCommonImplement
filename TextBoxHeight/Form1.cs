using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextBoxHeight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //textBox1.AutoSize = false;
            //textBox1.Height = 28;

            //numericUpDown1.AutoSize = false;
            //numericUpDown1.Height = 28;


            //textBox1.Multiline = true;
            //textBox1.Height = 28;
            //textBox1.KeyDown += TextBox1_KeyDown;


        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) // 或 (int)e.KeyCode == 13 或 e.KeyValue == 13
            //{
            //    // 指示按键事件是否应传递到基础控件
            //    // 如果为true，表示(Key)按键事件不应该发送到该控件
            //    e.SuppressKeyPress = true;
            //}

            // 禁止输入数字
            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && e.Modifiers != Keys.Shift) || (e.KeyCode>=Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
