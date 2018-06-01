using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
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

        #region public methods

        public async Task<string> GetTokenAsync(LoginUserViewModel model, bool reg = false)
        {
            return await this.TokenAsync(model, reg);
        }

        public async Task<string> EncryptPasswordAsync(string pass)
        {
            return await Task.Run(() => BcryptHash.GenerateBcryptHash(pass));
        }

        #endregion

        #region private methods

        private async Task<SymmetricSecurityKey> GetSymmetricSecurityKeyAsync()
        {
            return await Task.Run(() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Settings.Key)));
        }

        private async Task<string> TokenAsync(LoginUserViewModel model, bool reg)
        {
            var identity = await this.GetIdentityAsync(model, reg);
            //if (identity == null) throw new Exception("Invalid login or password.");

            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                this.Settings.Issuer,
                this.Settings.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(this.Settings.Lifetime)),
                signingCredentials: new SigningCredentials(await this.GetSymmetricSecurityKeyAsync(),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                acces_token = encodedJwt,
                username = identity.Name
  
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(LoginUserViewModel model, bool reg) {
            User user= new User();
            if (!reg)
            {
                var res = await this.m_userRepository.GetAllAsync(new List<Expression<Func<User, bool>>>{u => u.Login == model.Login});
                user = await res.FirstOrDefaultAsync();
                if (user == null) throw new Exception("Login not found.");
                var pass = user.Password;
                reg = await this.m_userRepository.ExistAsync(u => u.Login == model.Login && BcryptHash.CheckBcryptPassword(model.Password, pass));
            }

            if (reg)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims,
                    "TokenAsync",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        #endregion

    }
}
