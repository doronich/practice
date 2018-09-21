using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClothingStore.Data.Entities.Order;

namespace ClothingStore.Service.Models.Order {
    public class CreateOrderDTO {
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Comment { get; set; }
        public string Address { get; set; }

        public string Code { get; set; }

        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }

        [Required]
        public List<CreateOrderItemDTO> Items { get; set; }
    }
}
