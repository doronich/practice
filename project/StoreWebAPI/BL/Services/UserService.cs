using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using DAL.Entities;
using DAL.Interfaces;

namespace BL.Services {
    public class UserService : IUserService {
        private readonly ISecurityService m_security;
        private readonly IRepository<User> m_userRepository;

        public UserService(IRepository<User> userRepository, ISecurityService securityService) {
            this.m_userRepository = userRepository;
            this.m_security = securityService;
        }

        //ok
        public async Task DeleteUserAsync(long id) {
            var user = await this.m_userRepository.GetByIdAsync(id);
            if(user == null) throw new Exception("User didn't fount.");
            await this.m_userRepository.DeleteAsync(user);
        }

        //ok
        public async Task<UserViewModel> GetUserAsync(long id) {
            var user = await this.m_userRepository.GetByIdAsync(id);
            return new UserViewModel {
                UserName = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddedDate = user.CreatedDate
            };
        }

        //almost
        public async Task<IList<UserViewModel>> GetUsersAsync() {
            //var query = predicate == null ? await this.m_userRepository.GetAllAsync() : await this.m_userRepository.GetAllAsync(predicate);

            var query = await this.m_userRepository.GetAllAsync();

            return query.Select(item => new UserViewModel {
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.Login,
                AddedDate = item.CreatedDate
            }).ToList();
        }

        //ok
        public async Task<string> InsertUserAsync(RegisterUserViewModel model) {
            var user = new User {
                Login = model.Login,
                Email = model.Email,
                Password = this.m_security.EncryptPassword(model.Password),
                Role = UserRoles.User,
                CreatedBy = model.CreatedBy ?? "Admin1",
                CreatedDate = DateTime.UtcNow,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Active = true
            };
            //true true
            var ex = !await this.m_userRepository.ExistAsync(u => u.Login == user.Login) && !await this.m_userRepository.ExistAsync(e => e.Email == user.Email);
            if(ex) await this.m_userRepository.CreateAsync(user);
            else throw new Exception("Login or email already exist.");
            if(user.Id <= 0) throw new Exception("Registration error.");

            return await this.m_security.GetTokenAsync(new LoginUserViewModel { Login = user.Login, Password = model.Password, Role = user.Role },true);
        }

        //ok
        public async Task UpdateUserAsync(UpdateUserViewModel model) {
            var user = await this.m_userRepository.GetByIdAsync(model.Id);

            user.Email = model.Email ?? user.Email;
            user.Password = this.m_security.EncryptPassword(model.Password) ?? user.Password;
            user.FirstName = model.Firstname ?? user.FirstName;
            user.LastName = model.Lastname ?? user.LastName;
            user.UpdatedBy = model.UpdatedBy ?? user.UpdatedBy;
            user.UpdatedDate = DateTime.UtcNow;

            await this.m_userRepository.UpdateAsync(user);

            if(user.Id <= 0) throw new Exception("Update error.");
        }

        
    }
}
