using System.ComponentModel.DataAnnotations;
using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Models.Item {
    public class UpdateItemDTO {
        [Required]
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public long? Kind { get; set; }
        public long? Subkind { get; set; }
        public Statuses Status { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public Gender Sex { get; set; }

        public string PreviewImagePath { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
    }
}
