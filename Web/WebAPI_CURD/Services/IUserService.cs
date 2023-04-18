using WebAPI_CURD.Entities;
using WebAPI_CURD.Models.Users;

namespace WebAPI_CURD.Services
{
    /// <summary>
    /// core business logic and validation related to user CRUD operations.连接 controllers 和 repositories
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string email);
        Task<User> Create(CreateRequest model);
        Task<User> Update(int id, UpdateRequest model);
        Task Delete(int id);
    }
}
