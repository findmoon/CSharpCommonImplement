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
    public class SQLServerHelper : ISQLHelper, IDisposable
    {
        // 基本都是直接查询获取或更新、或执行sql，没必要维护一个全局变量
        //private SqlCommand cmd = null;

        private string _ipInstance;
        private string _userName;
        private string _password;
        private string _dbName;
        private ushort? _port;

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

        /* 非接口属性 */

        /// <summary>
        /// 当前连接字符串
        /// </summary>
        private string ConnStr
        {
            get
            {
                var port = "";
                if (_port != null)
                {
                    port = "," + _port;
                }
                return $"Server={_ipInstance}{port};Database={_dbName};User Id={_userName};Password={_password};Encrypt=False;";
            }
        }

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public ConnectionState? ConnState => Conn?.State;

        /// <summary>
        /// 获取当前连接对象SqlConnection (若未连接，会在获取时打开连接)
        /// </summary>
        /// <returns></returns>
        public SqlConnection Conn
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
                // 释放之前的资源
                _conn?.Dispose();
                _conn = value;
            }
        }
        #endregion

        #region 初始化器
        static readonly Dictionary<string, SQLServerHelper> _sqlHelperCache = new Dictionary<string, SQLServerHelper>();

        /// <summary>
        /// 初始化新的连接
        /// </summary>
        /// <param name="ipInstance"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <param name="port">端口，不指定将使用默认端口</param>
        /// <returns></returns>
        public bool Initializer(string ipInstance, string userName, string password, string dbName, ushort? port = null)
        {
            if ($"{ipInstance}-{userName}-{password}-{dbName}-{port}" == $"{_ipInstance}-{_userName}-{_password}-{_dbName}-{_port}")
            {
                return CheckInitial();
            }

            _ipInstance = ipInstance;
            _userName = userName;
            _password = password;
            _dbName = dbName;
            _port = port;

            return Initializer(ConnStr);
        }
        /// <summary>
        /// 初始化新的连接
        /// </summary>
        /// <param name="initModel"></param>
        /// <returns></returns>
        public bool Initializer(SQLInitModel initModel)
        {
            return Initializer(initModel.IpInstance, initModel.UserName, initModel.Password, initModel.DBName, initModel.Port);
        }
        /// <summary>
        /// 初始化获取SQLHelper对象，如果已有对应的数据库服务器连接，则直接返回；没有则创建新的
        /// 
        /// </summary>
        /// <param name="ipInstance"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <param name="port">端口，不指定将使用默认端口</param>
        /// <returns></returns>
        public static SQLServerHelper Init(string ipInstance, string userName, string password, string dbName, ushort? port = null)
        {
            var key = $"{ipInstance}-{userName}-{password}-{dbName}-{port}";
            if (!_sqlHelperCache.ContainsKey(key))
            {
                var sql = new SQLServerHelper();
                // 验证连接是否正常
                if (sql.Initializer(ipInstance, userName, password, dbName, port))
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
            return Init(initModel.IpInstance, initModel.UserName, initModel.Password, initModel.DBName, initModel.Port);
        }

        /// <summary>
        /// 直接通过connString初始化新的连接，可以自定义传入自己的连接字符串
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public bool Initializer(string connStr)
        {
            Conn = new SqlConnection(connStr);

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
        /// <summary>
        /// 变更连接的数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
        /// 检查是否初始化，未初始化将引发异常，已初始化返回true【通常用于open()操作或初始化之后的检查】
        /// </summary>
        public bool CheckInitial()
        {
            if (!ConnStatusOk)
            {
                Dispose();
                throw new Exception("当前SQLHelper未正确初始化！");
            }
            return true;
        }
        /// <summary>
        /// 是否存在某数据库或数据库中存在某表【dbName tableName不能同时为空】
        /// </summary>
        /// <param name="dbName">数据库名，如果为空，将检查当前连接的数据库中是否存在某table或column</param>
        /// <param name="tableName">表名，如果为空，将仅检查是否存在某db</param>
        /// <param name="columnName">列名，如果为空，将检查数据库或表是否存在;若不为空，则tableName也必须指定</param>
        /// <param name="schema">不应该为空</param>
        /// <returns></returns>
        public bool ExistsDBOrTableOrCol(string dbName, string tableName, string columnName = "", string schema = "dbo")
        {
            if (string.IsNullOrWhiteSpace(dbName) && string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }
            string sql = GetQueryExistsDBOrTableOrColSQLStr(dbName, tableName, columnName, schema);
            var dbExists = ExecuteScalar(sql);

            return dbExists != "0";
        }

        /// <summary>
        /// 是否存在某数据库或数据库中存在某表(异步)【dbName tableName不能同时为空】
        /// </summary>
        /// <param name="dbName">数据库名，如果为空，将检查当前连接的数据库中是否存在某table或column</param>
        /// <param name="tableName">表名，如果为空，将仅检查是否存在某db</param>
        /// <param name="columnName">列名，如果为空，将检查数据库或表是否存在;若不为空，则tableName也必须指定</param>
        /// <param name="schema">不应该为空</param>
        /// <returns></returns>
        public async Task<bool> ExistsDBOrTableOrColAsync(string dbName, string tableName, string columnName = "", string schema = "dbo")
        {
            if (string.IsNullOrWhiteSpace(dbName) && string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }
            string sql = GetQueryExistsDBOrTableOrColSQLStr(dbName, tableName, columnName, schema);
            var dbExists = await ExecuteScalarAsync(sql);

            return dbExists != "0";
        }
        /// <summary>
        /// 获取查询数据库或表或列是否存在的SQL语句
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private static string GetQueryExistsDBOrTableOrColSQLStr(string dbName, string tableName, string columnName = "", string schema = "dbo")
        {
            if (string.IsNullOrWhiteSpace(dbName) && string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException($"dbName and tableName");
            }
            var sql = string.Empty;
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new ArgumentNullException($"columnName存在时，必须指定tableName");
                }
                sql = $"SELECT case when COL_LENGTH('{dbName}.{schema}.{tableName}','{columnName}') IS NULL THEN 0 ELSE 1 END;";
            }
            else if (!string.IsNullOrWhiteSpace(dbName) && string.IsNullOrWhiteSpace(tableName))
            {
                sql = $"SELECT case when DB_ID('{dbName}') IS NULL THEN 0 ELSE 1 END;";
            }
            else if (string.IsNullOrWhiteSpace(dbName) && !string.IsNullOrWhiteSpace(tableName))
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
        /// 执行非查询语句，返回受影响的行数  ddl语句似乎返回 -1
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new SqlCommand(cmdText, Conn))
            {
                if (parameters != null && parameters.Length > 0) cmd.Parameters.AddRange(parameters);
                cmd.CommandType = cmdType;
                return cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 异步执行非查询语句，返回受影响的行数  ddl语句似乎返回 -1
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text)
        {
            using (var cmd = new SqlCommand(cmdText, Conn))
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
            using (var cmd = new SqlCommand(cmdText, Conn))
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
            using (var cmd = new SqlCommand(cmdText, Conn))
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
            using (var cmd = new SqlCommand(cmdText, Conn))
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
            using (var cmd = new SqlCommand(cmdText, Conn))
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
            var key = $"{_ipInstance}-{_userName}-{_password}-{_dbName}-{_port}";
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

        #region SQL Server的单独方法
        /// <summary>
        /// 使用 sp_attach_db 存储过程执行SQL，附加数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="mdfFileName"></param>
        /// <param name="otherLdfMdfFileNames"></param>
        /// <returns></returns>
        public async Task<bool> AttachDBAsync(string dbName,string mdfFileName,params string[] otherLdfMdfFileNames) {

            var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@dbname",dbName),
                    new SqlParameter("@filename1",mdfFileName)
                };
            for (int i = 0; i < otherLdfMdfFileNames.Length; i++)
            {
                sqlParams.Add(new SqlParameter("@filename"+(2+i), otherLdfMdfFileNames[i]));
            }
            var result = await ExecuteScalarAsync("sp_attach_db", sqlParams.ToArray(), CommandType.StoredProcedure);
            return result == "0"; // 1 失败
        }
        /// <summary>
        /// 使用 sp_detach_db 分离数据库 执行sql语句
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public async Task<bool> DetachDBAsync(string dbName) {

            var result = await ExecuteScalarAsync(@"USE master; ALTER DATABASE @dbname SET SINGLE_USER; EXEC sp_detach_db @dbname1;", new SqlParameter[]
                {
                    new SqlParameter("@dbname",dbName),
                    new SqlParameter("@dbname1",dbName)
                });
            return result == "0"; // 1 失败
        }
        /// <summary>
        /// 获取默认的数据库文件路径
        /// </summary>
        /// <returns></returns>
        public async Task<string> DefaultDataPathAsync() {
            return await ExecuteScalarAsync("SELECT SERVERPROPERTY('InstanceDefaultDataPath');");
            // SERVERPROPERTY('InstanceDefaultLogPath')
        }


        #endregion
    }
}
