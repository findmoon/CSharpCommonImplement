using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationCustom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            // 设置通知的数量
            //Form_Alert.AlertFormNum = 8;
            //Form_Alert.MoveEntry = false;
        }
        Random random = new Random();
        private void button1_Click(object sender, EventArgs e)
        {
            Form_Alert.ShowNotice("这是一条成功的消息", MsgType.Success, new Font(FontFamily.Families[random.Next(0, FontFamily.Families.Length)], (float)(10.0+10.0*random.NextDouble())));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form_Alert.ShowNotice("警告！警告的消息", MsgType.Warning, new Font(FontFamily.Families[random.Next(0, FontFamily.Families.Length)], (float)(10.0 + 10.0 * random.NextDouble())));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_Alert.ShowNotice("发生了错误，禁止！", MsgType.Error, new Font(FontFamily.Families[random.Next(0, FontFamily.Families.Length)], (float)(10.0 + 10.0 * random.NextDouble())));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form_Alert.ShowNotice("一条普通的信息记录", MsgType.Info, new Font(FontFamily.Families[random.Next(0, FontFamily.Families.Length)], (float)(10.0 + 10.0 * random.NextDouble())));
        }
    }
}
