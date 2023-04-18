using WebAPI_CURD.Entities;

namespace WebAPI_CURD.Repositories
{
    /// <summary>
    /// defines the user repository methods
    /// </summary>
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByUserName(string userName);
        Task<User> GetByEmail(string email);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task Delete(int id);
    }
}
