namespace ClothingStore.Service.Models.Cart
{
    public class GetItemForCartDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; } = 1;
    }
}
