using System;
using System.Threading.Tasks;
using ClothingStore.Filters;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.CouponCode;
using ClothingStore.Service.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api/order")]
    [Produces("application/json")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly IOrderService m_orderService;
        private readonly ICouponService m_couponService;
        public OrderController(IOrderService orderService, ICouponService couponService) {
            this.m_orderService = orderService;
            this.m_couponService = couponService;
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
        [Authorize]
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
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] CreateOrderDTO model) {
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
        [ValidateModel]
        public async Task<IActionResult> Put(UpdateOrderDTO model) {
            try {
                await this.m_orderService.UpdateOrderAsync(model);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpPost("code")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GenerateCode(int amount = 1) {
            try {
                var model = new CreateCouponCodeDTO {
                    Discount = 10,
                    ExpiryDate = DateTime.UtcNow + TimeSpan.FromDays(10)
                };

                await this.m_couponService.CreateCouponAsync(model,amount);
                return this.Ok();
            } catch(Exception ex) {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("code")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetCode(string code)
        {
            try {
                var result = await this.m_couponService.GetCouponByCodeAsync(code);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("userorders")]
        public async Task<IActionResult> GetUsersOrders([FromQuery]long id) {
            try {
                var result = await this.m_orderService.GetUsersOrdersAsync(id);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpGet("checkcode")]
        public async Task<IActionResult> CheckCode([FromQuery]string code) {
            try {
                var result = await this.m_couponService.CheckCodeAsync(code);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            try {
                await this.m_orderService.RemoveOrderAsync(id);
                return this.NoContent();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("order")]
        public async Task<IActionResult> GetOrder(long id) {
            try {
                var result = await this.m_orderService.GetOrderToUpdateAsync(id);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

    }
}
