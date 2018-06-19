namespace BL.ViewModels {
    public class ShopCartViewModel {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; } = 1;
    }
}
