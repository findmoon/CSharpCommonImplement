using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyEventTest
{
    public partial class Form1 : Form
    {
        int keyDonwCnt = 0;
        int keyPressCnt = 0;
        int keyUpCnt = 0;

        public Form1()
        {
            InitializeComponent();

            keyTestTxt.KeyDown += KeyTestTxt_KeyDown;
            keyTestTxt.KeyPress += KeyTestTxt_KeyPress;
            keyTestTxt.KeyUp += KeyTestTxt_KeyUp;
        }


        private void KeyTestTxt_KeyDown(object sender, KeyEventArgs e)
        {
            keyDonwCnt = 1;
            keyPressCnt = 0;
            keyUpCnt = 0;
            keyEventLabel.Text += "KeyDown事件;";
        }

        private void KeyTestTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            keyEventLabel.Text += "KeyPress事件;";
        }

        private void KeyTestTxt_KeyUp(object sender, KeyEventArgs e)
        {
            keyEventLabel.Text += "KeyUP事件;";
        }
    }
}
