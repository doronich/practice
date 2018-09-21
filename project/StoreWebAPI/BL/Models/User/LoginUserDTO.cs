using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Service.Models {
    public class LoginUserDTO {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
