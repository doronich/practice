using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClothingStore.Service.Models.User
{
    public class ChangePasswordDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]       
        public string NewPassword { get; set; }
    }
}
