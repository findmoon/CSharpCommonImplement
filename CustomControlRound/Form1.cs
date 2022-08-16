using CMControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var customPanle =new CustomPanelTest();
            customPanle.Show();

            var roundForm = new RoundForm();
            roundForm.Show();
            var gdiAPIRound = new GDIAPIRound();
            gdiAPIRound.Show();
            
            var roundFormSmooth = new RoundFormSmooth();
            roundFormSmooth.Show();
            roundFormSmooth.BringToFront();
            var roundedForm = new RoundedForm();
            roundedForm.Show();
            roundedForm.BringToFront();

            //var roundTxt = new RoundTextBoxTest();
            //roundTxt.Show();

            panel1.Paint += Panel1_Paint;
            panel3.Paint += Panel1_Paint;
            panel4.Paint += Panel1_Paint;
            panel5.Paint += Panel1_Paint;
            panel6.Paint += Panel1_Paint;
            panel7.Paint += Panel1_Paint;
            panel8.Paint += Panel1_Paint;
            panel9.Paint += Panel1_Paint;

            panel2.Paint += Panel2_Paint;

            panel1.BackColor = panel2.BackColor = Color.Transparent;

            button1.Paint += Button1_Paint;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            //button1.FlatAppearance.BorderColor = SystemColors.Control;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatAppearance.CheckedBackColor = Color.Transparent;

            

            button2.Paint += Button1_Paint;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            //button2.FlatAppearance.BorderColor = SystemColors.ButtonFace;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.CheckedBackColor = Color.Transparent;

            button3.Paint += Button1_Paint;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            //button3.FlatAppearance.BorderColor = SystemColors.ButtonFace;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.CheckedBackColor = Color.Transparent;

            label1.Paint += Label1_Paint;

            label1.BackColor = label2.BackColor = Color.Transparent;

            //checkBox1.Paint += CheckBox1_Paint;

            label2.Text = "我是Label显示在圆角按钮上";
            label2.Parent = button1;
            label2.AutoSize = false;
            label2.Dock = DockStyle.Fill;
            label2.BackColor = Color.Transparent;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.ForeColor = Color.Wheat;
        }

        //private void CheckBox1_Paint(object sender, PaintEventArgs e)
        //{
        //    Draw(e.ClipRectangle, e.Graphics, 18, false, Color.FromArgb(180, 200, 210), Color.FromArgb(120, 120, 100));
        //    
        //}

        private void Label1_Paint(object sender, PaintEventArgs e)
        {
            var l = (Label)sender;
            //e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle,  18, Color.FromArgb(180, 200, 210), Color.FromArgb(120, 120, 100));
            e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(0,0, l.Width,l.Height),  18, Color.FromArgb(180, 200, 210), Color.FromArgb(120, 120, 100));
        }

        private void Button1_Paint(object sender, PaintEventArgs e)
        {
            var ctl = (Button)sender;
            e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(0, 0, ctl.Width, ctl.Height), 18, Color.FromArgb(0, 122, 204), Color.FromArgb(8, 39, 57));

            ((Button)sender).NotifyDefault(false); // 去除窗体失去焦点时最新激活的按钮边框外观样式
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            var rectAlign = RectangleAlign.RightTop;
            switch (panel.Name)
            {
                case "panel3":
                    rectAlign = RectangleAlign.AboveLeft;
                    break;
                case "panel4":
                    rectAlign = RectangleAlign.AboveRight;
                    break;
                case "panel5":
                    rectAlign = RectangleAlign.BelowLeft;
                    break;
                case "panel6":
                    rectAlign = RectangleAlign.BelowRight;
                    break;
                case "panel7":
                    rectAlign = RectangleAlign.LeftBottom;
                    break;
                case "panel8":
                    rectAlign = RectangleAlign.LeftTop;
                    break;
                case "panel9":
                    rectAlign = RectangleAlign.RightBottom;
                    break;
                default:
                    break;
            }

            //var rect=e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle, 18, Color.FromArgb(90, 143, 0), Color.FromArgb(41, 67, 0),true, rectAlign);
            var rect=e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(0, 0, panel.Width, panel.Height), 18, Color.FromArgb(90, 143, 0), Color.FromArgb(41, 67, 0),true, rectAlign);
            //e.Graphics.DrawText(e.ClipRectangle, "这是一个Panel控件，非常适合显示消息", Color.White, panel.Font);
            // 使用合适的区域
            e.Graphics.DrawText(rect, "这是一个Panel控件，非常适合显示消息", Color.White, panel.Font);
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            //var rect = e.Graphics.DrawFillRoundRectAndCusp(e.ClipRectangle, 18, Color.FromArgb(113, 113, 113), Color.FromArgb(0, 0, 0));
            var rect = e.Graphics.DrawFillRoundRectAndCusp(new Rectangle(0, 0, panel.Width, panel.Height), 18, Color.FromArgb(113, 113, 113), Color.FromArgb(0, 0, 0));
            
            //e.Graphics.DrawText(e.ClipRectangle, "这是一个Panel控件，非常适合显示消息", Color.White, panel.Font);
            // 使用合适的区域
            e.Graphics.DrawText(rect, "我是一个圆角Panel", Color.White, panel.Font);
        }


    }
}
