using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace BL.Interfaces {
    public interface IUserService {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(long id);

        Task InsertUserAsync(User user);

        Task UpdateUserAsync(User user);

        Task DeleteUserAsync(long id);
    }
}
