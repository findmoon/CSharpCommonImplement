using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace SQLServerHelperUse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var ip = serverTxt.Text.Trim();
            var user = userTxt.Text.Trim();
            var pwd = pwdTxt.Text.Trim();
            var dbname = dbTxt.Text.Trim();

            #region ��ʹ��
            //using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
            //{
            //    var resule = await sqlHelper.ExecuteQueryAsync("select * from product");
            //    // ...

            //    var tableExists1 = await sqlHelper.ExistsDBOrTableOrColAsync("t", "test");
            //    var dbExists2 = await sqlHelper.ExistsDBOrTableOrColAsync("shop", "");
            //    var colExists3 = await sqlHelper.ExistsDBOrTableOrColAsync("", "product", "sale_price");
            //    var colExists4 = await sqlHelper.ExistsDBOrTableOrColAsync("", "product", "sale_price_No");
            //    var colExists5 = await sqlHelper.ExistsDBOrTableOrColAsync("shop", "product", "sale_price");
            //    var tableExists6 = await sqlHelper.ExistsDBOrTableOrColAsync("t", "test1");
            //}

            //using (var sqlHelper = new SQLServerHelper())
            //{
            //    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
            //    {
            //        var resule = await sqlHelper.ExecuteQueryAsync("select * from test");
            //        sqlHelper.Conn.Dispose();
            //        // ...
            //    }
            //}

            ////using (var sqlHelper = new SQLServerHelper())
            ////{

            ////        if (sqlHelper.Initializer("ConnectionString"))
            ////        {

            ////        }

            ////} 
            #endregion

            #region ��������ѯ
            using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname, 1611))
            {
                // select * from Product where product_id='0006' or sale_price=8800; 
                var selectSql = "select * from Product where product_id=@id or sale_price=@price;";
                var resuleDt = await sqlHelper.ExecuteQueryAsync(selectSql, new SqlParameter[]
                {
                    new SqlParameter("@id","0006"),
                    new SqlParameter("@price",8800)
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

                //// insert Product values('0009','Բ���','�칫��Ʒ',15,10,GETDATE());
                //var insertSql = "insert into Product values(@id,@name,@type,@price,@purchase,@date);";
                //var rowNum = await sqlHelper.ExecuteNonQueryAsync(insertSql, new SqlParameter[]
                //{
                //    new SqlParameter("@id","0010"),
                //    new SqlParameter("@name","Բ���"),
                //    new SqlParameter("@type","�칫��Ʒ"),
                //    new SqlParameter("@price",15),
                //    new SqlParameter("@purchase",10),
                //    new SqlParameter("@date",DateTime.Now)
                //});

                //Debug.WriteLine($"affect row num: {rowNum}");

                // select product_id from Product where product_id='0010';
                var selectOneSql = "select product_id from Product where product_id=@id;";
                var resuleValue = await sqlHelper.ExecuteScalarAsync(selectOneSql, new SqlParameter[]
                {
                    new SqlParameter("@id","0010")
                });
                Debug.WriteLine($"product_id: {resuleValue}");
                //Trace.WriteLine($"product_id: {resuleValue}");
            }
            #endregion
        }
    }
}