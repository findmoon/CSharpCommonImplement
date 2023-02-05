**C#中的MySqlHelper工具类及使用[不推荐，查看了解下即可]**

[toc]

> 原本是从网上搜一下 MySqlHelper，实际看下来，感觉代码并不怎么好【也可能自己水平不够】。仅做一个了解即可。后面将其改名为`MySqlHelper_No`

> 主要参考自 [C#中的MySqlHelper工具类及使用方法](https://www.cnblogs.com/timefiles/p/CsharpMySqlHelper.html)
>
> 对原文的`MysqlHelper`代码进行了不少修改，并，增加了`async/await`异步方法【原则上只需要异步方法即可】。

一下为基本的原文：

# 工具类

工具类转自[C# MysqlHelper C#连接mysql数据库类库全](https://my.oschina.net/xiaoxiezi/blog/3220690)，代码如下：

> **记得下载下面的MySql.Data.dll文件，并添加引用到项目中，也可以直接通过 NuGet 安装`MySql.Data`。**

```C#
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace System.Data
{
    /// <summary>
    /// MySql帮助类，不推荐使用，没多大帮助的帮助类。原本想直接参考一下网上的帮助类例子，看代码才发现写的真不太好。仅做保留
    /// </summary>
    [Obsolete("不推荐使用，没多大帮助的帮助类")]
    public class MySqlHelper_No
    {
        #region 额外增加的 async/await异步方法
        /// <summary> 
        /// 给定连接的数据库用假设参数执行一个sql命令（不返回数据集） 
        /// </summary> 
        /// <param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>执行命令所影响的行数</returns> 
        public static async Task<int> ExecuteNonQueryAsync(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await PrepareCommandAsync(cmd, conn, null, cmdType, cmdText, commandParameters);
                    int val = await cmd.ExecuteNonQueryAsync();
                    // cmd.Parameters.Clear();
                    return val;
                }
            }
        }
        /// <summary> 
        /// 用现有的数据库连接执行一个sql命令（不返回数据集） 
        /// </summary> 
        /// <param name="connection">一个现有的数据库连接</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>执行命令所影响的行数</returns> 
        public static async Task<int> ExecuteNonQueryAsync(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                await PrepareCommandAsync(cmd, connection, null, cmdType, cmdText, commandParameters);
                int val =await cmd.ExecuteNonQueryAsync();
                // cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary> 
        ///使用现有的SQL事务执行一个sql命令（不返回数据集） 
        /// </summary> 
        /// <remarks> 
        ///举例: 
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="trans">一个现有的事务</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>执行命令所影响的行数</returns> 
        public static async Task<int> ExecuteNonQueryAsync(MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                await PrepareCommandAsync(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                int val =await cmd.ExecuteNonQueryAsync();
                // cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary> 
        /// 返回DataSet 
        /// </summary> 
        /// <param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns></returns> 
        public static async Task<DataSet> GetDataSetAsync(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await PrepareCommandAsync(cmd, conn, null, cmdType, cmdText, commandParameters);
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();

                    await adapter.FillAsync(ds);
                    //cmd.Parameters.Clear();
                    return ds;
                }
            }
        }
        /// <summary>
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据表 
        /// </summary>
        ///<param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        public static async Task<DataTable> GetDataTableAsync(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await PrepareCommandAsync(cmd, conn, null, cmdType, cmdText, commandParameters);
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable ds = new DataTable();

                    await adapter.FillAsync(ds);
                    //cmd.Parameters.Clear();

                    return ds;
                }
            }
        }
        /// <summary> 
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列 
        /// </summary> 
        /// <remarks> 
        ///例如: 
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        ///<param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns> 
        public static async Task<object> ExecuteScalarAsync(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await PrepareCommandAsync(cmd, connection, null, cmdType, cmdText, commandParameters);
                    object val = await cmd.ExecuteScalarAsync();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }
        /// <summary>
        /// 返回插入值ID
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteNonExistAsync(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await PrepareCommandAsync(cmd, connection, null, cmdType, cmdText, commandParameters);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        /// <summary> 
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列 
        /// </summary> 
        /// <remarks> 
        /// 例如: 
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个存在的数据库连接</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns> 
        public static async Task<object> ExecuteScalarAsync(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {

            MySqlCommand cmd = new MySqlCommand();

            await PrepareCommandAsync(cmd, connection, null, cmdType, cmdText, commandParameters);
            return await cmd.ExecuteScalarAsync();

        }
        /// <summary> 
        /// 准备执行一个命令 
        /// </summary> 
        /// <param name="cmd">sql命令</param> 
        /// <param name="conn">OleDb连接</param> 
        /// <param name="trans">OleDb事务</param> 
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param> 
        /// <param name="cmdText">命令文本,例如:Select * from Products</param> 
        /// <param name="cmdParms">执行命令的参数</param> 
        private static async Task PrepareCommandAsync(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null && cmdParms.Length>0)
            {
                    cmd.Parameters.AddRange(cmdParms);
            }
        }
        #endregion

        /// <summary> 
        /// 获取一个有效的数据库连接对象 
        /// </summary> 
        /// <returns></returns> 
        public static MySqlConnection GetConnection(string connSting)
        {
            MySqlConnection Connection = new MySqlConnection(connSting);
            return Connection;
        }
        /// <summary> 
        /// 用执行的数据库连接执行一个返回数据集的sql命令 
        /// </summary> 
        /// <remarks> 
        /// 举例: 
        /// MySqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>包含结果的读取器</returns> 
        public static MySqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return reader;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        #region 旧的同步方法
        /// <summary> 
        /// 给定连接的数据库用假设参数执行一个sql命令（不返回数据集） 
        /// </summary> 
        /// <param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>执行命令所影响的行数</returns> 
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {

            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary> 
        /// 用现有的数据库连接执行一个sql命令（不返回数据集） 
        /// </summary> 
        /// <param name="connection">一个现有的数据库连接</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>执行命令所影响的行数</returns> 
        public static int ExecuteNonQuery(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary> 
        ///使用现有的SQL事务执行一个sql命令（不返回数据集） 
        /// </summary> 
        /// <remarks> 
        ///举例: 
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="trans">一个现有的事务</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>执行命令所影响的行数</returns> 
        public static int ExecuteNonQuery(MySqlTransaction trans, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary> 
        /// 返回DataSet 
        /// </summary> 
        /// <param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns></returns> 
        public static DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据表 
        /// </summary>
        ///<param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        public static DataTable GetDataTable(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataTable ds = new DataTable();

                adapter.Fill(ds);
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary> 
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列 
        /// </summary> 
        /// <remarks> 
        ///例如: 
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        ///<param name="connectionString">一个有效的连接字符串</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns> 
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        /// 返回插入值ID
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteNonExist(string connectionString, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteNonQuery();

                return cmd.LastInsertedId;
            }
        }
        /// <summary> 
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列 
        /// </summary> 
        /// <remarks> 
        /// 例如: 
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个存在的数据库连接</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns> 
        public static object ExecuteScalar(MySqlConnection connection, CommandType cmdType, string cmdText, params MySqlParameter[] commandParameters)
        {

            MySqlCommand cmd = new MySqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary> 
        /// 准备执行一个命令 
        /// </summary> 
        /// <param name="cmd">sql命令</param> 
        /// <param name="conn">OleDb连接</param> 
        /// <param name="trans">OleDb事务</param> 
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param> 
        /// <param name="cmdText">命令文本,例如:Select * from Products</param> 
        /// <param name="cmdParms">执行命令的参数</param> 
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null && cmdParms.Length > 0)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }
        #endregion

    }
}
```

# 使用方法 - 增删改查

这里只列举了常规的建库、建表、增删改查操作，代码如下：

```C#
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //建库            
            string connSting = "Data Source=localhost;Persist Security Info=yes; UserId=root; PWD=root;";
            string cmdText = "CREATE DATABASE IF NOT EXISTS test DEFAULT CHARSET utf8 COLLATE utf8_general_ci;";
            MySqlConnection conn = MySqlHelper.GetConnection(connSting);
            int val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText);
            Console.WriteLine("影响行数："+ val);

            //建表
            connSting = "server=localhost;Database='test';User='root';Password='root';charset='utf8';pooling=false;SslMode=none";
            StringBuilder sbr = new StringBuilder();
            sbr.Append("CREATE TABLE IF NOT EXISTS `test_table`(");
            sbr.Append("`id` INT UNSIGNED AUTO_INCREMENT,");
            sbr.Append("`name` VARCHAR(100) NOT NULL,");
            sbr.Append("`password` VARCHAR(40) NOT NULL,");
            sbr.Append("`create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',");
            sbr.Append("`update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',");
            sbr.Append("PRIMARY KEY( `id` ));");
            cmdText = sbr.ToString();
            conn = MySqlHelper.GetConnection(connSting);
            val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText);
            Console.WriteLine("影响行数：" + val);

            //增
            sbr.Clear();
            sbr.Append("INSERT INTO test_table (name,password) VALUES ");
            sbr.Append("(11,111), ");
            sbr.Append("(12,222); ");
            cmdText = sbr.ToString();
            val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText);
            Console.WriteLine("影响行数：" + val);

            //删
            sbr.Clear();
            sbr.Append("DELETE FROM test_table ");
            sbr.Append("WHERE id=1;");           
            cmdText = sbr.ToString();
            val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText);
            Console.WriteLine("影响行数：" + val);

            //改
            sbr.Clear();
            sbr.Append("UPDATE test_table SET ");
            sbr.Append("name='13', ");
            sbr.Append("password='333' ");
            sbr.Append("WHERE id=@id;");
            cmdText = sbr.ToString();
            MySqlParameter idParm = new MySqlParameter("@id", 2);
            val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText, idParm);
            Console.WriteLine("影响行数：" + val);

            //查
            sbr.Clear();
            sbr.Append("SELECT name,password FROM test_table ");
            sbr.Append("WHERE id=@id;");
            cmdText = sbr.ToString();
            DataTable dt= MySqlHelper.GetDataTable(connSting,CommandType.Text,cmdText, idParm);
            Console.WriteLine("结果行数：" + dt.Rows.Count);


            //测试Parameters.Clear()的作用
            string sqlInsert = "INSERT INTO test_table (name,password) VALUES ( @name ,1233);";
            string sqlSelect = "SELECT * FROM test_table WHERE name=@name;";
            MySqlParameter parms = new MySqlParameter("@name", "testName");
            MySqlHelper.ExecuteNonQuery(connSting, CommandType.Text, sqlInsert, parms);
            MySqlHelper.ExecuteNonQuery(connSting, CommandType.Text, sqlSelect, parms);

            //删除表
            sbr.Clear();
            sbr.Append("DROP TABLE test_table;");
            cmdText = sbr.ToString();
            val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText);
            Console.WriteLine("影响行数：" + val);

            //删除数据库
            connSting = "Data Source=localhost;Persist Security Info=yes; UserId=root; PWD=root;";
            cmdText = "DROP DATABASE test;";
            val = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, cmdText);
            Console.WriteLine("影响行数：" + val);

            Console.ReadKey();
        }
        
    }
}
```

关于**cmd.Parameters.Clear()**的作用，我没测出什么结果，有没有都一样。参考[cmd.Parameters.Clear() 语句的作用](https://blog.csdn.net/lachy/article/details/6546982)这篇文章，我怀疑问题只存在微软的Sql数据库中。

# 参考资料

- [MySql-8.0.23-winx64 dll 提取码：zloa](https://pan.baidu.com/s/1pu7DQbS7eGEGT2gWprtl9g)  
- [C#连接MySQL数据库，并建库、建表](https://blog.csdn.net/u014453443/article/details/85263954)  
- [C# MysqlHelper C#连接mysql数据库类库全](https://my.oschina.net/xiaoxiezi/blog/3220690)  
- [MySQL 教程](https://www.runoob.com/mysql/mysql-tutorial.html)
