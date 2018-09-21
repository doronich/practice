namespace ClothingStore.Service.Models.Item
{
    public class GetFilteredItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public string PreviewImagePath { get; set; }
        public bool IsFavorite { get; set; }
    }
}
