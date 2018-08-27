using System;
using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Models {
    public class UserDTO {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime AddedDate { get; set; }
        public UserRoles Role { get; set; }
    }
}
