using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Models;
using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Interfaces {
    public interface IItemService {
        Task InsertItemAsync(CreateItemDTO item);

        Task UpdateItemAsync(UpdateItemDTO item);

        Task DeleteItemAsync(long id);

        Task<IList<Item>> GetAllItemsAsync();

        Task<Item> GetItemAsync(long id);

        Task<object> GetItemsByKindAsync(ReqItemDTO item);

        Task<IList<PreviewItemDTO>> GetLastAsync(int amount);

        Task<IList<PreviewItemDTO>> GetRandomAsync(int amount);

        Task<IList<ShopCartDTO>> GetToCartAsync(long[] itemsId);
    }
}
