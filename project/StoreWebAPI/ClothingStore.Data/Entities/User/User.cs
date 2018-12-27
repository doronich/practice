using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Data.Entities.User {
    public class User : BaseEntity {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public UserRoles Role { get; set; }

        public string PhoneNumber { get; set; }
    }
}
