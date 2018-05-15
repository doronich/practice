using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using StoreWebAPI.Models;

namespace StoreWebAPI.Controllers {
    [Route("api")]
    public class UserController : Controller {
        private readonly IUserService m_userService;

        public UserController(IUserService userService) {
            this.m_userService = userService;
        }

        [HttpGet("User")]
        public async Task<UserViewModel> GetUser(long id) {
            UserViewModel user;
            try {
                var temp = await this.m_userService.GetUserAsync(id);
                user = new UserViewModel {
                    FirstName = temp.Firstname,
                    LastName = temp.Lastname,
                    UserName = temp.Login,
                    Email = temp.Email
                };
            } catch(Exception exception) {
                return null;
            }

            return user;
        }

        [HttpGet("Users")]
        public async Task<List<UserViewModel>> GetUsersAsync() {
            var model = new List<UserViewModel>();

            var users = await this.m_userService.GetUsersAsync();
            if(!users.Any()) return null;
            try {
                users.ToList().ForEach(item => {
                    var user = new UserViewModel {
                        Id = item.Id,
                        FirstName = item.Firstname,
                        LastName = item.Lastname,
                        UserName = item.Login,
                        Email = item.Email,
                        AddedDate = item.CreatedDate
                    };
                    model.Add(user);
                });
            } catch(Exception exception) {
                throw;
            }

            return model;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest();

            var user = new User {
                Login = model.UserName,
                Password = model.Password,
                Role = (int)UserRoles.User,
                Email = model.Email,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Active = true,
                CreatedBy = "DM",
                CreatedDate = DateTime.Now
            };
            try {
                await this.m_userService.InsertUserAsync(user);
            } catch(Exception exception) {
                return this.BadRequest();
            }

            if(user.Id > 0) return this.Ok();
            return this.BadRequest();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> EditUserAsync([FromBody] UserViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest();
            var user = await this.m_userService.GetUserAsync(model.Id);
            user.Email = model.Email ?? user.Email;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = "DM";
            user.Firstname = model.FirstName ?? user.Firstname;
            user.Lastname = model.LastName ?? user.Lastname;
            await this.m_userService.UpdateUserAsync(user);

            if(user.Id > 0) return this.Ok();

            return this.BadRequest();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserAsync([FromForm] long id) {
            try {
                await this.m_userService.DeleteUserAsync(id);
            } catch(Exception exception) {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
