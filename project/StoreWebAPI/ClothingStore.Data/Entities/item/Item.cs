using System.Collections.Generic;

namespace ClothingStore.Data.Entities.item {
    public class Item : BaseEntity {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; } = 0;
        public string PreviewImagePath { get; set; }
        public string MinPreviewImagePath { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public KindsOfItems Kind { get; set; }
        public string Subkind { get; set; }
        public Statuses Status { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public Gender Sex { get; set; }
        public IEnumerable<FavoriteItem> FavoriteItems { get; set; }

        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }

    }
}
