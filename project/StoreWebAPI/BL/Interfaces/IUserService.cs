using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;

namespace ClothingStore.Service.Interfaces {
    public interface IUserService {
        Task<IList<UserDTO>> GetUsersAsync();

        Task<UserDTO> GetUserAsync(long id);

        Task<ProfileDTO> GetUserAsync(string username);

        Task<string> InsertUserAsync(RegisterUserDTO model);

        Task UpdateUserAsync(UpdateUserDTO model);

        Task DeleteUserAsync(long id);
    }
}
