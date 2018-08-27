﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api/order")]
    [Produces("application/json")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly IOrderService m_orderService;
        private const string INCORRECT_DATA = "Incorrect data.";
        public OrderController(IOrderService orderService) {
            this.m_orderService = orderService;
        }

        // GET: api/orders
        [HttpGet("orders")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAll() {
            try {
                var result = await this.m_orderService.GetOrdersAsync();
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetOrderItems(long id) {
            try {
                var result = await this.m_orderService.GetOrderItemsAsync(id);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDTO model) {
            if(!this.ModelState.IsValid) return this.BadRequest(INCORRECT_DATA);

            try {
                await this.m_orderService.CreateOrderAsync(model);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
        // PUT: api/order
        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(UpdateOrderStatusDTO model) {
            if(!this.ModelState.IsValid) return this.BadRequest(INCORRECT_DATA);

            try {
                await this.m_orderService.UpdateOrderStatusAsync(model, this.User.Claims.FirstOrDefault()?.Value);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }


    }
}
