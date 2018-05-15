using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using StoreWebAPI.Models;

namespace StoreWebAPI.Controllers {
    public class UserController : Controller {
        private readonly IUserService m_userService;

        public UserController(IUserService userService) {
            this.m_userService = userService;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<List<UserViewModel>> GetUsersAsync() {
            var model = new List<UserViewModel>();
            var users = await this.m_userService.GetUsersAsync();

            foreach(var item in users.ToList()) {
                var user = new UserViewModel {
                    Id = item.Id,
                    FirstName = item.Firstname,
                    LastName = item.Lastname,
                    UserName = item.Login,
                    Email = item.Email
                };
                model.Add(user);
            }

            return model;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody]UserViewModel model) {
            if(!this.ModelState.IsValid) {
                return this.BadRequest();
            }

            User user = new User {
                Login = model.UserName,
                Password = model.Password,
                Role = (int)UserRoles.User,
                Email = model.Email,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Active = true,
                CreatedBy = "DM",
                CreatedDate = DateTime.UtcNow
            };
            await this.m_userService.InsertUserAsync(user);
            if(user.Id>0) {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
