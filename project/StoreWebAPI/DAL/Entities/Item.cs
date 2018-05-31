namespace DAL.Entities {
    public class Item : BaseEntity {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string PreviewImagePath { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public KindsOfItems Kind { get; set; }
        public string Subkind { get; set; }
        public Statuses Status { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public Sex Sex { get; set; }
    }
}
