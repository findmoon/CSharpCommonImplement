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
        private readonly ErrorProvider errProvider;

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

            textBox1.TextChanged += TextBox1_TextChanged;


            errProvider = new ErrorProvider();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            var txtBox= sender as TextBox;

            // 取消错误提示
            errProvider.SetError(txtBox, "");
            // 不为空时
            if (!string.IsNullOrWhiteSpace(txtBox.Text))
            {
                // "0123456789" 也可以改为其他需要禁止输入的内容文字
                var canNotInput = "0123456789";
                if (canNotInput.Contains(txtBox.Text[txtBox.Text.Length - 1]))
                {
                    // 删除最后一个字符【两种方式】
                    // txtBox.Text = txtBox.Text.Remove(txtBox.Text.Length - 1);
                    txtBox.Text = txtBox.Text.Substring(0, txtBox.Text.Length - 1);

                    // 重新赋值后光标会出现在最前面
                    //txtBox.Focus(); // 通常需要设置文本框获取焦点在使用下面的Select到末尾，但是上面赋值已经有了光标，所以不需要
                    // 设置光标位置到文本末尾
                    txtBox.Select(txtBox.TextLength, 0);
                    // 滚动控件到光标处
                    txtBox.ScrollToCaret();

                    // 给定一个错误提示
                    //errProvider.BlinkStyle= ErrorBlinkStyle.AlwaysBlink;
                    errProvider.SetError(txtBox, $"不行允许输入此中的字符：{canNotInput}");
                }
            }
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
