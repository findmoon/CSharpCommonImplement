namespace WebAPI_CURD.Services;
// 这种写法很有好处 比如 BCrypt 命名空间 和 BCrypt.Net.BCrypt 类名称相同问题

using AutoMapper;
using BCrypt.Net;
using WebAPI_CURD.Entities;
using WebAPI_CURD.Helpers;
using WebAPI_CURD.Models.Users;
using WebAPI_CURD.Repositories;



    /// <summary>
    /// core business logic and validation related to user CRUD operations.连接 controllers 和 repositories
    /// </summary>
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }
        public async Task<User> GetByUserName(string userName)
        {
            var user = await _userRepository.GetByUserName(userName);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }
        public async Task<User> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task<User> Create(CreateRequest model)
        {
            // validate
            if (await _userRepository.GetByEmail(model.Email!) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");

            if (await _userRepository.GetByEmail(model.UserName!) != null)
                throw new AppException("User with the UserName '" + model.UserName + "' already exists");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = BCrypt.HashPassword(model.Password);

            // save user
            return await _userRepository.Create(user);
        }

        public async Task<User> Update(int id, UpdateRequest model)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // validate
            var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            if (emailChanged && await _userRepository.GetByEmail(model.Email!) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");
            var uNameChanged = !string.IsNullOrEmpty(model.UserName) && user.UserName != model.UserName;
            if (uNameChanged && await _userRepository.GetByEmail(model.UserName!) != null)
                throw new AppException("User with the UserName '" + model.UserName + "' already exists");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.HashPassword(model.Password);

            // copy model props to user
            _mapper.Map(model, user);

            // save user
            return await _userRepository.Update(user);
        }

        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }
    }

