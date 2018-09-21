using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStore.Service.Models.Order
{
    public class CreateOrderItemDTO
    {
        public decimal Price { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
