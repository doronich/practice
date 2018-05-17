using System.Collections.Generic;
using System.Threading.Tasks;
using BL.ViewModels;

namespace BL.Interfaces {
    public interface IUserService {
        Task<IList<UserViewModel>> GetUsersAsync();

        Task<UserViewModel> GetUserAsync(long id);

        Task<string> InsertUserAsync(RegisterUserViewModel model);

        Task UpdateUserAsync(UpdateUserViewModel model);

        Task DeleteUserAsync(long id);
    }
}
