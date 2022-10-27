using System.Data;

namespace OracleSQLHeplerUse
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
                var ip = serverTxt.Text.Trim();
                var user = userTxt.Text.Trim();
                var pwd = pwdTxt.Text.Trim();
                var dbname = dbTxt.Text.Trim();

                using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
                {
                    var resule = sqlHelper.ExecuteQuery("select * from t_did");
                    // ...

                    var testTableExists1 = sqlHelper.ExistsDBOrTableOrCol("", "t_did", "didqty");
                    var testTableExists2 = sqlHelper.ExistsDBOrTableOrCol("", "t_did", "qty_No");
                    var testTableExists3 = sqlHelper.ExistsDBOrTableOrCol("", "t_did");
                    var testTableExists4 = sqlHelper.ExistsDBOrTableOrCol("", "t_did2");
                }

                using (var sqlHelper = new OracleSQLHelper())
                {
                    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
                    {
                        var resule = sqlHelper.ExecuteQuery("select * from t_did");
                        sqlHelper.Conn.Dispose();
                        // ...
                    }
                }

                //using (var sqlHelper = new OracleSQLHelper())
                //{

                //    if (sqlHelper.Initializer("ConnectionString"))
                //    {

                //    }

                //}
            }
            catch (Exception ex)
            {

            }
        }
    }
}