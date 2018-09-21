using System;

namespace ClothingStore.Service.Models.CouponCode
{
    public class CreateCouponCodeDTO
    {
        public DateTime ExpiryDate { get; set; }
        // in percents 50=50%
        public int Discount { get; set; } = 0;
    }
}
