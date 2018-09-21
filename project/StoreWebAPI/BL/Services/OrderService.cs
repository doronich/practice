using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Data.Entities.Order;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.CouponCode;
using ClothingStore.Service.Models.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Service.Services {
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly ICouponService m_couponService;
        private readonly IUserService m_userService;
        private readonly IRepository<OrderItem> m_orderItemRepository;

        public OrderService(IRepository<Order> repository,
                            ICouponService couponService,
                            IUserService userService,
                            IRepository<OrderItem> orderItemRepository,
                            IHttpContextAccessor accessor)
        :base(accessor, repository)
        {
            this.m_couponService = couponService;
            this.m_userService = userService;
            this.m_orderItemRepository = orderItemRepository;
        }

        #region GET methods
        public async Task<IList<GetUserOrdersDTO>> GetUsersOrdersAsync(long id)
        {
            var query = await this.Repository.GetAllAsync(includes: new[] { "Code" });

            var orders = await query.Where(o => o.User.Id == id).ToListAsync();

            var result = orders.Select(o => new GetUserOrdersDTO
            {
                Id = o.Id,
                Name = o.Name,
                Comment = o.Comment,
                Status = o.Status,
                TotalPrice = o.TotalPrice,
                Address = o.Address,
                Code = o.Code?.Code,
                Discount = o.Code?.Discount ?? 0,
                CreateDate = o.CreatedDate,
                PhoneNumber = o.PhoneNumber
            }).ToList();

            if (result == null) throw new Exception("Orders not found.");
            return result;
        }
        public async Task<UpdateOrderDTO> GetOrderToUpdateAsync(long id)
        {
            var order = await this.Repository.GetByIdAsync(id);

            if (order == null) throw new Exception("Order not found.");

            return new UpdateOrderDTO()
            {
                Id = order.Id,
                Name = order.Name,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                Status = order.Status
            };
        }
        public async Task<IList<GetOrderDTO>> GetOrdersAsync()
        {
            var ordersquary = await this.Repository.GetAllAsync(includes: new[] { "User", "Code" });

            if (!ordersquary.Any()) throw new Exception("Orders not found");
            var orders = ordersquary.ToList();
            var result = orders.Select(o => new GetOrderDTO
            {
                Id = o.Id,
                Username = o.User?.Login,
                Name = o.Name,
                Email = o.Email,
                PhoneNumber = o.PhoneNumber,
                Address = o.Address,
                Comment = o.Comment,
                TotalPrice = o.TotalPrice,
                Code = o.Code != null
                    ? new GetCouponCodeDTO
                    {
                        Code = o.Code.Code,
                        Discount = o.Code.Discount
                    }
                    : null,
                Status = o.Status,
                UserId = o.User?.Id,
                CreatedDate = o.CreatedDate
            }).ToList();
            return result;
        }
        public async Task<IList<GetOrderItemsDTO>> GetOrderItemsAsync(long id)
        {
            var orderItems = await this.m_orderItemRepository.GetAllAsync(new List<Expression<Func<OrderItem, bool>>>() { o => o.OrderId == id });
            var result = await orderItems.Select(o => new GetOrderItemsDTO
            {
                Name = o.Name,
                Price = o.Price,
                Amount = o.Amount,
                ItemId = o.ItemId
            }).ToListAsync();

            return result;
        }
        #endregion

        public async Task CreateOrderAsync(CreateOrderDTO model) {
            var coupon = await this.m_couponService.GetCouponByCodeAsync(model.Code);
            User user = null;

            var createdBy = string.IsNullOrWhiteSpace(model.UserName)?model.Name:model.UserName;

            var order = new Order {
                Active = true,
                Email = model.Email,
                Address = model.Address,
                Comment = model.Comment,
                CreatedBy = createdBy,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                OrderItems = model.Items.Select(i=> new OrderItem {
                    Amount = i.Amount,
                    ItemId = i.ItemId,
                    Active = true,
                    CreatedBy = createdBy,
                    Name = i.Name,
                    Price = i.Price,

                }).ToList(),
                TotalPrice = coupon != null ? model.TotalPrice - model.TotalPrice * (decimal)(coupon.Discount * 0.01) : model.TotalPrice,
                Status = OrderStatus.Queue,
                Code = coupon
            };

            if(!string.IsNullOrWhiteSpace(model.UserName)) user = await this.m_userService.GetUserByUsernameAsync(model.UserName);

            order.User = user;
            if(coupon != null) {
                coupon.User = user;
                coupon.Active = false;
                await this.m_couponService.UpdateCouponCodeAsync(coupon);
            }

            await this.Repository.CreateAsync(order);

            if(order.Id <= 0) throw new Exception("Creating order error.");
        }
        public async Task UpdateOrderAsync(UpdateOrderDTO model) {
            var order = await this.Repository.GetByIdAsync(model.Id);

            if(order == null) throw new Exception("Order not found.");

            order.Address = model.Address;
            order.Name = model.Name;
            order.Status = model.Status;
            order.PhoneNumber = model.PhoneNumber;
            order.UpdatedBy = this.HttpContext.User.Claims.FirstOrDefault()?.Value;

            await this.Repository.UpdateAsync(order);
        }
        public async Task RemoveOrderAsync(long id) {
            await this.RemoveAsync(id);
        }
    }
}
