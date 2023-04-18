
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Threading.Tasks;
using WebAPI_CURD.Entities;
using WebAPI_CURD.Helpers;
using WebAPI_CURD.Models.Users;
using WebAPI_CURD.Repositories;

namespace WebAPI_CURD.Services
{

    /// <summary>
    /// core business logic and validation related to user CRUD operations.连接 controllers 和 repositories
    /// </summary>
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task Create(CreateRequest model)
        {
            // validate
            if (await _userRepository.GetByEmail(model.Email) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");

            if (await _userRepository.GetByEmail(model.UserName) != null)
                throw new AppException("User with the UserName '" + model.UserName + "' already exists");

            if (!Enum.TryParse(model.Role, true, out Role role))
            {
                role = Role.User;
            }
            var user = new User()
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = role,
                Title = model.Title,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)  // hash password
            };

            // save user
            await _userRepository.Create(user);
        }

        public async Task Update(UpdateRequest model)
        {
            var user = await _userRepository.GetById(model.Id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            // validate
            var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            if (emailChanged && await _userRepository.GetByEmail(model.Email) != null)
                throw new AppException("User with the email '" + model.Email + "' already exists");
            var uNameChanged = !string.IsNullOrEmpty(model.UserName) && user.UserName != model.UserName;
            if (uNameChanged && await _userRepository.GetByEmail(model.UserName) != null)
                throw new AppException("User with the UserName '" + model.UserName + "' already exists");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

             user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Title = model.Title;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);  // hash password


            if (!Enum.TryParse(model.Role, true, out Role role))
            {
                role = Role.User;
            }
            user.Role = role;


            // save user
            await _userRepository.Update(user);
        }

        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }
    }

}