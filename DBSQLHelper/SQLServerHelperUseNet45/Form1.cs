using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLServerHelperUseNet45
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ip = serverTxt.Text.Trim();
            var user = userTxt.Text.Trim();
            var pwd = pwdTxt.Text.Trim();
            var dbname = dbTxt.Text.Trim();

            var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname);

            var resule = sqlHelper.ExecuteQuery("select * from t");
        }
    }
}
