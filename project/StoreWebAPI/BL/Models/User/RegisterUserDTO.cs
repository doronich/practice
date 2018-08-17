using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Models {
    public class RegisterUserDTO {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CreatedBy { get; set; }
    }
}
