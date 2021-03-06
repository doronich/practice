﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Helpers;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;
using ClothingStore.Service.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ClothingStore.Service.Services {
    public class SecurityService : ISecurityService {
        private readonly IRepository<User> m_userRepository;

        public SecurityService(IOptions<AuthSettings> options, IRepository<User> repository) {
            this.Settings = options.Value;
            this.m_userRepository = repository;
        }

        private AuthSettings Settings { get; }

        #region public methods

        public async Task<string> GetTokenAsync(LoginUserDTO model, bool reg = false) {
            return await this.TokenAsync(model, reg);
        }

        public async Task<string> EncryptPasswordAsync(string pass) {
            return await Task.Run(() => BcryptHash.GenerateBcryptHash(pass));
        }

        public async Task ChangePasswordAsync(ChangePasswordDTO model) {
            if(string.IsNullOrWhiteSpace(model.CurrentPassword) && string.IsNullOrWhiteSpace(model.NewPassword))
                throw new Exception("Password is empty.");

            var user = await this.m_userRepository.GetByIdAsync(model.Id);

            if(user == null) throw new Exception("User not found.");

            if(!BcryptHash.CheckBcryptPassword(model.CurrentPassword, user.Password))
                throw new Exception("Incorrect password.");

            user.Password = await this.EncryptPasswordAsync(model.NewPassword);
            await this.m_userRepository.UpdateAsync(user);

        }

        #endregion

        #region private methods

        private async Task<SymmetricSecurityKey> GetSymmetricSecurityKeyAsync() {
            return await Task.Run(() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Settings.Key)));
        }

        private async Task<string> TokenAsync(LoginUserDTO model, bool reg) {
            var identity = await this.GetIdentityAsync(model, reg);
            if(identity == null) throw new Exception("Invalid login or password.");

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                this.Settings.Issuer,
                this.Settings.Audience,
                notBefore : now,
                claims : identity.Claims,
                expires : now.Add(TimeSpan.FromMinutes(this.Settings.Lifetime)),
                signingCredentials : new SigningCredentials(await this.GetSymmetricSecurityKeyAsync(),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var id = long.Parse(identity.Claims.FirstOrDefault(i => i.Type == "Id")?.Value);
            var username = identity.Claims.FirstOrDefault(i => i.Type == "Login")?.Value;
            if (id == 0){
                id = (await (await this.m_userRepository.GetAllAsync(new List<Expression<Func<User, bool>>>() { u => u.Login == username }))
                                    .FirstOrDefaultAsync()).Id;
            }
            var response = new {
                access_token = encodedJwt,
                username = username,
                role = identity.Claims.FirstOrDefault(i => i.Type == "Role")?.Value,
                id = id
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(LoginUserDTO model, bool reg) {
            var user = new User { Role = UserRoles.User };

            if(!reg) {
                var res = await this.m_userRepository.GetAllAsync(new List<Expression<Func<User, bool>>> { u => u.Login == model.Login });
                user = await res.FirstOrDefaultAsync();
                if(user == null) throw new Exception("Login not found.");
                var pass = user.Password;
                reg = await this.m_userRepository.ExistAsync(u => u.Login == model.Login && BcryptHash.CheckBcryptPassword(model.Password, pass));
                if(!reg) throw new Exception("Incorrect password.");
            }

            var claims = new List<Claim> {
                new Claim("Login", model.Login),
                new Claim("Role", user.Role.ToString()),
                new Claim("Id", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                "TokenAsync",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        #endregion
    }
}
