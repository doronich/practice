using System;
using System.Collections.Generic;
using System.Text;
using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Models.Order
{
    public class UpdateOrderStatusDTO
    {
        public long Id { get; set; }
        //в очереди - 0, выполнение - 1, оплачен - 2, выполнен - 3, отменен - 4
        public OrderStatus Status { get; set; }

    }
}
