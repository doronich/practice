﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Service.Pagination;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.Item;

namespace ClothingStore.Service.Services {
    public class ItemService : IItemService {
        private readonly IImageService m_imageService;
        private readonly IRepository<Item> m_itemRepository;
        private readonly Random m_rand = new Random();

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
            if(item == null) throw new Exception("Item not found.");

            await this.m_itemRepository.DeleteAsync(item);
        }

        public async Task<Item> GetItemAsync(long id) {
            var item = await this.m_itemRepository.GetByIdAsync(id);
            if(item == null) throw new Exception("Item not found.");
            item.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath);
            item.ImagePath1 = string.IsNullOrEmpty(item.ImagePath1) ? "" : await this.m_imageService.GetBase64StringAsync(item.ImagePath1);
            item.ImagePath2 = string.IsNullOrEmpty(item.ImagePath2) ? "" : await this.m_imageService.GetBase64StringAsync(item.ImagePath2);
            item.ImagePath3 = string.IsNullOrEmpty(item.ImagePath3) ? "" : await this.m_imageService.GetBase64StringAsync(item.ImagePath3);

            return item;
        }

        public async Task InsertItemAsync(CreateItemDTO model) {
            var item = new Item {
                Active = model.Active,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                Amount = model.Amount,
                Brand = model.Brand.ToLower(),
                Color = model.Color.ToLower(),
                Description = model.Description,
                Discount = model.Discount,
                PreviewImagePath = string.IsNullOrEmpty(model.PreviewImagePath)
                    ? "../ImageStore/default.png"
                    : await this.m_imageService.GetImagePathAsync(model.PreviewImagePath),
                ImagePath1 = string.IsNullOrEmpty(model.ImagePath1) ? "" : await this.m_imageService.GetImagePathAsync(model.ImagePath1),
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

        public async Task UpdateItemAsync(UpdateItemDTO model) {
            async Task<string> NewImage(string path, string image) {
                this.m_imageService.DeleteImage(path);
                return await this.m_imageService.GetImagePathAsync(image);
            }

            var item = await this.m_itemRepository.GetByIdAsync(model.Id);
            if(item == null) throw new Exception("Item not found.");

            item.Active = model.Active;
            item.CreatedBy = "Admin";
            item.CreatedDate = DateTime.UtcNow;
            item.Amount = model.Amount;
            item.Brand = model.Brand.ToLower();
            item.Color = model.Color.ToLower();
            item.Description = model.Description;
            item.Discount = model.Discount;
            item.PreviewImagePath = model.PreviewImagePath == null
                ? item.PreviewImagePath
                : await NewImage(item.PreviewImagePath, model.PreviewImagePath);
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
            item.UpdatedDate = DateTime.UtcNow;

            await this.m_itemRepository.UpdateAsync(item);
            if(item.Id <= 0) throw new Exception("Updating error.");
        }

        public async Task<IList<Item>> GetAllItemsAsync() {
            var query = await this.m_itemRepository.GetAllAsync();

            var items = await query.AnyAsync();
            if(!items) throw new Exception("Items not found.");
            var res = query.ToList();
            foreach(var item in res) item.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath);

            return res;
        }

        public async Task<object> GetItemsByKindAsync(ReqItemDTO item) {
            var expressionsList = new List<Expression<Func<Item, bool>>>();

            if(item.Color != null) {
                Expression<Func<Item, bool>> expColor = i => i.Color.Contains(item.Color, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expColor);
            }

            if(item.Brand != null) {
                Expression<Func<Item, bool>> expBrand = i => i.Brand.Contains(item.Brand, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expBrand);
            }

            if(item.Size != null) {
                Expression<Func<Item, bool>> expSize = i => i.Size.Contains(item.Size, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expSize);
            }

            if(item.Subkind != null) {
                Expression<Func<Item, bool>> expSubkind = i => string.Equals(i.Subkind, item.Subkind, StringComparison.CurrentCultureIgnoreCase);
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
                Gender gender;
                switch(item.Sex.ToLower()) {
                    case "male":
                        gender = Gender.Male;
                        break;
                    case "female":
                        gender = Gender.Female;
                        break;
                    default:
                        gender = Gender.Uni;
                        break;
                }

                Expression<Func<Item, bool>> expGender = i => i.Sex == gender || i.Sex == Gender.Uni;
                expressionsList.Add(expGender);
            }

            if(!string.IsNullOrEmpty(item.Name)) {
                Expression<Func<Item, bool>> expName = i => i.Name.Contains(item.Name, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expName);
            }

            Expression<Func<Item, bool>> expRange = i => i.Price <= item.EndPrice && i.Price >= item.StartPrice;
            expressionsList.Add(expRange);
            var query = await this.m_itemRepository.GetAllAsync(expressionsList);

            var result = await PaginationList<Item>.CreateAsync(query.AsNoTracking(), item.PageIndex ?? 1, item.PageSize ?? 12);

            if(result == null) throw new Exception("Items not found.");

            var items = result.Select(i => new FilteredItemDTO
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Active = i.Active,
                PreviewImagePath = Task.Run(async () => await this.m_imageService.GetBase64StringAsync(i.PreviewImagePath)).Result
            });
            return new { res = items, hasPrev = result.HasPreviousPage, hasNext = result.HasNextPage, total = result.TotalPages, index = result.PageIndex };
        }

        public async Task<IList<PreviewItemDTO>> GetLastAsync(int amount) {
            var query = await this.m_itemRepository.GetAllAsync();
            var items = query.Skip(Math.Max(0, query.Count() - amount));
            if(!items.Any()) throw new Exception("Items not found.");
            var res = new List<PreviewItemDTO>();
            foreach(var item in items)
                res.Add(new PreviewItemDTO {
                    Id = item.Id,
                    Name = item.Name,
                    Image = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath)
                });

            return res;
        }

        public async Task<IList<PreviewItemDTO>> GetRandomAsync(int amount) {
            var query = await this.m_itemRepository.GetAllAsync();
            var res = new List<PreviewItemDTO>();
            if(!query.Any()) throw new Exception("Items not found.");

            var list = new List<int>();
            var i = 0;
            while(i < amount) {
                var toSkip = this.m_rand.Next(0, query.Count());

                if(list.Contains(toSkip)) continue;
                list.Add(toSkip);

                var item = await query.Skip(toSkip).Take(1).FirstAsync();
                res.Add(new PreviewItemDTO {
                    Id = item.Id,
                    Name = item.Name,
                    Image = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath)
                });
                i++;
            }

            return res;
        }

        public async Task<IList<ShopCartDTO>> GetToCartAsync(long[] itemsId) {
            IList<ShopCartDTO> list = new List<ShopCartDTO>();
            foreach(var id in itemsId)
                try {
                    var item = await this.GetItemAsync(id);
                    list.Add(new ShopCartDTO {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price
                    });
                } catch {
                    // ignored
                }

            if(list.Count == 0) throw new Exception("Items not found");

            return list;
        }
    }
}
