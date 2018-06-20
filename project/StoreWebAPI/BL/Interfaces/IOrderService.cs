using System.Collections.Generic;
using System.Threading.Tasks;
using BL.ViewModels;
using DAL.Entities;

namespace BL.Interfaces {
    public interface IOrderService {
        Task CreateOrderAsync(CreateOrderViewModel order);

        Task<IList<Order>> GetOrdersAsync();
    }
}
