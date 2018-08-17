using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Models;
using ClothingStore.Data.Entities;

namespace ClothingStore.Service.Interfaces {
    public interface IOrderService {
        Task CreateOrderAsync(CreateOrderDTO order);

        Task<IList<Order>> GetOrdersAsync();

        Task<IList<OrderItem>> GetOrderItemsAsync(long id);
    }
}
