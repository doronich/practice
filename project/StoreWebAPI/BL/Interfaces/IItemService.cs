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

        Task UpdateItemAsync(Item item);

        Task DeleteItemAsync(long id, bool deactive = true);

        Task<IList<Item>> GetAllItemAsync();

        Task<Item> GetItemAsync(long id);

        Task<IList<Item>> GetItemsByKindAsync(ReqItemViewModel item);
    }
}
