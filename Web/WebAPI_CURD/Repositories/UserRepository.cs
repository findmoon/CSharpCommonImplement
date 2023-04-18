using Dapper;
using WebAPI_CURD.Entities;
using WebAPI_CURD.Helpers;

namespace WebAPI_CURD.Repositories
{
    /// <summary>
    /// 封装数据库操作 encapsulates all SQLite database interaction related to user CRUD operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = """
                SELECT * FROM Users
            """;
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                SELECT * FROM Users 
                WHERE Id = @id
            """;
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
        }
        
        public async Task<User> GetByUserName(string userName)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                SELECT * FROM Users 
                WHERE UserName = @userName
            """;
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { userName });
        }

        public async Task<User> GetByEmail(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                SELECT * FROM Users 
                WHERE Email = @email
            """;
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        }

        public async Task<User> Create(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                INSERT INTO Users (UserName, Title, FirstName, LastName, Email, Role, PasswordHash)
                VALUES (@UserName,@Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)
            """;
            await connection.ExecuteAsync(sql, user);
            return user;
        }

        public async Task<User> Update(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                UPDATE Users 
                SET UserName = @UserName,
                    Title = @Title,
                    FirstName = @FirstName,
                    LastName = @LastName, 
                    Email = @Email, 
                    Role = @Role, 
                    PasswordHash = @PasswordHash
                WHERE Id = @Id
            """;
            await connection.ExecuteAsync(sql, user);
            return user;
        }

        public async Task Delete(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = """
                DELETE FROM Users 
                WHERE Id = @id
            """;
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
