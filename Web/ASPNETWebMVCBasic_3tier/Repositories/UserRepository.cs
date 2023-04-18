using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using WebAPI_CURD.Entities;
using WebAPI_CURD.Helpers;
using WebAPI_CURD.Services;

namespace WebAPI_CURD.Repositories
{
    /// <summary>
    /// 封装数据库操作 encapsulates all SQLite database interaction related to user CRUD operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private IDbConnection _DbConnection;

        public UserRepository(DataContext context)
        {
            _DbConnection = context.DbConnection;
        }

        public async Task<IEnumerable<User>> GetAll()
        {

            var sql = "SELECT * FROM Users";
            return await _DbConnection.QueryAsync<User>(sql);

        }

        public async Task<User> GetById(int id)
        {

                var sql = "SELECT * FROM Users WHERE Id = @id";
                return await _DbConnection.QuerySingleOrDefaultAsync<User>(sql, new { id });
 
        }
        
        public async Task<User> GetByUserName(string userName)
        {
                var sql = "SELECT * FROM Users WHERE UserName = @userName";
                return await _DbConnection.QuerySingleOrDefaultAsync<User>(sql, new { userName });
        }

        public async Task<User> GetByEmail(string email)
        {
                var sql = "SELECT * FROM Users WHERE Email = @email";
                return await _DbConnection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        }

        public async Task Create(User user)
        {
                var sql = "INSERT INTO Users (UserName, Title, FirstName, LastName, Email, Role, PasswordHash) VALUES (@UserName,@Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)";
                await _DbConnection.ExecuteAsync(sql, user);
        }

        public async Task Update(User user)
        {

                var sql = "UPDATE Users SET UserName = @UserName,Title = @Title, FirstName = @FirstName,LastName = @LastName,Email = @Email,Role = @Role, PasswordHash = @PasswordHash WHERE Id = @Id";
                await _DbConnection.ExecuteAsync(sql, user);
        }

        public async Task Delete(int id)
        {
                var sql = "DELETE FROM Users WHERE Id = @id";
                await _DbConnection.ExecuteAsync(sql, new { id });
        }
    }
}
