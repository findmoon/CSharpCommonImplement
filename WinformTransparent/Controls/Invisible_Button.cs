using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTransparent
{
    public partial class Invisible_Button : UserControl
    {
        public override string Text
        {
            get => label.Text;
            set
            {
                label.Text = value;
            }
        }

        private Label label = new Label();

        public Invisible_Button()
        {
            InitializeComponent();

             
            label.Dock = DockStyle.Fill;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(label);
        }
    }
}
