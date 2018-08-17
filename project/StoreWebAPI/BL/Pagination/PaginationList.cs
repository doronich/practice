using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Service.Pagination {
    public class PaginationList<T> : List<T> {
        public PaginationList(List<T> items,int count, int pageIndex, int pageSize) {
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.PageIndex = pageIndex;

            this.AddRange(items);
        }

        public int PageIndex { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => this.PageIndex > 1;

        public bool HasNextPage => this.PageIndex < this.TotalPages;

        public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize) {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationList<T>(items, count, pageIndex, pageSize);

        }
    }
}
