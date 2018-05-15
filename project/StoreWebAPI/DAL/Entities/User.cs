using System.ComponentModel.DataAnnotations;

namespace DAL.Entities {
    public class User : BaseEntity {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Login { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(20)]
        public string Firstname { get; set; }
        [StringLength(20)]
        public string Lastname { get; set; }
        [Required]
        public int Role { get; set; }
    }
}
