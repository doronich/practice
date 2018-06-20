using System.Collections.Generic;
using System.Threading.Tasks;
using BL.ViewModels;
using DAL.Entities;

namespace BL.Interfaces {
    public interface IItemService {
        Task InsertItemAsync(CreateItemViewModel item);

        Task UpdateItemAsync(UpdateItemViewModel item);

        Task DeleteItemAsync(long id);

        Task<IList<Item>> GetAllItemsAsync();

        Task<Item> GetItemAsync(long id);

        Task<object> GetItemsByKindAsync(ReqItemViewModel item);

        Task<IList<PreviewItemViewModel>> GetLastAsync(int amount);

        Task<IList<PreviewItemViewModel>> GetRandomAsync(int amount);

        Task<IList<ShopCartViewModel>> GetToCartAsync(long[] itemsId);
    }
}
