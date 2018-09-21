using System;
using System.Collections.Generic;
using System.Text;
using ClothingStore.Data.Entities;
using ClothingStore.Data.Entities.Order;

namespace ClothingStore.Service.Models.Order
{
    public class GetUserOrdersDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int Discount { get; set; } = 0;
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
