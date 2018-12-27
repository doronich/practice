using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Data.Entities.User;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;

namespace ClothingStore.Service.Interfaces {
    public interface IUserService {
        Task<IList<GetUserDTO>> GetUsersAsync();

        Task<GetUserDTO> GetUserInfoByIdAsync(long id);

        Task<GetProfileDTO> GetProfileByUsernameAsync(string username);

        Task<User> GetUserByUsernameAsync(string username);

        Task<string> InsertUserAsync(RegisterUserDTO model);

        Task UpdateUserAsync(UpdateUserDTO model);

        Task DeleteUserAsync(long id);

        Task<User> GetUserByIdAsync(long id);

        Task<bool> CheckLoginAndEmailForExistAsync(string email, string login);
    }
}
