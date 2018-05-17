using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BL.Additional;
using BL.Interfaces;
using BL.Options;
using BL.ViewModels;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BL.Services {
    public class SecurityService : ISecurityService {
        private readonly IRepository<User> m_userRepository;
        public SecurityService(IOptions<AuthSettings> options, IRepository<User> repository) {
            this.Settings = options.Value;
            this.m_userRepository = repository;
        }

        private AuthSettings Settings { get; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Settings.Key));
        }

        public async Task<string> GetTokenAsync(LoginUserViewModel model, bool reg=false) {
            return await this.TokenAsync(model,reg);
        }

        public string EncryptPassword(string pass) {
            var plainText = pass;

            string res = BcryptHash.GenerateBcryptHash(plainText);

            return res;
        }

        private async Task<string> TokenAsync(LoginUserViewModel model,bool reg) {
            var identity = await this.GetIdentityAsync(model,reg);
            if(identity == null) throw new Exception("Invalid login or password.");

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                this.Settings.Issuer,
                this.Settings.Audience,
                notBefore : now,
                claims : identity.Claims,
                expires : now.Add(TimeSpan.FromMinutes(this.Settings.Lifetime)),
                signingCredentials : new SigningCredentials(this.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new {
                acces_token = encodedJwt,
                username = identity.Name
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(LoginUserViewModel model, bool reg) {
            
            if (!reg) {
                var res = await this.m_userRepository.GetAllAsync(u=>u.Login==model.Login);
                var user = await res.FirstOrDefaultAsync();
                string pass = user.Password;
                reg = await this.m_userRepository.ExistAsync(u => u.Login == model.Login && BcryptHash.CheckBcryptPassword(model.Password, pass));
            }
            

            if(reg) {
                var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, model.Role.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims,
                    "TokenAsync",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
