using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTransparent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            #region From透明效果
            //// Form不支持透明的背景色，会报错
            //// BackColor = Color.Transparent;

            //// 无效
            ////SetStyle(ControlStyles.UserPaint, true);
            ////SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            ////SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            ////this.BackColor = Color.Transparent;
            ////this.TransparencyKey = BackColor;

            //// 透明窗体内的透明控件的背景图边缘 会有 BackColor 颜色的白边
            this.BackColor = Color.Empty;
            this.TransparencyKey = BackColor;

            ////FormBorderStyle = FormBorderStyle.None;
            #endregion

            #region button透明
            button1.FlatStyle = FlatStyle.Flat;
            button1.BackColor = Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatAppearance.CheckedBackColor = Color.Transparent;
            button1.FlatAppearance.BorderSize = 0; 
            #endregion

            pictureBox1.Image = Properties.Resources.start;
            // 控件背景直接设置透明
            pictureBox1.BackColor = Color.Transparent;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            // 控件背景直接设置透明
            label1.BackColor = Color.Transparent;

            invisible_Button1.Text = "我是自定义控件";
            
           
            label2.Image = Properties.Resources.pause1;
            label2.ImageAlign = ContentAlignment.MiddleCenter;


            transparentTextBox1.Text = "我是背景透明的TextBox";

            var loadImg = new LoadImgTransparentBtn();
            loadImg.Show();
            var et = new EnterThrough();
            et.Show();
        }
        ///// <summary>
        ///// override OnPaintBackground方法，并且内容空实现，会导致背景黑或全控件色，但不会透明
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    //empty implementation
        //}
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    e.Graphics.FillRectangle(Brushes.LimeGreen, e.ClipRectangle);
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new CodeSetBtnBackColorImg();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new LoadImgTransparentBtn();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new UseTranspCtrl();
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            transparentLabel1.Text += "1";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var form = new UseExtendedPanel();
            form.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var random = new Random();
            button7.ToTransparent(Color.FromArgb(random.Next(0, 1), random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            EnterThrough2 enterThrough2=new EnterThrough2();
            enterThrough2.Show();
        }
    }
}
