using ClothingStore.Data.Entities.Order;

namespace ClothingStore.Service.Models.Order
{
    public class UpdateOrderDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
    }
}
