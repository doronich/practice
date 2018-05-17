using System;
using DAL.Entities;

namespace BL.ViewModels {
    public class UserViewModel {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime AddedDate { get; set; }

        public UserRoles Role { get; set; }
    }
}
