namespace ClothingStore.Data.Entities {
    public class OrderItem {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public long OrderId { get; set; }
    }
}
