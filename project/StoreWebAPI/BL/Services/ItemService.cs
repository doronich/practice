using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Service.Pagination;
using ClothingStore.Data.Entities;
using ClothingStore.Data.Entities.item;
using ClothingStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Cart;
using ClothingStore.Service.Models.Item;
using Microsoft.AspNetCore.Http;

namespace ClothingStore.Service.Services {
    public class ItemService : BaseService<Item>,IItemService {
        private readonly IImageService m_imageService;
        private const string DEFAULT_IMAGE_PATH_S = "../ImageStore/default.png";

        public ItemService(IRepository<Item> repository,
                           IImageService imageService,
                           IHttpContextAccessor accessor):base(accessor, repository) {
            this.m_imageService = imageService;
        }

        #region GET methods

        //public async Task<IList<string>> GetCategoriesAsync() {
        //    var categories = await (await this.Repository.GetAllAsync()).Select(i => i.Category.ToString()).Distinct().ToListAsync();
        //    return categories;
        //}

        //public async Task<IList<string>> GetSubCategoriesAsync()
        //{
        //    var subCategories = await (await this.Repository.GetAllAsync()).Select(i => i.SubCategory).Distinct().ToListAsync();
        //    return subCategories;
        //}

        public async Task<Item> GetItemAsync(long id)
        {
            var item = await this.Repository.GetByIdAsync(id);
            if (item == null) throw new Exception("Item not found.");
            item.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath);
            item.ImagePath1 = string.IsNullOrEmpty(item.ImagePath1) ? string.Empty : await this.m_imageService.GetBase64StringAsync(item.ImagePath1);
            item.ImagePath2 = string.IsNullOrEmpty(item.ImagePath2) ? string.Empty : await this.m_imageService.GetBase64StringAsync(item.ImagePath2);
            item.ImagePath3 = string.IsNullOrEmpty(item.ImagePath3) ? string.Empty : await this.m_imageService.GetBase64StringAsync(item.ImagePath3);

            return item;
        }

