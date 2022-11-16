using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FullScreen
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // 无边框最大化的全屏
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            // label占满form并文字居中
            label1.Dock = DockStyle.Fill;
            label1.AutoSize = false;
            label1.TextAlign = ContentAlignment.MiddleCenter;

            // Esc退出全屏
            KeyUp += MainForm_KeyUp;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
