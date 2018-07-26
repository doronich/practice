using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization.Json;
using DAL.Entities;

namespace StoreWebAPI.Controllers {
    [Route("api/order")]
    [Produces("application/json")]
    [ApiController]
    public class OrderController : Controller {
        private readonly IOrderService m_orderService;

        public OrderController(IOrderService orderService) {
            this.m_orderService = orderService;
        }

        // GET: api/orders
        [HttpGet("orders")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAll() {
            try {
                var result = await this.m_orderService.GetOrdersAsync();
                return this.Json(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetOrderItems(long id)
        {
            try {
                var result = await this.m_orderService.GetOrderItemsAsync(id);
                return this.Json(result);
            }
            catch (Exception exception)
            {
                return this.BadRequest(exception.Message);
            }
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderViewModel model) {
            if(!this.ModelState.IsValid) return this.BadRequest();

            try {
                await this.m_orderService.CreateOrderAsync(model);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
    }
}
