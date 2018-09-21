namespace ClothingStore.Service.Models.Order
{
    public class GetOrderItemsDTO
    {
        public decimal Price { get; set; }
        public long ItemId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
