﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStore.Service.Models.CouponCode
{
    public class GetCouponCodeDTO {
        public double Discount { get; set; } = 0;
        public string Code { get; set; }
    }
}