        public async Task<Item> GetItemById(long id) {
            return await this.GetByIdAsync(id);
        }
        public async Task<GetItemForCartDTO> GetItemForCartAsync(long id)
        {
            var item = await this.Repository.GetByIdAsync(id);
            if (item == null) throw new Exception("Item not found.");

            return new GetItemForCartDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            };
        }
        public async Task<object> GetItemsByKindAsync(ReqItemDTO item)
        {
            var expressionsList = new List<Expression<Func<Item, bool>>>();

            if (item.Color != null)
            {
                Expression<Func<Item, bool>> expColor = i => i.Color.Contains(item.Color, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expColor);
            }

            if (item.Brand != null)
            {
                Expression<Func<Item, bool>> expBrand = i => i.Brand.Contains(item.Brand, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expBrand);
            }

            if (item.Size != null)
            {
                Expression<Func<Item, bool>> expSize = i => i.Size.Contains(item.Size, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expSize);
            }


            if (item.Kind != null)
            {
                Expression<Func<Item, bool>> expKind = i => i.CategoryId == item.Kind;
                expressionsList.Add(expKind);
            }

            if (item.Subkind != null)
            {
                Expression<Func<Item, bool>> expSubkind = i => i.SubCategoryId == item.Subkind;
                expressionsList.Add(expSubkind);
            }

            if (item.Status != null)
            {
                Statuses status;
                switch (item.Status.ToLower())
                {
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

            if (item.Sex != null)
            {
                Gender gender;
                switch (item.Sex.ToLower())
                {
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

            if (!string.IsNullOrWhiteSpace(item.Name))
            {
                Expression<Func<Item, bool>> expName = i => i.Name.Contains(item.Name, StringComparison.OrdinalIgnoreCase);
                expressionsList.Add(expName);
            }

            if(item.StartPrice != null && item.EndPrice != null) {
                Expression<Func<Item, bool>> expRange = i => i.Price <= item.EndPrice && i.Price >= item.StartPrice;

                expressionsList.Add(expRange);
            }

            long.TryParse(this.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "Id")?.Value, out var userId);

            var query = (await this.Repository.GetAllAsync(expressionsList)).Include(i => i.FavoriteItems).Select(i => new GetFilteredItemDTO() {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                Active = i.Active,
                PreviewImagePath = i.MinPreviewImagePath != null
                    ? this.m_imageService.GetBase64String(i.MinPreviewImagePath)
                    :"",
                IsFavorite = i.FavoriteItems == null || i.FavoriteItems.Any(fa => fa.UserId == userId)
            });

            var result = await PaginationList<GetFilteredItemDTO>.CreateAsync(query, item.PageIndex ?? 1, item.PageSize ?? 12);

            if (result == null) throw new Exception("Items not found.");

            return new { res = result, hasPrev = result.HasPreviousPage, hasNext = result.HasNextPage, total = result.TotalPages, index = result.PageIndex };
        }
        public async Task<IList<GetPreviewItemDTO>> GetLastAsync(int amount)
        {
            var query = await this.Repository.GetAllAsync();

            var items = query.Skip(Math.Max(0, query.Count() - amount)).ToList();

            if (!items.Any()) throw new Exception("Items not found.");

            var res = new List<GetPreviewItemDTO>();
            foreach (var item in items)
                res.Add(new GetPreviewItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = await this.m_imageService.GetBase64StringAsync(item.MinPreviewImagePath)
                });

            return res;
        }
        public async Task<IList<GetPreviewItemDTO>> GetRandomAsync(int amount)
        {
            var query = await this.Repository.GetAllAsync();
            var res = new List<GetPreviewItemDTO>();
            if (!query.Any()) throw new Exception("Items not found.");

            var list = new List<int>();
            var rand = new Random();
            var i = 0;
            while (i < amount)
            {
                var toSkip = rand.Next(0, query.Count());

                if (list.Contains(toSkip)) continue;
                list.Add(toSkip);

                var item = await query.Skip(toSkip).Take(1).FirstAsync();
                res.Add(new GetPreviewItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath)
                });
                i++;
            }

            return res;
        }
        public async Task<IList<GetItemForCartDTO>> GetToCartAsync(long[] itemsId)
        {
            IList<GetItemForCartDTO> list = new List<GetItemForCartDTO>();
            foreach (var id in itemsId)
            {
                var item = await this.GetItemForCartAsync(id);
                list.Add(new GetItemForCartDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price
                });
            }

            if (list.Count == 0) throw new Exception("Items not found");

            return list;
        }
        public async Task<IList<Item>> GetAllItemsAsync()
        {
            var query = await this.Repository.GetAllAsync();

            var items = await query.AnyAsync();
            if (!items) throw new Exception("Items not found.");
            var res = query.ToList();
            foreach (var item in res) item.PreviewImagePath = await this.m_imageService.GetBase64StringAsync(item.PreviewImagePath);

            return res;
        }

        public async Task<IList<ItemForAdmin>> AllItemsForAdminAsync() {
            var list = await (await this.Repository.GetAllAsync()).Select(i => new ItemForAdmin()
            {
                Id = i.Id,
                Name = i.Name,
                Active = i.Active,
                Price = i.Price
            }).ToListAsync();
            return list;
        }
        #endregion

        public async Task InsertItemAsync(CreateItemDTO model) {
            var createdBy = this.HttpContext.User.Claims.FirstOrDefault()?.Value;

            var item = new Item {
                Active = model.Active,
                CreatedBy = createdBy,
                Amount = model.Amount,
                Brand = model.Brand.ToLower(),
                Color = model.Color.ToLower(),
                Description = model.Description,
                Discount = model.Discount,
                CategoryId = model.Kind,
                Name = model.Name,
                Price = model.Price,
                Sex = model.Sex,
                Size = model.Size.ToLower(),
                Status = model.Status,
                SubCategoryId = model.Subkind,
                
                PreviewImagePath = string.IsNullOrEmpty(model.PreviewImagePath)
                    ? DEFAULT_IMAGE_PATH_S
                    : await this.m_imageService.GetImagePathAsync(model.PreviewImagePath)
            };

            item.MinPreviewImagePath = string.IsNullOrEmpty(model.PreviewImagePath)
                ? DEFAULT_IMAGE_PATH_S
                : await this.m_imageService.GetMinImagePathAsync(item.PreviewImagePath);
            item.ImagePath1 = string.IsNullOrEmpty(model.ImagePath1) ? string.Empty : await this.m_imageService.GetImagePathAsync(model.ImagePath1);
            item.ImagePath2 = string.IsNullOrEmpty(model.ImagePath2) ? string.Empty : await this.m_imageService.GetImagePathAsync(model.ImagePath2);
            item.ImagePath3 = string.IsNullOrEmpty(model.ImagePath3) ? string.Empty : await this.m_imageService.GetImagePathAsync(model.ImagePath3);

            await this.Repository.CreateAsync(item);
            if(item.Id <= 0) throw new Exception("Creating error.");
        }
        public async Task UpdateItemAsync(UpdateItemDTO model) {

            var updatedBy = this.HttpContext.User.Claims.FirstOrDefault()?.Value;

            async Task<string> NewImage(string path, string image) {
                this.m_imageService.DeleteImage(path);
                return await this.m_imageService.GetImagePathAsync(image);
            }

            var item = await this.Repository.GetByIdAsync(model.Id);
            if(item == null) throw new Exception("Item not found.");

            item.Active = model.Active;
            item.Amount = model.Amount;
            item.Brand = model.Brand.ToLower();
            item.Color = model.Color.ToLower();
            item.Description = model.Description;
            item.Discount = model.Discount;
            item.PreviewImagePath = model.PreviewImagePath == null
                ? item.PreviewImagePath
                : await NewImage(item.PreviewImagePath, model.PreviewImagePath);
            item.MinPreviewImagePath = string.IsNullOrEmpty(model.PreviewImagePath)
                ? item.MinPreviewImagePath
                : await this.m_imageService.GetMinImagePathAsync(item.PreviewImagePath);
            item.ImagePath1 = string.IsNullOrEmpty(model.ImagePath1)
                ? item.ImagePath1
                : await NewImage(item.ImagePath1, model.ImagePath1);
            item.ImagePath2 = string.IsNullOrEmpty(model.ImagePath2)
                ? item.ImagePath2
                : await NewImage(item.ImagePath2, model.ImagePath2);
            item.ImagePath3 = string.IsNullOrEmpty(model.ImagePath3)
                ? item.ImagePath3
                : await NewImage(item.ImagePath3, model.ImagePath3);
            item.CategoryId = model.Kind;
            item.Name = model.Name;
            item.Price = model.Price;
            item.Sex = model.Sex;
            item.Size = model.Size.ToLower();
            item.Status = model.Status;
            item.SubCategoryId = model.Subkind;
            item.UpdatedBy = updatedBy;

            await this.Repository.UpdateAsync(item);
            if(item.Id <= 0) throw new Exception("Updating error.");
        }
        public async Task DeleteItemAsync(long id){
            await this.RemoveAsync(id);
        }

        public override async Task RemoveAsync(long id) {
            var item = await this.Repository.GetByIdAsync(id);

            if (item == null) throw new Exception("Item not found.");

            this.m_imageService.DeleteImage(item.PreviewImagePath);
            this.m_imageService.DeleteImage(item.MinPreviewImagePath);
            this.m_imageService.DeleteImage(item.ImagePath1);
            this.m_imageService.DeleteImage(item.ImagePath2);
            this.m_imageService.DeleteImage(item.ImagePath3);

            await this.Repository.DeleteAsync(item);
        }
    }
}
