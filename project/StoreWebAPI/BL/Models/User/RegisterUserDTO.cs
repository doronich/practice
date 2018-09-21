﻿using System.ComponentModel.DataAnnotations;
using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Models {
    public class RegisterUserDTO {
        [Required]
        public string Login { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public UserRoles Role { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CreatedBy { get; set; }
    }
}
