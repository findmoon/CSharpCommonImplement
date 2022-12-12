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

                #region 参数化查询
                using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
                {
                    // CREATE 语句要单独执行，不能和 INSERT 一起
                    var createSql = @"CREATE TABLE Product(
	product_id char(4) NOT NULL PRIMARY KEY,
	product_name varchar(100) NOT NULL,
	product_type varchar(32) NOT NULL,
	sale_price int NULL,
	purchase_price int NULL,
	regist_date date NULL
)";
                    var cresult = await sqlHelper.ExecuteNonQueryAsync(createSql);
                    // 创建语句返回 -1
                    Debug.WriteLine($"create result: {cresult}");

                    // 不能一次执行多个INSERT语句，要单独执行
                    var iniInsertSql = "INSERT INTO Product values('0006','叉子','厨房用具',5000,NULL,TO_DATE('2009-09-20','yyyy-mm-dd'))";
                    var inResult = await sqlHelper.ExecuteNonQueryAsync(iniInsertSql);
                    iniInsertSql = "INSERT INTO Product values('0007','擦菜板','厨房用具',8800,395,TO_DATE('2008-04-28','yyyy-mm-dd'))";
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
                            // 使用 row.Field<T>(name/idx/..) 获取字段值，需要判断是否DBNull -- row[col.ColumnName] is DBNull。 不为DBNull才能转换 且 必须指定正确的T数据类型

                            Debug.Write(row[col.ColumnName].ToString() + "\t");
                        }
                        Debug.WriteLine(null);
                    }

                    // ...

                    // insert Product values('0009','圆珠笔','办公用品',15,10,GETDATE());

                    // 参数化查询中使用的 参数名 不能为 Oracle相关的关键字，比如 date type 等，否则报错
                    //var insertSql = "insert into Product values(:id,:name,:mytype,:price,:purchase,TO_DATE(:mydate,'yyyy-mm-dd'))";
                    var insertSql = "insert into Product values(:id,:name,:mytype,:price,:purchase,:mydate)";
                    var rowNum = await sqlHelper.ExecuteNonQueryAsync(insertSql, new OracleParameter[]
                    {
                        // new OracleParameter(":id","0010"),
                        new OracleParameter(":id","0011"),
                        new OracleParameter(":name","圆珠笔"),
                        new OracleParameter(":mytype","办公用品"),
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


                #region 参数化查询
                using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
                {
                    // 创建表、索引、序列、触发器、过程

                }
                    #endregion
                }
            catch (Exception ex)
            {

            }
        }
    }
}