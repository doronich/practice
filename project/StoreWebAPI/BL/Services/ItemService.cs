﻿using System;
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
        private readonly IImageService m_imageService;
        private readonly IRepository<Item> m_itemRepository;

        public ItemService(IRepository<Item> itemRepository, IImageService imageService) {
            this.m_itemRepository = itemRepository;
            this.m_imageService = imageService;
        }

        
        public async Task DeleteItemAsync(long id) {
            var item = await this.m_itemRepository.GetByIdAsync(id);
            this.m_imageService.DeleteImage(item.PreviewImagePath);
            this.m_imageService.DeleteImage(item.ImagePath1);
            this.m_imageService.DeleteImage(item.ImagePath2);
            this.m_imageService.DeleteImage(item.ImagePath3);
            if (item == null) throw new Exception("Item not found.");

                await this.m_itemRepository.DeleteAsync(item);

        }

        public async Task<Item> GetItemAsync(long id) {
            var item = await this.m_itemRepository.GetByIdAsync(id);
            item.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath);
            item.ImagePath1 = string.IsNullOrEmpty(item.ImagePath1) ? "" : await this.m_imageService.GetBase64StringAsync(item.ImagePath1);
            item.ImagePath2 = string.IsNullOrEmpty(item.ImagePath2) ? "" : await this.m_imageService.GetBase64StringAsync(item.ImagePath2);
            item.ImagePath3 = string.IsNullOrEmpty(item.ImagePath3) ? "" : await this.m_imageService.GetBase64StringAsync(item.ImagePath3);
            if (item == null) throw new Exception("Item not found.");
            return item;
        }

        public async Task InsertItemAsync(CreateItemViewModel model) {
            var item = new Item {
                Active = model.Active,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                Amount = model.Amount,
                Brand = model.Brand.ToLower(),
                Color = model.Color.ToLower(),
                Description = model.Description,
                Discount = model.Discount,
                PreviewImagePath = string.IsNullOrEmpty(model.PreviewImagePath) ? "../ImageStore/default.png" : await this.m_imageService.GetImagePathAsync(model.PreviewImagePath),
                ImagePath1 = string.IsNullOrEmpty(model.ImagePath1) ? "": await this.m_imageService.GetImagePathAsync(model.ImagePath1),
                ImagePath2 = string.IsNullOrEmpty(model.ImagePath2) ? "" : await this.m_imageService.GetImagePathAsync(model.ImagePath2),
                ImagePath3 = string.IsNullOrEmpty(model.ImagePath3) ? "" : await this.m_imageService.GetImagePathAsync(model.ImagePath3),
                Kind = model.Kind,
                Name = model.Name,
                Price = model.Price,
                Sex = model.Sex,
                Size = model.Size.ToLower(),
                Status = model.Status,
                Subkind = model.Subkind.ToLower()
            };

            await this.m_itemRepository.CreateAsync(item);
            if(item.Id <= 0) throw new Exception("Creating error.");
        }

        public async Task UpdateItemAsync(UpdateItemViewModel model) {

            async Task<string> NewImage(string path, string image) {
                this.m_imageService.DeleteImage(path);
                return await this.m_imageService.GetImagePathAsync(image);
            }

            var item = await this.m_itemRepository.GetByIdAsync(model.Id);
            if (item == null) throw new Exception("Item not found.");

            item.Active = model.Active;
            item.CreatedBy = "Admin";
            item.CreatedDate = DateTime.Now;
            item.Amount = model.Amount;
            item.Brand = model.Brand.ToLower();
            item.Color = model.Color.ToLower();
            item.Description = model.Description;
            item.Discount = model.Discount;
            item.PreviewImagePath = model.PreviewImagePath == null
                ? item.PreviewImagePath
                : await NewImage(item.PreviewImagePath,model.PreviewImagePath);
            item.ImagePath1 = string.IsNullOrEmpty(model.ImagePath1) ? item.ImagePath1 : await NewImage(item.ImagePath1, model.ImagePath1);
            item.ImagePath2 = string.IsNullOrEmpty(model.ImagePath2) ? item.ImagePath2 : await NewImage(item.ImagePath2, model.ImagePath2);
            item.ImagePath3 = string.IsNullOrEmpty(model.ImagePath3) ? item.ImagePath3 : await NewImage(item.ImagePath3, model.ImagePath3);
            item.Kind = model.Kind;
            item.Name = model.Name;
            item.Price = model.Price;
            item.Sex = model.Sex;
            item.Size = model.Size.ToLower();
            item.Status = model.Status;
            item.Subkind = model.Subkind.ToLower();
            item.UpdatedBy = model.Username;
            item.UpdatedDate = DateTime.Now;

            await this.m_itemRepository.UpdateAsync(item);
            if(item.Id <= 0) throw new Exception("Updating error.");
        }

        public async Task<IList<Item>> GetAllItemsAsync() {
            var query = await this.m_itemRepository.GetAllAsync();

            var items = await query.AnyAsync();
            if(!items) throw new Exception("Items not found.");
            var res = query.ToList();
            foreach(var item in res){ item.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath);}

            return res;
        }

        public async Task<IList<Item>> GetItemsByKindAsync(ReqItemViewModel item) {
            var expressionsList = new List<Expression<Func<Item, bool>>>();

            if(item.Color != null) {
                Expression<Func<Item, bool>> expColor = i => String.Equals(i.Color, item.Color, StringComparison.CurrentCultureIgnoreCase);
                expressionsList.Add(expColor);
            }

            if(item.Brand != null) {
                Expression<Func<Item, bool>> expBrand = i => String.Equals(i.Brand, item.Brand, StringComparison.CurrentCultureIgnoreCase);
                expressionsList.Add(expBrand);
            }

            if(item.Size != null) {
                Expression<Func<Item, bool>> expSize = i => String.Equals(i.Size, item.Size, StringComparison.CurrentCultureIgnoreCase);
                expressionsList.Add(expSize);
            }

            if(item.Subkind != null) {
                Expression<Func<Item, bool>> expSubkind = i => String.Equals(i.Subkind, item.Subkind, StringComparison.CurrentCultureIgnoreCase);
                expressionsList.Add(expSubkind);
            }

            if(item.Kind != null) {
                KindsOfItems kind;
                switch(item.Kind.ToLower()) {
                    case "1":
                        kind = KindsOfItems.Footwear;
                        break;
                    case "2":
                        kind = KindsOfItems.Clothing;
                        break;
                    case "3":
                        kind = KindsOfItems.Accessories;
                        break;
                    default:
                        kind = KindsOfItems.Other;
                        break;
                }

                Expression<Func<Item, bool>> expKind = i => i.Kind == kind;
                expressionsList.Add(expKind);
            }

            if(item.Status != null) {
                Statuses status;
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

                Expression<Func<Item, bool>> expStatus = i => i.Status == status;
                expressionsList.Add(expStatus);
            }

            if(item.Sex != null) {
                Sex sex;
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

                Expression<Func<Item, bool>> expSex = i => i.Sex == sex || i.Sex == Sex.Uni;
                expressionsList.Add(expSex);
            }

            if(!string.IsNullOrEmpty(item.Name)) {
                Expression<Func<Item, bool>> expName = i => i.Name.Contains(item.Name, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expName);
            }

            Expression<Func<Item, bool>> expRange = i => i.Price <= item.EndPrice && i.Price >=item.StartPrice;
            expressionsList.Add(expRange);
            var query = await this.m_itemRepository.GetAllAsync(expressionsList);
            if(query == null) throw new Exception("Some troubles with items!@?1");
            var res = query.ToList();
            foreach (var i in res) { i.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(i.PreviewImagePath); }

            return res;
        }
    }
}
