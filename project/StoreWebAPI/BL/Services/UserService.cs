using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Service.Services {
    public class UserService : IUserService {
        private readonly ISecurityService m_security;
        private readonly IRepository<User> m_userRepository;

        public UserService(IRepository<User> userRepository, ISecurityService securityService) {
            this.m_userRepository = userRepository;
            this.m_security = securityService;
        }

        public async Task DeleteUserAsync(long id) {
            var user = await this.m_userRepository.GetByIdAsync(id);
            if(user == null) throw new Exception("User not found.");
            await this.m_userRepository.DeleteAsync(user);
        }

        public async Task<UserDTO> GetUserAsync(long id) {
            var user = await this.m_userRepository.GetByIdAsync(id);
            if(user == null) throw new Exception("User not found.");
            return new UserDTO {
                UserName = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddedDate = user.CreatedDate,
                Role = user.Role
            };
        }

        public async Task<IList<UserDTO>> GetUsersAsync() {
            var query = await this.m_userRepository.GetAllAsync();
            var users = await query.AnyAsync();
            if(!users) throw new Exception("No users found.");

            return query.Select(item => new UserDTO {
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.Login,
                AddedDate = item.CreatedDate,
                Role = item.Role
            }).ToList();
        }

        public async Task<string> InsertUserAsync(RegisterUserDTO model) {
            var user = new User {
                Login = model.Login,
                Email = model.Email,
                Password = await this.m_security.EncryptPasswordAsync(model.Password),
                Role = UserRoles.User,
                CreatedBy = model.CreatedBy ?? "Admin1",
                CreatedDate = DateTime.UtcNow,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Active = true
            };
            var ex = !await this.m_userRepository.ExistAsync(u => u.Login == user.Login) && !await this.m_userRepository.ExistAsync(e => e.Email == user.Email);
            if(ex) await this.m_userRepository.CreateAsync(user);
            else throw new Exception("Login or email already exists.");
            if(user.Id <= 0) throw new Exception("Registration error.");

            return await this.m_security.GetTokenAsync(new LoginUserDTO { Login = user.Login, Password = model.Password }, true);
        }

        public async Task UpdateUserAsync(UpdateUserDTO model) {
            var user = await this.m_userRepository.GetByIdAsync(model.Id);
            if(user == null) throw new Exception("User not found.");
            user.Email = model.Email ?? user.Email;
            user.Password = model.Password == null ? user.Password : await this.m_security.EncryptPasswordAsync(model.Password);
            user.FirstName = model.Firstname ?? user.FirstName;
            user.LastName = model.Lastname ?? user.LastName;
            user.UpdatedBy = model.UpdatedBy ?? user.UpdatedBy;
            user.UpdatedDate = DateTime.UtcNow;

            await this.m_userRepository.UpdateAsync(user);

            if(user.Id <= 0) throw new Exception("Update error.");
        }
    }
}
