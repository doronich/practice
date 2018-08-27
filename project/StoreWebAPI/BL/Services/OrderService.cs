using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Order;

namespace ClothingStore.Service.Services {
    public class OrderService : IOrderService {
        private readonly IRepository<Order> m_orderRepository;

        public OrderService(IRepository<Order> orderRepository) {
            this.m_orderRepository = orderRepository;
        }

        public async Task CreateOrderAsync(CreateOrderDTO model) {
            var order = new Order {
                Active = true,
                Email = model.Email,
                Address = model.Address,
                Comment = model.Comment,
                CreatedBy = "User: " + model.Name,
                CreatedDate = DateTime.UtcNow,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                OrderItems = model.Items,
                TotalPrice = model.TotalPrice
            };

            await this.m_orderRepository.CreateAsync(order);

            if(order.Id <= 0) throw new Exception("Creating order error.");
        }

        public async Task<IList<Order>> GetOrdersAsync() {
            var orders = await this.m_orderRepository.GetAllAsync( /*null, new []{"OrderItems"}*/);
            if(!orders.Any()) throw new Exception("Orders not found");
            var result = orders.ToList();
            return result;
        }

        public async Task<IList<OrderItem>> GetOrderItemsAsync(long id) {
            var order = await this.m_orderRepository.GetAllAsync(new List<Expression<Func<Order, bool>>>(){i=>i.Id==id},new []{"OrderItems"});
            var list= await order.ToListAsync();
            var result = list[0].OrderItems.ToList();

            return result;
        }

        public async Task UpdateOrderStatusAsync(UpdateOrderStatusDTO model, string username) {
            var order = await this.m_orderRepository.GetByIdAsync(model.Id);

            if(order==null) throw new Exception("Order not found.");

            order.Id = model.Id;
            order.UpdatedDate = DateTime.UtcNow;
            order.UpdatedBy = username;

            await this.m_orderRepository.UpdateAsync(order);
        }
    }
}
