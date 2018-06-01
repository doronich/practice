using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;

namespace StoreWebAPI.Controllers {
    [Produces("application/json")]
    [Route("api/item")]
    public class ItemController : Controller {
        private readonly IItemService m_itemService;

        public ItemController(IItemService itemService) {
            this.m_itemService = itemService;
        }

        //GET: api/item/all
        [AllowAnonymous]
        [ResponseCache(Duration = 360)]
        [HttpGet("all")]
        public async Task<IActionResult> All() {

            try {
                IEnumerable<Item> items = await this.m_itemService.GetAllItemsAsync();
                return this.Json(items);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // GET: api/item/q?
        [AllowAnonymous]
        [HttpGet("q")]
        public async Task<IActionResult> GetBy(ReqItemViewModel item) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect request");

            try {
                IEnumerable<Item> items = await this.m_itemService.GetItemsByKindAsync(item);
                return this.Json(items);
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
                return this.Json(item);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // POST: api/item
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreateItemViewModel item) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");

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
        public async Task<IActionResult> Put([FromBody] UpdateItemViewModel item) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");

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
