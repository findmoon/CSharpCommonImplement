using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlButton
{
    public partial class TestForm : Form
    {
        /// <summary>
        /// 实现窗体动画效果的函数
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="dwTime">动画持续时间</param>
        /// <param name="dwFlags">动画类型</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        public TestForm()
        {
            InitializeComponent();

            Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            using (var g = e.Graphics)
            {
                var rect = new Rectangle(10, 10, 200, 50);
                g.FillRectangle(new SolidBrush(Color.Black), rect);

                g.DrawText(rect, "我是测试居中效果的文字", Color.White, Font);
            }
        }

        private void buttonPro1_Click(object sender, EventArgs e)
        {

        }
    }
}
