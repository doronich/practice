using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;

namespace BL.Interfaces {
    public interface IUserService {
        Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate = null);

        Task<User> GetUserAsync(long id);

        Task InsertUserAsync(User user);

        Task UpdateUserAsync(User user);

        Task DeleteUserAsync(long id);
    }
}
