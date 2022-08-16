using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwoWayDataBindings
{
    public partial class Form1 : Form
    {
        //public string MyData
        //{
        //    get {
        //        return dataLabel.Text;
        //    }
        //    set
        //    {
        //        dataLabel.Text = value;
        //    }
        //}

        public string MyData
        {
            get=> dataLabel.Text;  
            set {
                dataLabel.Text = value;
                txt.Text = value;
            }
        }

        public Form1()
        {
            InitializeComponent();

            txt.TextChanged += Txt_TextChanged;

             var btn = MessageBoxButtons.AbortRetryIgnore;
           
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            MyData = txt.Text;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MyData = new Random().Next().ToString();
        }
    }
}
