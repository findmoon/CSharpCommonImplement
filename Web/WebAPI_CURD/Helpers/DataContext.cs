using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace WebAPI_CURD.Helpers
{
    /// <summary>
    /// connect the SQLite database
    /// </summary>
    public class DataContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(Configuration.GetConnectionString("WebApiDatabase"));
        }

        /// <summary>
        /// 创建DB、tables【Program.cs 的 startup 执行一次】
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            // create database tables if they don't exist
            using var connection = CreateConnection();
            await _initUsers();

            async Task _initUsers()
            {
                var sql = """
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
                );
            """;
                await connection.ExecuteAsync(sql);
            }
        }
    }
}
