﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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

            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serverTxt.Text = Properties.Settings.Default.serverNameIp;
            userTxt.Text = Properties.Settings.Default.user;
            pwdTxt.Text=Properties.Settings.Default.pwd;
            dbTxt.Text=Properties.Settings.Default.dbName;
        }
        void SaveSetting()
        {
            Properties.Settings.Default.serverNameIp= serverTxt.Text.Trim();
            Properties.Settings.Default.user= userTxt.Text.Trim();
            Properties.Settings.Default.pwd= pwdTxt.Text.Trim();
            Properties.Settings.Default.dbName= dbTxt.Text.Trim();

            Properties.Settings.Default.Save();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SaveSetting();
            var ip = serverTxt.Text.Trim();
            var user = userTxt.Text.Trim();
            var pwd = pwdTxt.Text.Trim();
            var dbname = dbTxt.Text.Trim();

            //var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname);

            //var resule = sqlHelper.ExecuteQuery("select * from t");

            #region 参数化查询
            using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
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
                    Console.Write(col.ColumnName + "\t");
                }
                Console.WriteLine();
                foreach (DataRow row in resuleDt.Rows)
                {
                    foreach (DataColumn col in resuleDt.Columns)
                    {
                        // 使用 row.Field<T>(name/idx/..) 获取字段值，需要判断是否DBNull -- row[col.ColumnName] is DBNull。 不为DBNull才能转换 且 必须指定正确的T数据类型

                        Console.Write(row[col.ColumnName].ToString() + "\t");
                    }
                    Console.WriteLine();
                }

                // ...

                //// insert Product values('0009','圆珠笔','办公用品',15,10,GETDATE());
                //var insertSql = "insert Product values(@id,@name,@type,@price,@purchase,@date);";
                //var rowNum = await sqlHelper.ExecuteNonQueryAsync(insertSql, new SqlParameter[]
                //{
                //    new SqlParameter("@id","0010"),
                //    new SqlParameter("@name","圆珠笔"),
                //    new SqlParameter("@type","办公用品"),
                //    new SqlParameter("@price",15),
                //    new SqlParameter("@purchase",10),
                //    new SqlParameter("@date",DateTime.Now)
                //});

                //Console.WriteLine($"affect row num: {rowNum}");

                // select product_id from Product where product_id='0010';
                var selectOneSql = "select product_id from Product where product_id=@id;";
                var resuleValue = await sqlHelper.ExecuteScalarAsync(selectOneSql, new SqlParameter[]
                {
                    new SqlParameter("@id","0010")
                });
                Console.WriteLine($"product_id: {resuleValue}");
            }
            #endregion
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            SaveSetting();

            var ip = serverTxt.Text.Trim();
            var user = userTxt.Text.Trim();
            var pwd = pwdTxt.Text.Trim();
            var dbname = dbTxt.Text.Trim();
            try
            {

                var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname);

                //var resule = sqlHelper.ExecuteQuery("select * from t");


                // 获取数据路径
                var dbDir = await sqlHelper.DefaultDataPathAsync();
                // 分离
                var detach_reulst = await sqlHelper.DetachDBAsync("t");
                // 附加
                var attach_reulst = await sqlHelper.AttachDBAsync("t", Path.Combine(dbDir, "t.mdf"), Path.Combine(dbDir, "t_log.ldf"));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("结束");
        }
    }
}
