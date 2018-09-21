using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Service.Services {
    public class UserService : BaseService<User>, IUserService {
        private readonly ISecurityService m_security;

        public UserService(IRepository<User> repository, ISecurityService securityService, IHttpContextAccessor accessor) : base(accessor, repository) {
            this.m_security = securityService;
        }

        public async Task<string> InsertUserAsync(RegisterUserDTO model) {
            var ex = await this.Repository.ExistAsync(u => u.Login == model.Login) || await this.Repository.ExistAsync(e => e.Email == model.Email);

            if(ex) throw new Exception("Login or email already exists.");

            var user = new User {
                Login = model.Login,
                Email = model.Email,
                Password = await this.m_security.EncryptPasswordAsync(model.Password),
                Role = UserRoles.User,
                CreatedBy = model.Login,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Active = true
            };

            await this.Repository.CreateAsync(user);
            if(user.Id <= 0) throw new Exception("Registration error.");

            return await this.m_security.GetTokenAsync(new LoginUserDTO { Login = user.Login, Password = model.Password }, true);
        }

        public async Task UpdateUserAsync(UpdateUserDTO model) {
            var user = await this.Repository.GetByIdAsync(model.Id);

            if(user == null) throw new Exception("User not found.");

            user.FirstName = model.Firstname ?? user.FirstName;
            user.LastName = model.Lastname ?? user.LastName;
            user.UpdatedBy = this.HttpContext.User.Claims.FirstOrDefault()?.Value;
            user.PhoneNumber = model.PhoneNumber;

            await this.Repository.UpdateAsync(user);

            if(user.Id <= 0) throw new Exception("Update error.");
        }

        public async Task DeleteUserAsync(long id) {
            await this.RemoveAsync(id);
        }

        #region GET methods

        public async Task<GetUserDTO> GetUserInfoByIdAsync(long id) {
            var user = await this.Repository.GetByIdAsync(id);
            if(user == null) throw new Exception("User not found.");
            return new GetUserDTO {
                UserName = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddedDate = user.CreatedDate,
                Role = user.Role
            };
        }

        public async Task<User> GetUserByIdAsync(long id) {
            var user = await this.GetByIdAsync(id);
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username) {
            var users = await this.Repository.GetAllAsync();
            var user = await users.Where(i => i.Login == username).FirstAsync();
            if(user == null) throw new Exception("User not found.");
            return user;
        }

        public async Task<GetProfileDTO> GetProfileByUsernameAsync(string username) {
            var users = await this.Repository.GetAllAsync();
            var user = await users.Where(i => i.Login == username).FirstAsync();
            if(user == null) throw new Exception("User not found.");
            return new GetProfileDTO {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<IList<GetUserDTO>> GetUsersAsync() {
            var query = await this.Repository.GetAllAsync();
            var users = await query.AnyAsync();
            if(!users) throw new Exception("Users not found.");

            return query.Select(item => new GetUserDTO {
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.Login,
                AddedDate = item.CreatedDate,
                Role = item.Role
            }).ToList();
        }

        #endregion
    }
}
