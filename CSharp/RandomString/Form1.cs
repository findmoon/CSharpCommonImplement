using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomString
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var strLength = random.Next(10, 100);
            // random.NextDouble();

            var strBuilder = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                strBuilder.Append(str[random.Next(62)]);
            }
            resultTxt.Text = strBuilder.ToString();

            #region random
            //var random = new Random();
            //var strLength = random.Next(10,100);
            //var strBuilder = new StringBuilder();
            //char letter;

            ////for (int i = 0; i < strLength; i++)
            ////{
            ////    // 生成随机字符
            ////    //random.Next(0,26);
            ////    var letterNum = random.Next(26);
            ////    if (random.Next(10)>=5)
            ////    {
            ////        letter = Convert.ToChar(letterNum + 97);
            ////    }
            ////    else
            ////    {
            ////        letter = Convert.ToChar(letterNum + 65);
            ////    }
            ////    strBuilder.Append(letter);
            ////}

            //for (int i = 0; i < strLength; i++)
            //{
            //    // 生成随机字符
            //    //random.Next(0,26);
            //    var letterNum = random.Next(26);
            //    var type = random.Next(3);
            //    if (type == 0)
            //    {
            //        strBuilder.Append(Convert.ToChar(letterNum + 97));
            //    }
            //    else if(type == 1)
            //    {
            //        strBuilder.Append(Convert.ToChar(letterNum + 65));
            //    }
            //    else
            //    {
            //        strBuilder.Append(random.Next(10));
            //    }
            //}
            //resultTxt.Text = strBuilder.ToString(); 
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resultTxt.Text = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resultTxt.Text = Path.GetTempFileName();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region test1
            //var gb = Encoding.GetEncoding("gb2312");

            //var bytes1 = gb.GetBytes("好");
            //var bytes2 = gb.GetBytes("啊");

            //Console.WriteLine(Convert.ToString(bytes1[0], 16) + Convert.ToString(bytes1[1], 16));
            //Console.WriteLine(Convert.ToString(bytes1[0]) + Convert.ToString(bytes1[1]));
            //Console.WriteLine(Convert.ToString(bytes2[0], 16) + Convert.ToString(bytes2[1], 16));
            //Console.WriteLine(Convert.ToString(bytes2[0]) + Convert.ToString(bytes2[1])); 
            #endregion

            //var gb = Encoding.GetEncoding("gb2312");

            //var str1 = gb.GetString(new byte[] { Convert.ToByte(186), Convert.ToByte(195) });
            //var str2 = gb.GetString(new byte[] { Convert.ToByte(176), Convert.ToByte(161) });
            //var str3 = gb.GetString(new byte[] { Convert.ToByte(0xba), Convert.ToByte(0xc3) });
            //var str4 = gb.GetString(new byte[] { Convert.ToByte(0xb0), Convert.ToByte(0xa1) });

            //Console.WriteLine(str1);
            //Console.WriteLine(str2);
            //Console.WriteLine(str3);
            //Console.WriteLine(str4);

            //resultTxt.Text= RandomStr.GetBG2312ZH();
            //resultTxt.Text= RandomStr.GetBG2312ZH(true);
            //resultTxt.Text = RandomStr.GetRandomGBStr(1, 10);

            #region MyRegion
            //Random rnd = new Random();
            //resultTxt.Text = rnd.Next().ToString();

            //resultTxt.Text += "\r\n\r\n";

            //rnd = new Random();
            //resultTxt.Text += rnd.Next().ToString(); 
            #endregion

            //resultTxt.Text = CreateRandomBGStr(12);

            //MessageBox.Show($"{Encoding.Default.ToString()},{Encoding.UTF8.ToString()}");
        }

    }
}
