using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BL.ViewModels
{
    public class UpdateItemViewModel
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public KindsOfItems Kind { get; set; }
        public string Subkind { get; set; }
        public Statuses Status { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public Sex Sex { get; set; }

        public string Image { get; set; }
    }
}
