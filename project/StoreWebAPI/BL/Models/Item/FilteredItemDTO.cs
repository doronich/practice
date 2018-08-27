using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStore.Service.Models.Item
{
    public class FilteredItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public string PreviewImagePath { get; set; }
    }
}
