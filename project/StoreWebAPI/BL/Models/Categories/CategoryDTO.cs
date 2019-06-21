using System;
using System.Collections.Generic;
using System.Text;

namespace ClothingStore.Service.Models.Categories
{
    public class CategoryDTO
    {
        public bool Active { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string RusName { get; set; }
    }
}
