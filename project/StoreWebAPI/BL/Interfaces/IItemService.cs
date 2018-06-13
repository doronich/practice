using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BL.ViewModels;
using DAL.Entities;

namespace BL.Interfaces
{
    public interface IItemService {
        Task InsertItemAsync(CreateItemViewModel item);

        Task UpdateItemAsync(UpdateItemViewModel item);

        Task DeleteItemAsync(long id);

        Task<IList<Item>> GetAllItemsAsync();

        Task<Item> GetItemAsync(long id);

        Task<IList<Item>> GetItemsByKindAsync(ReqItemViewModel item);

        Task<IList<PreviewItemViewModel>> GetLastAsync(int amount);
    }
}
