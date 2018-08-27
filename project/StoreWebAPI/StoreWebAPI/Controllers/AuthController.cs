using System;
using System.Linq;
using System.Threading.Tasks;
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
        private const string INCORRECT_DATA = "Incorrect data.";
        public AuthController(IUserService userService, ISecurityService securityService) {
            this.m_userService = userService;
            this.m_securityService = securityService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO model) {
            if(!this.ModelState.IsValid) return this.BadRequest(INCORRECT_DATA);
            try {
                var token = await this.m_userService.InsertUserAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] LoginUserDTO model) {
            if(!this.ModelState.IsValid) return this.BadRequest(INCORRECT_DATA);

            try {
                var token = await this.m_securityService.GetTokenAsync(model);
                return this.Ok(token);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model) {
            if(!this.ModelState.IsValid) return this.BadRequest(INCORRECT_DATA);

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
