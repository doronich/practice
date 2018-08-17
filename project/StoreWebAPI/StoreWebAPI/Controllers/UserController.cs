using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly ISecurityService m_securityService;
        private readonly IUserService m_userService;

        public UserController(IUserService userService, ISecurityService securityService) {
            this.m_userService = userService;
            this.m_securityService = securityService;
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUser(long id) {
            UserDTO user;
            try {
                user = await this.m_userService.GetUserAsync(id);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
            return this.Ok(user);
        }

        [Authorize]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers() {
            IList<UserDTO> users;

            try {
                users = await this.m_userService.GetUsersAsync();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }

            return this.Ok(users);
        }



        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> EditUser([FromBody] UpdateUserDTO model) {
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
    }
}
