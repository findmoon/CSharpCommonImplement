
using Oracle.ManagedDataAccess.Client;

using System.Collections.Generic;
using System.Data.Common;

using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Data
{
    /// <summary>
    /// 推荐使用Init创建SQLHelper对象，若使用new单独创建，必须调用Initializer
    /// </summary>
    public class OracleSQLHelper : ISQLHelper, IDisposable
    {
        // 基本都是直接查询获取或更新、或执行sql，没必要维护一个全局变量
        //private SqlCommand cmd = null;

        private string _ipInstance;
        private string _userName;
        private string _password;
        private string _dbName;

        private OracleConnection _conn;

        #region 属性
        /// <summary>
        /// 获取连接状态是否OK
        /// </summary>
        public bool ConnStatusOk
        {
            get
            {
                return Conn != null && Conn.State != ConnectionState.Closed && Conn.State != ConnectionState.Broken;
            }
        }

        /* 非接口属性 */

        /// <summary>
        /// 当前连接字符串
        /// </summary>
        private string ConnStr => $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={_ipInstance})(PORT=1521))" +
                    $"(CONNECT_DATA=(SERVICE_NAME={_dbName})));Persist Security Info=True;User ID={_userName};Password={_password};";

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public ConnectionState? ConnState => Conn?.State;

        /// <summary>
        /// 获取当前连接对象SqlConnection (若未连接，会在获取时打开连接)
        /// </summary>
        /// <returns></returns>
        public OracleConnection Conn
        {
            get
            {
                if (_conn == null) return _conn;
                // The ConnectionString property has not been initialized 时 State 为 Closed
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                return _conn;
            }
            private set
            {
                _conn = value;
            }
        }
        #endregion

        #region 初始化器
        static readonly Dictionary<string, OracleSQLHelper> _sqlHelperCache = new Dictionary<string, OracleSQLHelper>();

        /// <summary>
        /// 初始化新的连接
        /// </summary>
        /// <param name="ipServer"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public bool Initializer(string ipServer, string userName, string password, string dbName)
        {
            if ($"{ipServer}-{userName}-{password}-{dbName}" == $"{_ipInstance}-{_userName}-{_password}-{_dbName}")
            {
                return CheckInitial();
            }

            _ipInstance = ipServer;
            _userName = userName;
            _password = password;
            _dbName = dbName;

            return Initializer(ConnStr);
        }
        /// <summary>
        /// 初始化新的连接
        /// </summary>
        /// <param name="initModel"></param>
        /// <returns></returns>
        public bool Initializer(SQLInitModel initModel)
        {
            return Initializer(initModel.IpInstance, initModel.UserName, initModel.Password, initModel.DBName);
        }
        /// <summary>
        /// 初始化获取SQLHelper对象，如果已有对应的数据库服务器连接，则直接返回；没有则创建新的
        /// 
        /// </summary>
        /// <param name="ipInstance"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static OracleSQLHelper Init(string ipServer, string userName, string password, string dbName)
        {
            var key = $"{ipServer}-{userName}-{password}-{dbName}";
            if (!_sqlHelperCache.ContainsKey(key))
            {
                var sql = new OracleSQLHelper();
                // 验证连接是否正常
                if (sql.Initializer(ipServer, userName, password, dbName))
                {
                    _sqlHelperCache.Add(key, sql);
                    return sql;
                }
            }
            // “Conn.ServerVersion”引发了类型“System.InvalidOperationException”的异常
            // 强行打开将会报错
            // Conn.Open();

            return _sqlHelperCache[key];
        }
        /// <summary>
        /// 初始化SQLHelper对象，如果已有对应的数据库服务器连接，则直接返回；没有则创建新的
        /// </summary>
        /// <param name="initModel"></param>
        /// <returns></returns>
        public static OracleSQLHelper Init(SQLInitModel initModel)
        {
            return Init(initModel.IpInstance, initModel.UserName, initModel.Password, initModel.DBName);
        }

        /// <summary>
        /// 直接通过connString初始化新的连接，可以自定义传入自己的连接字符串
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Initializer(string connStr)
        {
            Conn = new OracleConnection(connStr);

            // 验证连接是否正常
            return CheckInitial();
        }

        //public async Task<bool> InitAsync(SQLInitModel initModel)
        //{

        //    // “Conn.ServerVersion”引发了类型“System.InvalidOperationException”的异常
        //    // 强行打开将会报错
        //    // await Conn.OpenAsync();

        //    return ConnStatusOk;
        //}
        #endregion

        public bool ChangeDB(string dbName)
        {
            if (_conn == null || _dbName == null) throw new Exception("当前无法更改数据库，请直接进行初始化连接");
            if (_dbName == dbName)
            {
                return true;
            }
            _conn.Close();
            _dbName = dbName;
            return Initializer(ConnStr);
        }
        //public async Task<bool> ChangeDBAsync(string dbName)
        //{
        //    await _conn.OpenAsync();
        //    return CheckInitial();
        //}
        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConn()
        {
            _conn?.Close();
        }
        /// <summary>
        /// 检查是否初始化，未初始化将返回false或引发异常，已初始化返回true【通常用于open()操作或初始化之后的检查】
        /// </summary>
        public bool CheckInitial()
        {
            if (Conn == null)
            {
                return false;
            }
            if (!ConnStatusOk)
            {
                Dispose();
                throw new Exception("当前SQLHelper未正确初始化！");
            }
            return true;
        }
        /// <summary>
        /// 是否存在某表或某列(异步)【tableName columnName不能同时为空】
        /// </summary>
        /// <param name="dbName">参数无效</param>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名，如果为空，将检查表是否存在</param>
        /// <param name="schema">参数无效</param>
        /// <returns></returns>
        public bool ExistsDBOrTableOrCol(string dbName, string tableName, string columnName = "", string schema = "")
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }
            string sql = GetQueryExistsDBOrTableSQLStr(dbName, tableName, columnName, schema);
            var dbExists = ExecuteScalar(sql);

            return dbExists != "0";
        }

        /// <summary>
        /// 是否存在某表或某列(异步)【tableName columnName不能同时为空】
        /// </summary>
        /// <param name="dbName">参数无效</param>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名，如果为空，将检查表是否存在</param>
        /// <param name="schema">参数无效</param>
        /// <returns></returns>
        public async Task<bool> ExistsDBOrTableOrColAsync(string dbName, string tableName, string columnName = "", string schema = "")
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }
            string sql = GetQueryExistsDBOrTableSQLStr(dbName, tableName, columnName, schema);
            var dbExists = await ExecuteScalarAsync(sql);

            return dbExists != "0";
        }
        /// <summary>
        /// 获取查询数据库或表是否存在的SQL语句
        /// </summary>
        /// <param name="dbName">参数无效</param>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名，如果为空，将检查表是否存在</param>
        /// <param name="schema">参数无效</param>
        /// <returns></returns>
        private static string GetQueryExistsDBOrTableSQLStr(string dbName, string tableName, string columnName="", string schema = "")
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"tableName");
            }
            // 语句结尾;要去掉，否则报错 ORA-00933
            // 查询表/列是否存在时 user_tables、user_tab_columns 表中的表名、列名均为大写，传入参数如果不大写将查询不到，因此添加upper()
            var sql = string.Empty;
            if (!string.IsNullOrWhiteSpace(tableName) && string.IsNullOrWhiteSpace(columnName))
            {
                sql = $"select count(*) from user_tables where table_name = upper('{tableName}')";
            }
            else
            {
                sql = $"select count(*) from user_tab_columns t where t.table_name= upper('{tableName}') and t.column_name = upper('{columnName}')";
            }

            return sql;
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new OracleCommand(cmdText, Conn))
            {
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                cmd.CommandType = cmdType;
                return cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 异步执行非查询语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new OracleCommand(cmdText, Conn))
            {
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                cmd.CommandType = cmdType;
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// 执行查询，返回结果的DataTable格式
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new OracleCommand(cmdText, Conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                using (var sqlrdr = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(sqlrdr);
                    return dt;
                }
            }
        }
        /// <summary>
        /// 执行异步查询，返回结果的DataTable格式
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new OracleCommand(cmdText, Conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                using (var sqlrdr = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(sqlrdr);
                    return dt;
                }
            }
        }

        /// <summary>
        /// 返回第一行第一列的值，即返回单个值
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public string ExecuteScalar(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new OracleCommand(cmdText, Conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar().ToString();
            }
        }
        /// <summary>
        /// 异步查询，返回第一行第一列的值，即返回单个值
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public async Task<string> ExecuteScalarAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new OracleCommand(cmdText, Conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
        }
        /// <summary>
        /// 释放 Connection
        /// </summary>
        public void Dispose()
        {
            var key = $"{_ipInstance}-{_userName}-{_password}-{_dbName}";
            if (_sqlHelperCache.ContainsKey(key))
            {
                _sqlHelperCache.Remove(key);
            }
            _conn?.Dispose();
            // 简要重置
            _conn = null;
            //ConnStr = null;
            _ipInstance = null;
            _userName = null;
            _password = null;
            _dbName = null;
        }
    }
}
