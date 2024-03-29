﻿namespace ClothingStore.Service.Models.Item {
    public class ReqItemDTO {
        public string Color { get; set; }
        public string Brand { get; set; }
        public long? Kind { get; set; }
        public long? Subkind { get; set; }
        public string Status { get; set; }
        public string Size { get; set; }
        public string Sex { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? EndPrice { get; set; }
        public string Name { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; } = 12;
    }
}
