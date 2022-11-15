using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace GetIPFromNetWork
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBoxGetAllUnicastAddresses2.Items.Clear();
            listBoxGetAllUnicastAddresses_New.Items.Clear();
            listBoxLocalIps.Items.Clear();

            var Adresses = IPLookup.GetAllUnicastAddresses2();
            listBoxGetAllUnicastAddresses2.Items.AddRange(Adresses.OrderBy(i=>i.ToString()).ToArray());
          

            var Adresses2 = IPLookup.GetAllUnicastAddresses_New();
            listBoxGetAllUnicastAddresses_New.Items.AddRange(Adresses2.OrderBy(i => i.ToString()).ToArray());

            var localIps = IPLookup.GetLocalIps();
            listBoxLocalIps.Items.AddRange(localIps.OrderBy(i => i.ToString()).ToArray());
        }
    }
}
