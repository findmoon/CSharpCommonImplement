using HelperCollections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelperCollectionsUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShortcutHelper.CreateUrlShortcut(".\\", "https://cn.bing.com/");

            ShortcutHelper.CreateCurrShortcut(".\\",icon: "D:\\SoftWareDevelope\\CSharp\\csharp-common-implement\\assert\\icons\\mytest2.ico");

            MessageBox.Show("结束");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShortcutHelper_WSH.CreateCurrShortcut(".\\");
            // 文件夹
            ShortcutHelper_WSH.CreateShortcut(".\\", "D:\\SoftWareDevelope\\CSharp\\csharp-common-implement\\assert");
            ShortcutHelper_WSH.CreateShortcut(".\\", "https://cn.bing.com/search?q=%E4%BD%A0%E5%A5%BD","url测试");
            ShortcutHelper_WSH.CreateShortcut(".\\", "https://cn.bing.com/");

            MessageBox.Show("结束");
        }
    }
}
