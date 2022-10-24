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

        /// <summary>
        /// 获取当前的连接字符串
        /// </summary>
        string ConnStr
        {
            get;
        }

        /// <summary>
        /// 获取连接状态是否OK
        /// </summary>
        bool ConnStatusOk
        {
            get;
        }


        /// <summary>
        /// SQLHelper初始化器，初始化一个新的SQLHelper
        /// </summary>
        /// <param name="initModel"></param>
        /// <returns></returns>
        bool Initializer(SQLInitModel initModel);
        //Task<bool> InitAsync(SQLInitModel initModel);

        /// <summary>
        /// 更换连接的数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        bool ChangeDB(string dbName);
        Task<bool> ChangeDBAsync(string dbName);

        /// <summary>
        /// 断开连接
        /// </summary>
        void DisConn();

        /// <summary>
        /// 检查当前连接是否打开，未打开将引发异常或返回false，已打开返回true【通常用于open()操作或初始化之后的检查】】
        /// </summary>
        bool CheckOpen();

        /// <summary>
        /// 是否存在某数据库及数据库中存在表
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool ExistsDBOrTable(string dbName, string tableName, string schema);
        /// <summary>
        /// 是否存在某数据库及数据库中存在表
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<bool> ExistsDBOrTableAsync(string dbName, string tableName, string schema);

        /// <summary>
        /// 执行非查询SQL操作
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        Task<int> ExecuteNonQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 执行查询SQL，返回DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        DataTable ExecuteQuery(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        Task<DataTable> ExecuteQueryAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        /// <summary>
        /// 查询SQL返回第一个值
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        string ExecuteScalar(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
        Task<string> ExecuteScalarAsync(string cmdText, DbParameter[] parameters = null, CommandType cmdType = CommandType.Text);
    }
}
