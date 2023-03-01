using Microsoft.Win32;
using RegistryUse.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            FormClosing += Form1_FormClosing;

            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFormDataFromRegistryHKCU();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormDataToRegistryHKCU();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SaveFormDataToRegistryHKCU()
        {
            Registry.SetValue($"HKEY_CURRENT_USER\\Software\\{Application.ProductName}", "FormSize", Size);
            Registry.SetValue($"HKEY_CURRENT_USER\\Software\\{Application.ProductName}", "FormPosition", Location);
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadFormDataFromRegistryHKCU()
        {
            using (var hkcuFormReg=Registry.CurrentUser.OpenSubKey($"Software\\{Application.ProductName}"))
            {
                try
                {
                    var size = (Size)hkcuFormReg.GetValue("FormSize");
                    var position = (Point)hkcuFormReg.GetValue("FormPosition");
                    

                    Size= size;
                    Location = position;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFormDataToRegistryHKCU();
            //Registry.CurrentConfig
        }
    }
}
