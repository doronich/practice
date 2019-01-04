using System;
using System.Threading.Tasks;
using ClothingStore.Filters;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Produces("application/json")]
    [Route("api/item")]
    public class ItemController : Controller {
        private readonly IItemService m_itemService;
        public ItemController(IItemService itemService) {
            this.m_itemService = itemService;
        }

        //GET: api/item/all
        [AllowAnonymous]
        [ResponseCache(Duration = 10)]
        [HttpGet("all")]
        public async Task<IActionResult> All() {
            try {
                var items = await this.m_itemService.GetAllItemsAsync();
                return this.Ok(items);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
        [HttpGet("admin/all")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AllItems() {
            try{
                var items = await this.m_itemService.AllItemsForAdminAsync();
                return this.Ok(items);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }
        }

        //GET: api/item/cart
        [AllowAnonymous]
        [ResponseCache(Duration = 10)]
        [HttpGet("cart")]
        public async Task<IActionResult> GetToCart([FromQuery]string[] itemsId) {
            var temp = Array.ConvertAll(itemsId, long.Parse);
            try {
                var items = await this.m_itemService.GetToCartAsync(temp);
                return this.Ok(items);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // GET: api/item/q?
        [AllowAnonymous]
        [HttpGet("q")]
        //[ResponseCache(Duration = 10)]
        [ValidateModel]
        public async Task<IActionResult> GetBy([FromQuery]ReqItemDTO item) {
            try {
                var items = await this.m_itemService.GetItemsByKindAsync(item);
                return this.Ok(items);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // GET: api/item/last
        [AllowAnonymous]
        [HttpGet("last")]
        [ResponseCache(Duration = 1200)]
        [ValidateModel]
        public async Task<IActionResult> GetLast(int amount = 5) {
            try {
                var items = await this.m_itemService.GetLastAsync(amount);
                return this.Ok(items);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // GET: api/item/random
        [AllowAnonymous]
        [HttpGet("random")]
        [ResponseCache(Duration = 1200)]
        [ValidateModel]
        public async Task<IActionResult> GetRandom(int amount = 6) {
            try {
                var items = await this.m_itemService.GetRandomAsync(amount);
                return this.Ok(items);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // GET: api/item/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id) {
            try {
                var item = await this.m_itemService.GetItemAsync(id);
                return this.Ok(item);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // POST: api/item
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] CreateItemDTO item) {
            try {
                await this.m_itemService.InsertItemAsync(item);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // PUT: api/item
        [Authorize(Policy = "Admin")]
        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Put([FromBody] UpdateItemDTO item) {
            try {
                await this.m_itemService.UpdateItemAsync(item);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // DELETE: api/item/5
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                await this.m_itemService.DeleteItemAsync(id);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

    }
}
