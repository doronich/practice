using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Models;

namespace ClothingStore.Service.Interfaces {
    public interface IUserService {
        Task<IList<UserDTO>> GetUsersAsync();

        Task<UserDTO> GetUserAsync(long id);

        Task<string> InsertUserAsync(RegisterUserDTO model);

        Task UpdateUserAsync(UpdateUserDTO model);

        Task DeleteUserAsync(long id);
    }
}
