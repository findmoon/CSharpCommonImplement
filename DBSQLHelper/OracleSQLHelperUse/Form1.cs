using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace OracleSQLHelperUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var ip = serverTxt.Text.Trim();
                var user = userTxt.Text.Trim();
                var pwd = pwdTxt.Text.Trim();
                var dbname = dbTxt.Text.Trim();

                //using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
                //{
                //    var resule = await sqlHelper.ExecuteQueryAsync("select * from t_did");
                //    // ...

                //    var testTableExists1 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t_did", "didqty");
                //    var testTableExists2 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t_did", "qty_No");
                //    var testTableExists3 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t_did");
                //    var testTableExists4 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t_did2");
                //}

                //using (var sqlHelper = new OracleSQLHelper())
                //{
                //    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
                //    {
                //        var resule = await sqlHelper.ExecuteQueryAsync("select * from t_did");
                //        sqlHelper.Conn.Dispose();
                //        // ...
                //    }
                //}

                //using (var sqlHelper = new OracleSQLHelper())
                //{

                //    if (sqlHelper.Initializer("ConnectionString"))
                //    {

                //    }

                //}

                #region ��������ѯ
                using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
                {
                    // CREATE ���Ҫ����ִ�У����ܺ� INSERT һ��
                    var createSql = @"CREATE TABLE Product(
	product_id char(4) NOT NULL PRIMARY KEY,
	product_name varchar(100) NOT NULL,
	product_type varchar(32) NOT NULL,
	sale_price int NULL,
	purchase_price int NULL,
	regist_date date NULL
)";
                    var cresult = await sqlHelper.ExecuteNonQueryAsync(createSql);
                    // ������䷵�� -1
                    Debug.WriteLine($"create result: {cresult}");

                    // ����һ��ִ�ж��INSERT��䣬Ҫ����ִ��
                    var iniInsertSql = "INSERT INTO Product values('0006','����','�����þ�',5000,NULL,TO_DATE('2009-09-20','yyyy-mm-dd'))";
                    var inResult = await sqlHelper.ExecuteNonQueryAsync(iniInsertSql);
                    iniInsertSql = "INSERT INTO Product values('0007','���˰�','�����þ�',8800,395,TO_DATE('2008-04-28','yyyy-mm-dd'))";
                    inResult = await sqlHelper.ExecuteNonQueryAsync(iniInsertSql);
                    Debug.WriteLine($"insert result: {inResult}");

                    // select * from Product where product_id='0006' or sale_price=8800; 
                    var selectSql = "select * from Product where product_id=:id or sale_price=:price";
                    var resuleDt = await sqlHelper.ExecuteQueryAsync(selectSql, new OracleParameter[]
                    {
                        new OracleParameter(":id","0006"),
                        new OracleParameter(":price",8800)
                    });

                    foreach (DataColumn col in resuleDt.Columns)
                    {
                        Debug.Write(col.ColumnName + "\t");
                    }
                    Debug.WriteLine(null);
                    foreach (DataRow row in resuleDt.Rows)
                    {
                        foreach (DataColumn col in resuleDt.Columns)
                        {
                            // ʹ�� row.Field<T>(name/idx/..) ��ȡ�ֶ�ֵ����Ҫ�ж��Ƿ�DBNull -- row[col.ColumnName] is DBNull�� ��ΪDBNull����ת�� �� ����ָ����ȷ��T��������

                            Debug.Write(row[col.ColumnName].ToString() + "\t");
                        }
                        Debug.WriteLine(null);
                    }

                    // ...

                    // insert Product values('0009','Բ���','�칫��Ʒ',15,10,GETDATE());

                    // ��������ѯ��ʹ�õ� ������ ����Ϊ Oracle��صĹؼ��֣����� date type �ȣ����򱨴�
                    //var insertSql = "insert into Product values(:id,:name,:mytype,:price,:purchase,TO_DATE(:mydate,'yyyy-mm-dd'))";
                    var insertSql = "insert into Product values(:id,:name,:mytype,:price,:purchase,:mydate)";
                    var rowNum = await sqlHelper.ExecuteNonQueryAsync(insertSql, new OracleParameter[]
                    {
                        // new OracleParameter(":id","0010"),
                        new OracleParameter(":id","0011"),
                        new OracleParameter(":name","Բ���"),
                        new OracleParameter(":mytype","�칫��Ʒ"),
                        new OracleParameter(":price",15),
                        new OracleParameter(":purchase",10),
                        //new OracleParameter(":mydate",DateTime.Now.ToString("yyyy-MM-dd"))
                        new OracleParameter(":mydate",DateTime.Now)
                    });

                    Debug.WriteLine($"affect row num: {rowNum}");

                    // select product_id from Product where product_id='0010';
                    var selectOneSql = "select product_id from Product where product_id=:id";
                    var resuleValue = await sqlHelper.ExecuteScalarAsync(selectOneSql, new OracleParameter[]
                    {
                    new OracleParameter(":id","0010")
                    });
                    Debug.WriteLine($"product_id: {resuleValue}");
                  
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var ip = serverTxt.Text.Trim();
                var user = userTxt.Text.Trim();
                var pwd = pwdTxt.Text.Trim();
                var dbname = dbTxt.Text.Trim();


                #region ��������ѯ
                using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
                {
                    // ���������������С�������������

                }
                    #endregion
                }
            catch (Exception ex)
            {

            }
        }
    }
}