namespace ClothingStore.Service.Models.Categories
{
    public class SubCategoryDTO
    {
        public bool Active { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string RusName { get; set; }
        public long CategoryId { get; set; }
    }
}
