using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace MiscellaneousTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //        new ToastContentBuilder()
            //.AddArgument("action", "viewConversation")
            //.AddArgument("conversationId", 9813)
            //.AddText("Andrew sent you a picture")
            //.AddText("Check this out, The Enchantments in Washington!")
            //.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater

            var dt = new DataTable();
            //dt.Clone();// 克隆结构
            //dt.Select(
        
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //unsafe 
            //{
            //    var num = 10;
            //    int* pNum = &num;
            //    Debug.WriteLine();
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string @int = "你好";
            int @string = 10;
        }
    }
}
