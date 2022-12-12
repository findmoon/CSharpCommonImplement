using System.Data.Common;
using System.Threading.Tasks;

namespace System.Data
{
    /// <summary>
    /// SQLHelper的接口
    /// </summary>
    public interface ISQLHelper
    {
        //DbCommand Cmd { get; set; }
        //DbDataReader Sqlrdr { get; set; }
        ///// <summary>
        ///// 获取当前连接对象 DbConnection(SqlConnection\MySqlConnection\...)
        ///// </summary>
        ///// <returns></returns>
        //DbConnection Conn
        //{
        //    get;
        //}

        ///// <summary>
        ///// 获取当前的连接字符串 【似乎没太大必要，子类自己返回DbConnection更好】
        ///// </summary>
        //string ConnStr
        //{
        //    get;
        //}

        /// <summary>
        /// 获取连接状态是否OK
        /// </summary>
        bool ConnStatusOk
        {
            get;
        }


        /// <summary>
        /// SQLHelper初始化器，初始化新的连接【应注意避免重复初始化】
        /// </summary>
        /// <param name="initModel"></param>
        /// <returns></returns>
        bool Initializer(SQLInitModel initModel);
        /// <summary>
        /// 初始化新的连接
        /// </summary>
        /// <param name="ipInstance"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dbName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool Initializer(string ipInstance, string userName, string password, string dbName, ushort? port=null);
        /// <summary>
        /// 直接通过connString初始化新的连接，可以自定义传入自己的连接字符串
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        bool Initializer(string connStr);

        //Task<bool> InitAsync(SQLInitModel initModel);

        /// <summary>    
        /// 更换连接的数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        bool ChangeDB(string dbName);
        // 异步更改数据库似乎没太必要
        //Task<bool> ChangeDBAsync(string dbName);

        /// <summary>
        /// 断开连接
        /// </summary>
        void DisConn();

        /// <summary>
        /// 检查是否初始化，未初始化将返回false或引发异常（且应该进行简单的重置），已初始化返回true【通常用于open()操作或初始化之后的检查】
        /// </summary>
        bool CheckInitial();

        /// <summary>
        /// 是否存在某数据库及数据库中存在表
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName">列名，如果为空，将检查数据库或表是否存在</param>
        /// <param name="schema"></param>
        /// <returns></returns>
        bool ExistsDBOrTableOrCol(string dbName, string tableName, string columnName, string schema);
        /// <summary>
        /// 是否存在某数据库及数据库中存在表
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName">列名，如果为空，将检查数据库或表是否存在</param>
        /// <param name="schema"></param>
        /// <returns></returns>
       // Task<bool> ExistsDBOrTableOrColAsync(string dbName, string tableName, string columnName, string schema);

        /// <summary>
        /// 执行非查询SQL操作，返回受影响的行数 ddl语句似乎返回 -1
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        //Task<int> ExecuteNonQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行查询SQL，返回DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        DataTable ExecuteQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        //Task<DataTable> ExecuteQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 查询SQL返回第一个值
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        string ExecuteScalar(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        //Task<string> ExecuteScalarAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
    }
}
