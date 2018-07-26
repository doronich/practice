using System.Collections.Generic;
using System.Threading.Tasks;
using BL.ViewModels;
using DAL.Entities;

namespace BL.Interfaces {
    public interface IOrderService {
        Task CreateOrderAsync(CreateOrderViewModel order);

        Task<IList<Order>> GetOrdersAsync();

        Task<IList<OrderItem>> GetOrderItemsAsync(long id);
    }
}
