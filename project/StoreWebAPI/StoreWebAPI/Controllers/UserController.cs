using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace StoreWebAPI.Controllers {
    [Route("api")]
    public class UserController : Controller {
        private readonly IUserService m_userService;

        public UserController(IUserService userService) {
            this.m_userService = userService;
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

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsersAsync() {
            IList<UserViewModel> users;

            try {
                users = await this.m_userService.GetUsersAsync();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Json(users);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest("!");
            try {
                var token = await this.m_userService.InsertUserAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> EditUserAsync([FromBody] UpdateUserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest();
            try {
                await this.m_userService.UpdateUserAsync(model);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] long id) {
            try {
                await this.m_userService.DeleteUserAsync(id);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Ok();
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] LoginUserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest();

            try {
                var token = await this.m_userService.GetTokenAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
    }
}
