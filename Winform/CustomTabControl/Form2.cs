using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomTabControl
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            var btn = new Button();
            btn.Click += Btn_Click;

            // 为指定的控件触发Click事件
            //InvokeOnClick(btn, EventArgs.Empty);

            MethodInfo onClick = btn.GetType().GetMethod("OnClick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            onClick.Invoke(btn, new object[] { new EventArgs() });


            textBox.TextChanged += TextBox_TextChanged;

            MethodInfo onTextChanged = textBox.GetType().GetMethod("OnTextChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            onTextChanged.Invoke(textBox, new object[] { new EventArgs() });

            Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //SendKeys.Send("{ENTER}"); //SendKeys.Send("{INSERT}");
            SendKeys.SendWait("{ENTER}");
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("你好");
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Btn");
        }
    }
}
