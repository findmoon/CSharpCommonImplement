using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HEXAndDECInterConvert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            decToTxt.ReadOnly = decToTxt2.ReadOnly = true;
            decToTxt.BackColor = decToTxt2.BackColor = SystemColors.Window;


            hexToTxt.ReadOnly= true;
            hexToTxt.BackColor = SystemColors.Window;

            hexFromTxt.TextChanged += HexFromTxt_TextChanged;
            decFromTxt.TextChanged += DecFromTxt_TextChanged;
        }

        private void DecFromTxt_TextChanged(object sender, EventArgs e)
        {
            var decStr = decFromTxt.Text.Trim();
            if (!int.TryParse(decStr, out int dec))
            {
                // 不是数字或者不能正确的转为数字则清空
                decFromTxt.Text =hexToTxt.Text = "";
                return;
            }

            var hex1 = HexDecConvert.DecimalToHex(dec);
            hexToTxt.Text = hex1;

            var tmp = Convert.ToString(dec, 16); // ab..f等为小写表示
            // X2表示两位的16进制，比如5表示为05
            // X表示16进制格式
            var tmp2 = dec.ToString("X2");
            var tmp3 = string.Format("{0:X2}", dec);

            // 验证下实现是否正确
            // dec.ToString("x2") 小写的2位十六进制
            if (hex1 != Convert.ToString(dec, 16).ToUpper() || hex1 != dec.ToString("X") || hex1 != string.Format("{0:X}",dec))
            {
                throw new Exception("转换错误!");
            }
        }

        private void HexFromTxt_TextChanged(object sender, EventArgs e)
        {
            var hex = hexFromTxt.Text.Trim();
            var dec1 = HexDecConvert.HexToDecimal(hex);
            var dec2 = HexDecConvert.HexaToDecimal(hex);
            decToTxt.Text = dec1.ToString();
            decToTxt2.Text = dec2.ToString();

            // 验证下实现是否正确
            if (dec1 != dec2 || dec1 != Convert.ToInt32(hex,16) || dec1!=int.Parse(hex,System.Globalization.NumberStyles.HexNumber))
            {
                throw new Exception("转换错误!");
            }
        }
    }
}
