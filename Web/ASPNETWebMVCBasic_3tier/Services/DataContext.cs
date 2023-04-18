using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace WebAPI_CURD.Services
{
    /// <summary>
    /// connect the SQLite database
    /// </summary>
    public class DataContext : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Dapper 的 数据库连接
        /// </summary>
        public IDbConnection DbConnection { get; }
        public DataContext()
        {
            DbConnection = new OracleConnection(WebConfigurationManager.ConnectionStrings["OracleConStr"].ConnectionString);
        }


        /// <summary>
        /// 创建DB、tables【Program.cs 的 startup 执行一次】
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            await _initUsers();
           
            async Task _initUsers()
            {
                var sql = @"
                CREATE TABLE IF NOT EXISTS 
                Users (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    UserName VARCHAR(255) NOT NULL UNIQUE,
                    Title VARCHAR(255),
                    FirstName VARCHAR(30),
                    LastName VARCHAR(30),
                    Email VARCHAR(255) NOT NULL UNIQUE,
                    Role TINYINT DEFAULT 0,
                    PasswordHash TEXT
                )";
                await DbConnection.ExecuteAsync(sql);
            }
        }

        /// <summary>
        /// 关闭DbConnection
        /// </summary>
        public void Dispose()
        {
            if (this.DbConnection == null)
                return;

            if (this._disposed)
                return;

            this._disposed = true;

            if (this.DbConnection.State != ConnectionState.Closed)
                this.DbConnection.Close(); // DbConnection.Dispose()
        }
    }
}
