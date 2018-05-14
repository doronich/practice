using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace StoreWebAPI.Controllers {
    public class AccountController : Controller {
        //private readonly ApplicationContext m_context;

        //public AccountController(ApplicationContext context) {
        //    this.m_context = context;

        //}

        //[HttpPost]
        //[Route("/reg")]
        //public async Task<IActionResult> Register([FromBody] RegisterViewModel model) {
        //    //var login = this.Request.B["login"];
        //    //var password = this.Request.Form["password"];
        //    var user = new User { Login = login, Password = password };
        //    this.m_context.Users.Add(user);
        //    await this.m_context.SaveChangesAsync();
        //    return this.Ok();
        //}

        //[HttpPost("/token")]
        //public async Task Token() {
        //    var username = this.Request.Form["username"];
        //    var password = this.Request.Form["password"];

        //    var identity = this.GetIdentity(username, password);

        //    if(identity == null) {
        //        this.Response.StatusCode = 400;
        //        await this.Response.WriteAsync("Invalid username or password.");
        //        return;
        //    }

        //    var now = DateTime.UtcNow;
        //    // создаем JWT-токен

        //    var jwt = new JwtSecurityToken(
        //        AuthOptions.ISSUER,
        //        AuthOptions.AUDIENCE,
        //        notBefore : now,
        //        claims : identity.Claims,
        //        expires : now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
        //        signingCredentials : new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
        //            SecurityAlgorithms.HmacSha256));

        //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        //    var response = new {
        //        access_token = encodedJwt,
        //        username = identity.Name
        //    };

        //    // сериализация ответа
        //    this.Response.ContentType = "application/json";
        //    await this.Response.WriteAsync(JsonConvert.SerializeObject(response,
        //        new JsonSerializerSettings { Formatting = Formatting.Indented }));
        //}

        //private ClaimsIdentity GetIdentity(string username, string password) {
        //    var user = this.m_context.Users.FirstOrDefault(x => x.Login == username && x.Password == password);

        //    if(user != null) {
        //        var claims = new List<Claim> {
        //            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
        //            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
        //        };
        //        var claimsIdentity = new ClaimsIdentity(claims,
        //            "Token",
        //            ClaimsIdentity.DefaultNameClaimType,
        //            ClaimsIdentity.DefaultRoleClaimType);
        //        return claimsIdentity;
        //    }

        //    return null;
        //}
    }
}
