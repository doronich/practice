using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services {
    public class ItemService : IItemService {
        private readonly IRepository<Item> m_itemRepository;
        private readonly IImageService m_imageService;

        public ItemService(IRepository<Item> itemRepository, IImageService imageService) {
            this.m_itemRepository = itemRepository;
            this.m_imageService = imageService;
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

        public async Task<Item> GetItemAsync(long id) {
            var item = await this.m_itemRepository.GetByIdAsync(id);
            if(item == null) throw new Exception("Item not found.");
            return item;
        }

        public async Task InsertItemAsync(CreateItemViewModel model) {
            Item item = new Item {
                Active = true,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                Amount = model.Amount,
                Brand = model.Brand,
                Color = model.Color,
                Description = model.Description,
                Discount = model.Discount,
                ImagePath = model.Image==null? "": await this.m_imageService.GetImagePathAsync(model.Image),
                Kind = model.Kind,
                Name = model.Name,
                Price = model.Price,
                Sex = model.Sex,
                Size = model.Size,
                Status = model.Status,
                Subkind = model.Subkind
            };

            
            
            await this.m_itemRepository.CreateAsync(item);
            if(item.Id <= 0) throw new Exception("Creating error.");
        }

        public async Task UpdateItemAsync(Item item) {
            var res = await this.m_itemRepository.GetByIdAsync(item.Id);
            if(res == null) throw new Exception("Item not found.");
            await this.m_itemRepository.UpdateAsync(item);
        }

        public async Task<IList<Item>> GetAllItemAsync() {
            var query = await this.m_itemRepository.GetAllAsync();
            var items = await query.AnyAsync();
            if(!items) throw new Exception("Items not found.");
            return query.ToList();
        }

        public async Task<IList<Item>> GetItemsByKindAsync(ReqItemViewModel item) {
            var kind = KindsOfItems.Other;
            var sex = Sex.Uni;
            var status = Statuses.Default;
            var expressionsList = new List<Expression<Func<Item, bool>>>();

            if(item.Kind != null)
                switch(item.Kind.ToLower()) {
                    case "sneakers":
                        kind = KindsOfItems.Footwear;
                        break;
                    case "clothing":
                        kind = KindsOfItems.Clothing;
                        break;
                    case "accessories":
                        kind = KindsOfItems.Accessories;
                        break;
                    default:
                        kind = KindsOfItems.Other;
                        break;
                }

            if(item.Sex != null)
                switch(item.Sex.ToLower()) {
                    case "male":
                        sex = Sex.Male;
                        break;
                    case "female":
                        sex = Sex.Female;
                        break;
                    default:
                        sex = Sex.Uni;
                        break;
                }

            if(item.Status != null)
                switch(item.Status.ToLower()) {
                    case "discounted":
                        status = Statuses.Discounted;
                        break;
                    case "new":
                        status = Statuses.New;
                        break;

                    default:
                        status = Statuses.Default;
                        break;
                }

            Expression<Func<Item, bool>> expColor = i => i.Color == item.Color;
            Expression<Func<Item, bool>> expKind = i => i.Kind == kind;
            Expression<Func<Item, bool>> expStatus = i => i.Status == status;
            Expression<Func<Item, bool>> expSex = i => i.Sex == sex;
            Expression<Func<Item, bool>> expSubkind = i => i.Subkind == item.Subkind;
            Expression<Func<Item, bool>> expBrand = i => i.Brand == item.Brand;
            Expression<Func<Item, bool>> expSize = i => i.Brand == item.Brand;

            if(item.Color != null) expressionsList.Add(expColor);
            if(item.Brand != null) expressionsList.Add(expBrand);
            if(item.Size != null) expressionsList.Add(expSize);
            if(item.Subkind != null) expressionsList.Add(expSubkind);
            expressionsList.AddRange(new List<Expression<Func<Item, bool>>> { expKind, expStatus, expSex });

            var query = await this.m_itemRepository.GetAllAsync(expressionsList);
            //if(query==null) throw new Exception("");
            return query.ToList();
        }
    }
}
