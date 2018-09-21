using System.Collections.Generic;
using ClothingStore.Data.Entities.item;

namespace ClothingStore.Data.Entities.Categories
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; }
        public string RusName { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
