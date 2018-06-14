using System.Collections.Generic;
using DAL.Entities;

namespace BL.ViewModels {
    public class PaginatedResponseViewModel {
        public List<Item> Items { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public bool HasPrev { get; set; }
        public bool HasNext { get; set; }
    }
}
