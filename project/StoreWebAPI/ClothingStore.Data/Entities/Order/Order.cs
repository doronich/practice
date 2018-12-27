using System.Collections.Generic;

namespace ClothingStore.Data.Entities.Order {
    public class Order : BaseEntity {

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public User.User User { get; set; }
        public CouponCode Code { get; set; }
        public OrderStatus Status { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
