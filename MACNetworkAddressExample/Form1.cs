using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MACNetworkAddressExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var macs=MacAddressHelper.GetMacByWMI();
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var macs = MacAddressHelper.GetMacByWMI(false,containINCName:true);
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var macs = MacAddressHelper.GetMacByWMI(false);
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var macs = MacAddressHelper.GetMacByNetworkInterface();
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var mac = MacAddressHelper.GetMacBySendArp("192.168.104.151");
            listBox1.Items.Clear();
            listBox1.Items.Add(mac);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var macs = MacAddressHelper.GetMacByIpConfig();
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var macs = MacAddressHelper.GetMacByIpConfig2();
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var macs = MacAddressHelper.GetMacByNetworkInterface2("192.168.104.160");
            listBox1.Items.Clear();
            foreach (var mac in macs)
            {
                listBox1.Items.Add(mac);
            }
        }
    }
}
