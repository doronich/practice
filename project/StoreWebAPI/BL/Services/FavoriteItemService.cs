using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Data.Entities.item;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Item;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Service.Services {
    public class FavoriteItemService : IFavoriteItemService {
        private readonly IImageService m_imageService;
        private readonly IItemService m_itemService;
        private readonly IRepository<FavoriteItem> m_repository;
        private readonly IUserService m_userService;

        public FavoriteItemService(IRepository<FavoriteItem> repository,
                                   IUserService userService,
                                   IItemService itemService,
                                   IImageService imageService) {
            this.m_repository = repository;
            this.m_userService = userService;
            this.m_itemService = itemService;
            this.m_imageService = imageService;
        }

        public async Task<IList<long>> GetFavoritesIdsAsync(long userId) {
            var query = await this.GetFavoritesAsync(userId);

            var favItems = await query.Select(i => i.ItemId).ToListAsync();

            return favItems;
        }

        public async Task<IList<GetFavItemDTO>> GetFavoriteItemsAsync(long userId)
        {
            var query = await this.GetFavoritesAsync(userId);

            var favItems = await query.Select(i => new GetFavItemDTO
            {
                Id = i.ItemId,
                Name = i.Item.Name,
                Price = i.Item.Price,
                Active = i.Item.Active,
                PreviewImagePath = this.m_imageService.GetBase64String(i.Item.MinPreviewImagePath)
            }).ToListAsync();

            return favItems;
        }

        private async Task<IQueryable<FavoriteItem>> GetFavoritesAsync(long userId) {
            var user = await this.m_userService.GetUserByIdAsync(userId);

            if (user == null) throw new Exception("User not found.");

            return await this.m_repository.GetAllAsync(new List<Expression<Func<FavoriteItem, bool>>> { i => i.UserId == userId });

        }

        public async Task AddFavoriteAsync(long userId, long itemId) {
            var user = await this.m_userService.GetUserByIdAsync(userId);
            var item = await this.m_itemService.GetItemById(itemId);

            if(user == null) throw new Exception("User not found.");
            if(item == null) throw new Exception("Item not found.");

            var exist = await this.m_repository.ExistAsync(i => i.UserId == userId && i.ItemId == itemId);

            if(exist) return;

            var favItem = new FavoriteItem {
                User = user,
                Item = item,
                UserId = user.Id,
                ItemId = item.Id,
                CreatedBy = user.Login,
                Active = true
            };

            await this.m_repository.CreateAsync(favItem);
        }

        public async Task DeleteFavoriteAsync(long userId, long itemId) {
            var user = await this.m_userService.GetUserByIdAsync(userId);

            if(user == null) throw new Exception("User not found.");

            var item = await this.m_itemService.GetItemById(itemId);

            if(item == null) throw new Exception("Item not found");

            var favItem =
                await (await this.m_repository.GetAllAsync(new List<Expression<Func<FavoriteItem, bool>>> { f => f.UserId == user.Id && f.ItemId == item.Id }))
                    .FirstOrDefaultAsync();

            if(favItem == null) throw new Exception("Favorite item not found");

            await this.m_repository.DeleteAsync(favItem);
        }
    }
}
