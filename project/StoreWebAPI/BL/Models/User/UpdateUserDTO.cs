using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Service.Models {
    public class UpdateUserDTO {
        [Required]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string UpdatedBy { get; set; }
    }
}
