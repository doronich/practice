using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace StoreWebAPI.Controllers {
    [Route("api/order")]
    [Produces("application/json")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly IOrderService m_orderService;

        public OrderController(IOrderService orderService) {
            this.m_orderService = orderService;
        }


        // POST: api/Order
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
