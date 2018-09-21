using System;

namespace ClothingStore.Data.Entities.Order
{
    public class CouponCode:BaseEntity
    {
        public DateTime ExpiryDate { get; set; }
        public string Code { get;set; }
        public User User { get; set; }
        // in percents 50=50%
        public int Discount { get; set; } = 0;
    }
}
