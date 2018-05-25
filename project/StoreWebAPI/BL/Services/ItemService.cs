using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services {
    public class ItemService : IItemService {
        private readonly IRepository<Item> m_itemRepository;

        public ItemService(IRepository<Item> itemRepository) {
            this.m_itemRepository = itemRepository;
        }

        public async Task DeleteItemAsync(long id, bool deactive = true) {
            var item = await this.m_itemRepository.GetByIdAsync(id);
            if(item == null) throw new Exception("Item not found.");

            if(deactive) {
                item.Active = false;
                await this.m_itemRepository.UpdateAsync(item);
            } else {
                await this.m_itemRepository.DeleteAsync(item);
            }
        }

        public Task<Item> GetItemAsync(long id) {
            throw new NotImplementedException();
        }

        public Task InsertItemAsync(Item item) {
            throw new NotImplementedException();
        }

        public Task UpdateItemAsync(Item item) {
            throw new NotImplementedException();
        }

        public async Task<IList<Item>> GetAllItemAsync() {
            var query = await this.m_itemRepository.GetAllAsync();
            var items = query.AnyAsync();
            if(items == null) throw new Exception("Items not found.");
            return query.ToList();
        }

        public async Task<IList<Item>> GetItemsByKindAsync(string sortingParam= ";") {
            var param = sortingParam.Split(';');
            KindsOfItems param1;
            var param2 = param[1];
            switch(param[0]) {
                case "sneakers":
                    param1 = KindsOfItems.Footwear;
                    break;
                case "clothing":
                    param1 = KindsOfItems.Clothing;
                    break;
                case "accessories":
                    param1 = KindsOfItems.Accessories;
                    break;
                default:
                    param1 = KindsOfItems.Other;
                    break;
            }

            var query = await this.m_itemRepository.GetAllAsync(i => i.Kind == param1 && i.Subkind == param2);
            return query.ToList();
        }
    }
}
