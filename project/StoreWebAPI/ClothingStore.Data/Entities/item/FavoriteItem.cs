namespace ClothingStore.Data.Entities.item
{
    public class FavoriteItem:BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long ItemId { get; set; }
        public Item Item { get; set; }
    }
}
