using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Models.Order;

namespace ClothingStore.Service.Interfaces {
    public interface IOrderService {
        Task CreateOrderAsync(CreateOrderDTO order);

        Task<IList<GetOrderDTO>> GetOrdersAsync();

        Task<IList<GetUserOrdersDTO>> GetUsersOrdersAsync(long id);

        Task<IList<GetOrderItemsDTO>> GetOrderItemsAsync(long id);

        Task UpdateOrderAsync(UpdateOrderDTO model);

        Task RemoveOrderAsync(long id);

        Task<UpdateOrderDTO> GetOrderToUpdateAsync(long id);
    }
}
