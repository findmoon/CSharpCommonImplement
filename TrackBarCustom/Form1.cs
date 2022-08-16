using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackBarCustom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitControls();
        }

        private void InitControls()
        {
            colorTxt.ReadOnly = true;
            colorTxt.BackColor = SystemColors.Window;

            CopyBtn.FlatStyle = FlatStyle.Flat;
            CopyBtn.FlatAppearance.BorderSize = 0;
            CopyBtn.BackgroundImage = Properties.Resources.copy;
            CopyBtn.BackgroundImageLayout = ImageLayout.Stretch;
            CopyBtn.Text = "";
            CopyBtn.Click += CopyBtn_Click;

            

            rColorTrackBar.Minimum= gColorTrackBar.Minimum= bColorTrackBar.Minimum = alphaColorTrackBar.Minimum = 0;
            rColorTrackBar.Maximum= gColorTrackBar.Maximum= bColorTrackBar.Maximum = alphaColorTrackBar.Minimum = 255;
            rColorTrackBar.Maximum = gColorTrackBar.Maximum= bColorTrackBar.Maximum = 255;
            
            alphaColorTrackBar.Value = 255;

            //Color.FromArgb()
        }

        private void CopyBtn_Click(object sender, EventArgs e)
        {
            // 复制colorTxt
            colorTxt.SelectAll();
            // 复制选择的文本
            colorTxt.Copy();
        }
    }
}
