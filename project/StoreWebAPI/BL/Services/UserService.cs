using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services {
    public class UserService : IUserService {
        private readonly ISecurityService m_security;
        private readonly IRepository<User> m_userRepository;

        public UserService(IRepository<User> userRepository, ISecurityService securityService) {
            this.m_userRepository = userRepository;
            this.m_security = securityService;
        }

        //DELETE
        public async Task DeleteUserAsync(long id) {
            var user = await this.m_userRepository.GetByIdAsync(id);
            if(user == null) throw new Exception("User not found.");
            await this.m_userRepository.DeleteAsync(user);
        }

        //USER
        public async Task<UserViewModel> GetUserAsync(long id) {
            var user = await this.m_userRepository.GetByIdAsync(id);
            if(user == null) throw new Exception("User not found.");
            return new UserViewModel {
                UserName = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddedDate = user.CreatedDate,
                Role = user.Role
            };
        }

        //USERs
        public async Task<IList<UserViewModel>> GetUsersAsync() {
            //var query = predicate == null ? await this.m_userRepository.GetAllAsync() : await this.m_userRepository.GetAllAsync(predicate);

            var query = await this.m_userRepository.GetAllAsync();
            var users = await query.AnyAsync();
            if(!users)
            {
                throw new Exception("No users found.");
            }

            return query.Select(item => new UserViewModel {
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.Login,
                AddedDate = item.CreatedDate,
                Role = item.Role
            }).ToList();
        }

        //ISERT
        public async Task<string> InsertUserAsync(RegisterUserViewModel model) {
            var user = new User {
                Login = model.Login,
                Email = model.Email,
                Password = await this.m_security.EncryptPasswordAsync(model.Password),
                Role = UserRoles.User,
                CreatedBy = model.CreatedBy ?? "Admin1",
                CreatedDate = DateTime.Now,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Active = true
            };
            //true true
            var ex = !await this.m_userRepository.ExistAsync(u => u.Login == user.Login) && !await this.m_userRepository.ExistAsync(e => e.Email == user.Email);
            if(ex) await this.m_userRepository.CreateAsync(user);
            else throw new Exception("Login or email already exists.");
            if(user.Id <= 0) throw new Exception("Registration error.");

            return await this.m_security.GetTokenAsync(new LoginUserViewModel { Login = user.Login, Password = model.Password, Role = user.Role }, true);
        }

        //UPDATE
        public async Task UpdateUserAsync(UpdateUserViewModel model) {
            var user = await this.m_userRepository.GetByIdAsync(model.Id);
            if(user == null) throw new Exception("User not found.");
            user.Email = model.Email ?? user.Email;
            user.Password = model.Password == null ? user.Password : await this.m_security.EncryptPasswordAsync(model.Password);
            user.FirstName = model.Firstname ?? user.FirstName;
            user.LastName = model.Lastname ?? user.LastName;
            user.UpdatedBy = model.UpdatedBy ?? user.UpdatedBy;
            user.UpdatedDate = DateTime.Now;

            await this.m_userRepository.UpdateAsync(user);

            if(user.Id <= 0) throw new Exception("Update error.");
        }
    }
}
