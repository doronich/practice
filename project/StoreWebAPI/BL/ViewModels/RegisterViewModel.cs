using System.ComponentModel.DataAnnotations;

namespace BL.ViewModels {
    public class RegisterViewModel {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
