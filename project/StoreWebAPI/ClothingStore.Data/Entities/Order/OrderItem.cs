namespace ClothingStore.Data.Entities.Order {
    public class OrderItem :BaseEntity {
        public decimal Price { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; }
    }
}
