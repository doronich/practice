using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStore.Service.Models.User
{
    public class ProfileDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
