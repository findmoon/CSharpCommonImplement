#if NETSTANDARD2_1_OR_GREATER
using Microsoft.Data.SqlClient;
#endif
using System.Collections.Generic;
using System.Data.Common;
#if NET45_OR_GREATER
using System.Data.SqlClient;
#endif
using System.Threading.Tasks;

namespace System.Data
{
    /// <summary>
    /// 推荐使用Init创建SQLHelper对象，若使用new单独创建，必须调用Initializer
    /// </summary>
    public class SQLServerHelper : ISQLHelper
    {
        private SqlCommand cmd = null;

        private string _ipInstance;
        private string _userName;
        private string _password;
        private string _dbName;

        private SqlConnection _conn;

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
        /// <summary>
        /// 当前连接字符串
        /// </summary>
        public string ConnStr
        {
            get;
            private set;
        }

        /* 非接口属性 */

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public ConnectionState? ConnState
        {
            get
            {
                return Conn?.State;
            }
        }
        /// <summary>
        /// 获取当前连接对象SqlConnection (若未连接，会在获取时打开连接)
        /// </summary>
        /// <returns></returns>
        public SqlConnection Conn
        {
            get
            {
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
        static readonly Dictionary<string, SQLServerHelper> _sqlHelperCache = new Dictionary<string, SQLServerHelper>();

        /// <summary>
        /// 初始化一个新的SQLHelper对象
        /// </summary>
        /// <param name="ipInstance"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public bool Initializer(string ipInstance, string userName, string password, string dbName)
        {
            _ipInstance = ipInstance;
            _userName = userName;
            _password = password;
            _dbName = dbName;
            ConnStr = $"Server={_ipInstance};Database={dbName};User Id={_userName};Password={_password};";
            Conn = new SqlConnection(ConnStr);
            // 验证连接是否正常
            return CheckOpen();
        }
        /// <summary>
        /// 初始化一个新的SQLHelper对象
        /// </summary>
        /// <param name="initModel"></param>
        /// <returns></returns>
        public bool Initializer(SQLInitModel initModel)
        {
            return Initializer(initModel.IpInstance, initModel.UserName,initModel.Password,initModel.DBName);
        }
        /// <summary>
        /// 初始化SQLHelper对象，如果已有对应的数据库服务器连接，则直接返回；没有则创建新的
        /// </summary>
        /// <param name="ipInstance"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static SQLServerHelper Init(string ipInstance, string userName, string password, string dbName)
        {
            var key = $"{ipInstance}-{userName}-{password}-{dbName}";
            if (!_sqlHelperCache.ContainsKey(key))
            {
                var sql = new SQLServerHelper();
                // 验证连接是否正常
                if (sql.Initializer(ipInstance, userName, password, dbName))
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
        public static SQLServerHelper Init(SQLInitModel initModel)
        {
            return Init(initModel.IpInstance, initModel.UserName, initModel.Password, initModel.DBName);
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
            if (_dbName== dbName)
            {
                return true;
            }
            _conn?.Close();
            var connStr = $"Server={_ipInstance};Database={dbName};User Id={_userName};Password={_password};";
            _conn = new SqlConnection(connStr);
            _conn.Open();
            return CheckOpen();
        }
        public async Task<bool> ChangeDBAsync(string dbName)
        {
            if (_dbName == dbName)
            {
                return true;
            }
            _conn?.Close();
            var connStr = $"Server={_ipInstance};Database={dbName};User Id={_userName};Password={_password};";
            _conn = new SqlConnection(connStr);
            await _conn.OpenAsync();
            return CheckOpen();
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConn()
        {
            cmd?.Dispose();
            _conn?.Close();
        }
        /// <summary>
        /// 检查当前连接是否打开，未打开将引发异常，已打开返回true【通常用于open()操作或初始化之后的检查】】
        /// </summary>
        public bool CheckOpen()
        {
            if (Conn.State != ConnectionState.Open)
            {
                throw new Exception("当前SQL未连接，请确认设置是否正确，或稍后重试！");
            }
            return true;
        }
        /// <summary>
        /// 是否存在某数据库或数据库中存在某表【dbName tableName不能同时为空】
        /// </summary>
        /// <param name="dbName">数据库名，如果为空，将检查当前连接的数据库中是否存在某table</param>
        /// <param name="tableName">表名，如果为空，将仅检查是否存在某db</param>
        /// <param name="schema">不应该为空</param>
        /// <returns></returns>
        public bool ExistsDBOrTable(string dbName, string tableName, string schema = "dbo")
        {
            if (string.IsNullOrEmpty(dbName) && string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            string sql = GetQueryExistsDBOrTableSQLStr(dbName, tableName, schema);
            var dbExists = ExecuteScalar(sql);

            return dbExists != "0";
        }

        /// <summary>
        /// 是否存在某数据库或数据库中存在某表(异步)【dbName tableName不能同时为空】
        /// </summary>
        /// <param name="dbName">数据库名，如果为空，将检查当前连接的数据库中是否存在某table</param>
        /// <param name="tableName">表名，如果为空，将仅检查是否存在某db</param>
        /// <param name="schema">不应该为空</param>
        /// <returns></returns>
        public async Task<bool> ExistsDBOrTableAsync(string dbName, string tableName, string schema = "dbo")
        {
            if (string.IsNullOrEmpty(dbName) && string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            string sql = GetQueryExistsDBOrTableSQLStr(dbName, tableName, schema);
            var dbExists = await ExecuteScalarAsync(sql);

            return dbExists != "0";
        }
        /// <summary>
        /// 获取查询数据库或表是否存在的SQL语句
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private static string GetQueryExistsDBOrTableSQLStr(string dbName, string tableName, string schema = "dbo")
        {
            if (string.IsNullOrWhiteSpace(dbName) && string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"dbName and tableName");
            }
            var sql = string.Empty;
            if (!string.IsNullOrEmpty(dbName) && string.IsNullOrEmpty(tableName))
            {
                sql = $"SELECT case when DB_ID('{dbName}') IS NULL THEN 0 ELSE 1 END;";
            }
            else if (string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(tableName))
            {
                sql = $"SELECT case when OBJECT_ID('{schema}.{tableName}') IS NULL THEN 0 ELSE 1 END;";
            }
            else
            {
                sql = $"SELECT case when (DB_ID('{dbName}') IS NULL OR OBJECT_ID('{dbName}.{schema}.{tableName}') IS NULL) THEN 0 ELSE 1 END;";
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
            using (cmd = new SqlCommand(cmdText, Conn))
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
            using (cmd = new SqlCommand(cmdText, Conn))
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
            using (cmd = new SqlCommand(cmdText, Conn))
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
            using (cmd = new SqlCommand(cmdText, Conn))
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
            using (cmd = new SqlCommand(cmdText, Conn))
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
            using (cmd = new SqlCommand(cmdText, Conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
        }
    }
}
