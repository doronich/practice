using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoreWebAPI.Controllers {
    [Route("api")]
    public class UserController : Controller {
        private readonly ISecurityService m_securityService;
        private readonly IUserService m_userService;

        public UserController(IUserService userService, ISecurityService securityService) {
            this.m_userService = userService;
            this.m_securityService = securityService;
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUser(long id) {
            UserViewModel user;
            try {
                user = await this.m_userService.GetUserAsync(id);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Json(user);
        }

        [Authorize]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers() {
            IList<UserViewModel> users;

            try {
                users = await this.m_userService.GetUsersAsync();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Json(users);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");
            try {
                var token = await this.m_userService.InsertUserAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");
            try {
                await this.m_userService.UpdateUserAsync(model);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Ok();
        }
        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser([FromBody] long id) {
            try {
                await this.m_userService.DeleteUserAsync(id);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Ok();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] LoginUserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");

            try {
                var token = await this.m_securityService.GetTokenAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPost("checkToken")]
        public async Task<IActionResult> CheckToken(string user)
        {

            return this.Ok();
        }
    }
}
