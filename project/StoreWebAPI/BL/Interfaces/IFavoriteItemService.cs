using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClothingStore.Data.Entities.item;
using ClothingStore.Service.Models.Item;

namespace ClothingStore.Service.Interfaces
{
    public interface IFavoriteItemService {
        Task<IList<GetFavItemDTO>> GetFavoriteItemsAsync(long userId);

        Task<IList<long>> GetFavoritesIdsAsync(long userId);

        Task AddFavoriteAsync(long userId, long itemId);

        Task DeleteFavoriteAsync(long userId, long itemId);
    }
}
