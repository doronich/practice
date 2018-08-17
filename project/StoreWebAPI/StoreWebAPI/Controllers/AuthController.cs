using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IUserService m_userService;
        private readonly ISecurityService m_securityService;
        public AuthController(IUserService userService, ISecurityService securityService) {
            this.m_userService = userService;
            this.m_securityService = securityService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO model)
        {
            if (!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");
            try
            {
                var token = await this.m_userService.InsertUserAsync(model);
                return this.Ok(token);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody] LoginUserDTO model)
        {
            if (!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");

            try
            {
                var token = await this.m_securityService.GetTokenAsync(model);
                return this.Ok(token);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model) {
            if(!this.ModelState.IsValid) {
                return this.BadRequest("Incorrect data.");
            }

            try {
                var result = await this.m_securityService.ChangePasswordAsync(model);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
        
        [Authorize]
        [HttpPost("checkToken")]
        public IActionResult CheckToken(string user)
        {
            return this.Ok(this.User.Claims.FirstOrDefault()?.Value);
        }
    }
}