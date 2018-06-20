using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BL.ViewModels
{
    public class CreateOrderViewModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        //в очереди - 0, выполнение - 1, оплачен - 2, выполнен - 3, отменен - 4
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
