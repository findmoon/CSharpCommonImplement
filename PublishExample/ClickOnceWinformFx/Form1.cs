using System;
using System.Drawing;
using System.Windows.Forms;


namespace ClickOnceWinformFx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // System.Drawing 中的Color
                var color = System.Drawing.Color.Red;

                var color1 = System.Drawing.Color.FromName("red");
                var color2 = System.Drawing.Color.FromName(color.Name);
                var color3 = System.Drawing.Color.FromKnownColor(KnownColor.Red);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
