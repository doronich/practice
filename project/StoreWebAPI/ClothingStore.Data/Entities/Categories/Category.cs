using System.Collections.Generic;
using ClothingStore.Data.Entities.item;

namespace ClothingStore.Data.Entities.Categories
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public string RusName { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
