using System;
using System.Threading.Tasks;
using ClothingStore.Filters;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly IUserService m_userService;

        public UserController(IUserService userService) {
            this.m_userService = userService;
        }

        [Authorize]
        [HttpGet("User")]
        public async Task<IActionResult> GetUser([FromQuery] string username) {
            try {
                var user = await this.m_userService.GetProfileByUsernameAsync(username);
                return this.Ok(user);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

        }

        [Authorize]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers() {
            try {
                var users = await this.m_userService.GetUsersAsync();
                return this.Ok(users);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }



        [Authorize]
        [HttpPut("Update")]
        [ValidateModel]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserDTO model) {
            try {
                await this.m_userService.UpdateUserAsync(model);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser([FromBody] long id) {
            try {
                await this.m_userService.DeleteUserAsync(id);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }          
        }
    }
}
