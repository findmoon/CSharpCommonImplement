using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlRound
{
    public partial class CustomPanelTest : Form
    {
        public CustomPanelTest()
        {
            InitializeComponent();

            //this.FormBorderStyle = FormBorderStyle.None;
            //Paint += Form_Paint;
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            Region = new Region(ClientRectangle.GetRoundedRectPath(20));// 无边框窗体下似乎无锯齿
        }

    }
}
