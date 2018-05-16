using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.Options;
using BL.ViewModels;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BL.Services {
    public class UserService : IUserService {
        private readonly SecurityService m_security;
        private readonly IRepository<User> m_userRepository;

        public UserService(IRepository<User> userRepository, IOptions<AuthSettings> options) {
            this.m_userRepository = userRepository;
            this.m_security = new SecurityService(options);
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
                FirstName = user.Firstname,
                LastName = user.Lastname,
                AddedDate = user.CreatedDate
            };
        }

        //almost
        public async Task<IList<UserViewModel>> GetUsersAsync() {
            //var query = predicate == null ? await this.m_userRepository.GetAllAsync() : await this.m_userRepository.GetAllAsync(predicate);

            var query = await this.m_userRepository.GetAllAsync();

            return query.Select(item => new UserViewModel {
                FirstName = item.Firstname,
                LastName = item.Lastname,
                UserName = item.Login,
                AddedDate = item.CreatedDate
            }).ToList();
        }

        //ok
        public async Task<string> InsertUserAsync(RegisterUserViewModel model) {
            var user = new User {
                Login = model.Login,
                Email = model.Email,
                Password = model.Password,
                Role = UserRoles.User,
                CreatedBy = model.CreatedBy ?? "Admin1",
                CreatedDate = DateTime.UtcNow,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Active = true
            };

            var ex = await this.m_userRepository.ExistAsync(u => u.Login == user.Login) && await this.m_userRepository.ExistAsync(e => e.Email == user.Email);
            if(!ex) await this.m_userRepository.CreateAsync(user);
            else throw new Exception("Login already exist.");
            if(user.Id <= 0) throw new Exception("Registration error.");

            return await this.GetTokenAsync(new LoginUserViewModel { Login = user.Login, Password = user.Password, Role = user.Role });
        }

        //ok
        public async Task UpdateUserAsync(UpdateUserViewModel model) {
            var user = await this.m_userRepository.GetByIdAsync(model.Id);

            user.Email = model.Email ?? user.Email;
            user.Password = model.Password ?? user.Password;
            user.Firstname = model.Firstname ?? user.Firstname;
            user.Lastname = model.Lastname ?? user.Lastname;
            user.UpdatedBy = model.UpdatedBy ?? user.UpdatedBy;
            user.UpdatedDate = DateTime.UtcNow;

            await this.m_userRepository.UpdateAsync(user);

            if(user.Id <= 0) throw new Exception("Update error.");
        }

        public async Task<string> GetTokenAsync(LoginUserViewModel model) {
            return await this.Token(model);
        }

        private async Task<string> Token(LoginUserViewModel model) {
            var identity = await this.GetIdentity(model);
            if(identity == null) throw new Exception("Invalid login or password.");

            //error
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                this.m_security.Settings.Issuer,
                this.m_security.Settings.Audience,
                notBefore : now,
                claims : identity.Claims,
                expires : now.Add(TimeSpan.FromMinutes(this.m_security.Settings.Lifetime)),
                signingCredentials : new SigningCredentials(this.m_security.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new {
                acces_token = encodedJwt,
                username = identity.Name
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private async Task<ClaimsIdentity> GetIdentity(LoginUserViewModel model) {
            var user = await this.m_userRepository.ExistAsync(u => u.Login == model.Login && u.Password == model.Password);

            if(user) {
                var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, model.Role.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims,
                    "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
