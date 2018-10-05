using System;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Filters;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly ISecurityService m_securityService;
        private readonly IUserService m_userService;
        public AuthController(IUserService userService, ISecurityService securityService) {
            this.m_userService = userService;
            this.m_securityService = securityService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        [ValidateModel]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO model) {
            try {
                var token = await this.m_userService.InsertUserAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Check")]
        [ValidateModel]
        public async Task<IActionResult> CheckUser([FromBody] CheckUserDTO model)
        {
            try {
                var userExist = await this.m_userService.CheckLoginAndEmailForExistAsync(model.Email, model.Login);
                return this.Ok(userExist);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        [ValidateModel]
        public async Task<IActionResult> Token([FromBody] LoginUserDTO model) {
            try {
                var token = await this.m_securityService.GetTokenAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        [ValidateModel]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model) {
            try {
                await this.m_securityService.ChangePasswordAsync(model);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPost("checkToken")]
        public IActionResult CheckToken(string user) {
            return this.Ok(this.User.Claims.FirstOrDefault()?.Value);
        }
    }
}
