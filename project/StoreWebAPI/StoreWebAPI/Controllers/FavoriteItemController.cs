using System;
using System.Threading.Tasks;
using ClothingStore.Filters;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Fav;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api/favitem")]
    [ApiController]
    public class FavoriteItemController : ControllerBase {
        private readonly IFavoriteItemService m_favoriteItemService;

        public FavoriteItemController(IFavoriteItemService favoriteItemService) {
            this.m_favoriteItemService = favoriteItemService;
        }

        [Authorize]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddFavItem([FromBody]FavDTO model) {
            try {
                await this.m_favoriteItemService.AddFavoriteAsync(model.UserId, model.ItemId);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFavItems(long userId) {
            try {
                var result = await this.m_favoriteItemService.GetFavoriteItemsAsync(userId);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpGet("ids")]
        public async Task<IActionResult> GetFavIds(long userId)
        {
            try
            {
                var result = await this.m_favoriteItemService.GetFavoritesIdsAsync(userId);
                return this.Ok(result);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteFavItems(long userId, long itemId) {
            try {
                await this.m_favoriteItemService.DeleteFavoriteAsync(userId, itemId);
                return this.NoContent();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
    }
}
