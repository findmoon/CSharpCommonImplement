using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            msgLabel.ForeColor = Color.White;
            msgLabel.BackColor = Color.MediumPurple;

            var font =new Font(msgLabel.Font.FontFamily,msgLabel.Font.Size+1,msgLabel.Font.Style);
            msgLabel.Font = font;

            // Button的Click和MouseClick只支持鼠标左键触发；如要区分鼠标右键、中间键等事件，参考Mouse相关事件
            btn.Click += Btn_Click;
            btn.MouseClick += Btn_MouseClick;

            // 除Button外，其他控件(包括Form)的Click、MouseClick事件都可以由鼠标的任意键触发
            // MouseClick即鼠标事件（所有的鼠标事件）参数都包含鼠标的属性，用于获取位置、哪个键等
            // Click的事件参数可以转换为MouseEventArgs，从而获取更多信息（一般就直接使用MouseClick了）
            // 先触发Click，再触发MouseClick
            msgLabel.Click += MsgLabel_Click;
            msgLabel.MouseClick += MsgLabel_MouseClick;

            Click += Form1_Click;
            MouseClick += Form1_MouseClick;

            //鼠标事件
            btn.MouseEnter += Btn_MouseEnter;
            btn.MouseHover += Btn_MouseHover;
            btn.MouseMove += Btn_MouseMove;
            btn.MouseDown += Btn_MouseDown;
            btn.MouseUp += Btn_MouseUp;
            btn.MouseLeave += Btn_MouseLeave;

            btn.MouseCaptureChanged += Btn_MouseCaptureChanged;
            //btn.MouseDoubleClick;
            //btn.MouseWheel;

            MouseMove += Form1_MouseMove;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"鼠标的屏幕坐标：{MousePosition}；鼠标的窗体坐标：{PointToClient(MousePosition)}";
            var btnScreenPotion = PointToScreen(new Point(btn.Left, btn.Top));// Button控件在屏幕坐标的位置
        }

        private void Btn_MouseCaptureChanged(object sender, EventArgs e)
        {
            msgLabel.Text = "MouseCaptureChanged事件;";
        }

        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            msgLabel.Text += "Button的MouseUp事件;";
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            msgLabel.Text += "Button的MouseDown事件;";
        }

        private void Btn_MouseMove(object sender, MouseEventArgs e)
        {
            if (!msgLabel.Text.Contains("MouseMove"))
            {
                msgLabel.Text += "Button的MouseMove鼠标在按钮上移动事件;";
            }


        }

        private void Btn_MouseHover(object sender, EventArgs e)
        {
            if (!msgLabel.Text.Contains("MouseHover"))
            {
                msgLabel.Text += "Button的MouseHover事件;";
            }
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            btn.BackColor = Color.Moccasin;
            msgLabel.Text += "Button的MouseEnter事件;";
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            btn.BackColor = SystemColors.Control;
            msgLabel.Text += "Button的MouseLeave事件;";
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            msgLabel.Text += "窗体的MouseClick事件;";
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            msgLabel.Text += "窗体的Click事件;";
            // CLick事件中可以将事件参数EventArgs转换为MouseEventArgs类型
            var me = (MouseEventArgs)e;
           
        }

        private void MsgLabel_MouseClick(object sender, MouseEventArgs e)
        {
            msgLabel.Text += "MsgLabel的MouseClick事件;";
        }

        private void MsgLabel_Click(object sender, EventArgs e)
        {
            msgLabel.Text += "MsgLabel的Click事件;";
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            msgLabel.Text += "Button的Click事件;";
        }

        private void Btn_MouseClick(object sender, MouseEventArgs e)
        {
            msgLabel.Text += "Button的MouseClick事件;";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            msgLabel.Text = "";
        }
    }
}
