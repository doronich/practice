using System.ComponentModel.DataAnnotations;

namespace DAL.Entities {
    //доделать
    public class User : BaseEntity {
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Role { get; set; }
    }
}
