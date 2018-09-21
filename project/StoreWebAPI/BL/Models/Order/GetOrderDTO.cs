using System;

using ClothingStore.Data.Entities.Order;
using ClothingStore.Service.Models.CouponCode;

namespace ClothingStore.Service.Models.Order
{
    public class GetOrderDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }

        public decimal TotalPrice { get; set; }
        public long? UserId { get; set; }
        public string Username { get; set; }
        public GetCouponCodeDTO Code { get; set; }
        //в очереди - 0, выполнение - 1, оплачен - 2, выполнен - 3, отменен - 4
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
