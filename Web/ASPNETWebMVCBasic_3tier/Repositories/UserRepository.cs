using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Web.Helpers;
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
        private OracleDynamicParameters _dynParams;

        public UserRepository(DataContext context, OracleDynamicParameters dynParams)
        {
            _DbConnection = context.DbConnection;
            _dynParams = dynParams;
        }

        public async Task<IEnumerable<User>> GetAll()
        {

            var sql = "SELECT * FROM Users";
            return await _DbConnection.QueryAsync<User>(sql);

        }

        public async Task<User> GetById(int id)
        {
            _dynParams.Add(":id", OracleDbType.Int32, ParameterDirection.Input, id);
             var sql = "SELECT * FROM Users WHERE Id = :id";
                return await _DbConnection.QuerySingleOrDefaultAsync<User>(sql, _dynParams);
 
        }
        
        public async Task<User> GetByUserName(string userName)
        {
            _dynParams.Add(":userName", OracleDbType.Varchar2, ParameterDirection.Input, userName);
            var sql = "SELECT * FROM Users WHERE UserName = :userName";
                return await _DbConnection.QuerySingleOrDefaultAsync<User>(sql, _dynParams);
        }

        public async Task<User> GetByEmail(string email)
        {
            _dynParams.Add(":email", OracleDbType.Varchar2, ParameterDirection.Input, email);

            var sql = "SELECT * FROM Users WHERE Email = :email";
                return await _DbConnection.QuerySingleOrDefaultAsync<User>(sql, _dynParams);
        }

        public async Task Create(User user)
        {
            SetDynParams(user);
            var sql = "INSERT INTO Users (UserName, Title, FirstName, LastName, Email, Role, PasswordHash) VALUES (:UserName,:Title, :FirstName, :LastName, :Email, :Role, :PasswordHash)";
            await _DbConnection.ExecuteAsync(sql, _dynParams);
        }

        private void SetDynParams(User user)
        {
            _dynParams.Add(":Role", OracleDbType.Int16, ParameterDirection.Input, user.Role);
            _dynParams.Add(":Title", OracleDbType.Varchar2, ParameterDirection.Input, user.Title);
            _dynParams.Add(":UserName", OracleDbType.Varchar2, ParameterDirection.Input, user.UserName);
            _dynParams.Add(":Email", OracleDbType.Varchar2, ParameterDirection.Input, user.Email);
            _dynParams.Add(":FirstName", OracleDbType.Varchar2, ParameterDirection.Input, user.FirstName);
            _dynParams.Add(":LastName", OracleDbType.Varchar2, ParameterDirection.Input, user.LastName);
            _dynParams.Add(":PasswordHash", OracleDbType.Varchar2, ParameterDirection.Input, user.PasswordHash);
        }

        public async Task Update(User user)
        {
            SetDynParams(user);
            var sql = "UPDATE Users SET UserName = :UserName,Title = :Title, FirstName = :FirstName,LastName = :LastName,Email = :Email,Role = :Role, PasswordHash = :PasswordHash WHERE Id = :Id";
                await _DbConnection.ExecuteAsync(sql, _dynParams);
        }

        public async Task Delete(int id)
        {
            _dynParams.Add(":id", OracleDbType.Int32, ParameterDirection.Input, id);
            var sql = "DELETE FROM Users WHERE Id = :id";
                await _DbConnection.ExecuteAsync(sql, _dynParams);
        }
    }
}
