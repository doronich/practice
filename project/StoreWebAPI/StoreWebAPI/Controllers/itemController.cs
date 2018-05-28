using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoreWebAPI.Controllers {
    
    [Produces("application/json")]
    [Route("api/item")]
    public class ItemController : Controller {
        private readonly IItemService m_itemService;

        public ItemController(IItemService itemService) {
            this.m_itemService = itemService;
        }

        //GET: api/item
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            IEnumerable<Item> items;

            try
            {
                items = await this.m_itemService.GetAllItemAsync();
                return Json(items);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }


        }

        // GET: api/item/q?
        [HttpGet("q")]
        public async Task<IActionResult> GetBy(ReqItemViewModel item)
        {
            if(!ModelState.IsValid) {
                return this.BadRequest("Incorrect request");
            }
            IEnumerable<Item> items;

            try
            {
                items = await this.m_itemService.GetItemsByKindAsync(item);
                return Json(items);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }


        }


        // GET: api/item/5
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
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateItemViewModel item) {
            if(!this.ModelState.IsValid) return this.BadRequest("Incorrect data.");

            try {
                await this.m_itemService.InsertItemAsync(item);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // PUT: api/item/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}
