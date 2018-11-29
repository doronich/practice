using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Data.Entities.item;
using ClothingStore.Service.Models.Cart;
using ClothingStore.Service.Models.Item;

namespace ClothingStore.Service.Interfaces {
    public interface IItemService {
        Task InsertItemAsync(CreateItemDTO item);

        Task UpdateItemAsync(UpdateItemDTO item);

        Task DeleteItemAsync(long id);

        Task<IList<Item>> GetAllItemsAsync();

        Task<Item> GetItemAsync(long id);

        Task<object> GetItemsByKindAsync(ReqItemDTO item);

        Task<IList<GetPreviewItemDTO>> GetLastAsync(int amount);

        Task<IList<GetPreviewItemDTO>> GetRandomAsync(int amount);

        Task<IList<GetItemForCartDTO>> GetToCartAsync(long[] itemsId);

        Task<GetItemForCartDTO> GetItemForCartAsync(long id);

        Task<Item> GetItemById(long id);

        Task<IList<ItemForAdmin>> AllItemsForAdminAsync();

        //Task<IList<string>> GetCategoriesAsync();
        //Task<IList<string>> GetSubCategoriesAsync();
    }
}
