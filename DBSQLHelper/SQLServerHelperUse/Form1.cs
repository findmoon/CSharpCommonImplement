using Microsoft.Data.SqlClient;
using System.Data;

namespace SQLServerHelperUse
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

            using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
            {
                var resule = sqlHelper.ExecuteQuery("select * from product");
                // ...

                var tableExists1 = sqlHelper.ExistsDBOrTableOrCol("t", "test");
                var dbExists2 = sqlHelper.ExistsDBOrTableOrCol("shop","");
                var colExists3 = sqlHelper.ExistsDBOrTableOrCol("", "product", "sale_price");
                var colExists4 = sqlHelper.ExistsDBOrTableOrCol("", "product", "sale_price_No");
                var colExists5 = sqlHelper.ExistsDBOrTableOrCol("shop", "product", "sale_price");
                var tableExists6 = sqlHelper.ExistsDBOrTableOrCol("t", "test1");
            }

            using (var sqlHelper = new SQLServerHelper())
            {
                if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
                {
                    var resule = sqlHelper.ExecuteQuery("select * from test");
                    sqlHelper.Conn.Dispose();
                    // ...
                }
            }

            //using (var sqlHelper = new SQLServerHelper())
            //{

            //        if (sqlHelper.Initializer("ConnectionString"))
            //        {

            //        }

            //}
        }
    }
}