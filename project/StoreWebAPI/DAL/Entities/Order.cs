﻿using System.Collections.Generic;

namespace DAL.Entities
{
    public class Order:BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }

        public decimal TotalPrice { get; set; }
        //в очереди - 0, выполнение - 1, оплачен - 2, выполнен - 3, отменен - 4
        public OrderStatus Status { get; set; } = OrderStatus.Queue;
        public List<OrderItem> Items { get; set; }

    }
}
