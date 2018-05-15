using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BL.Services {
    public class UserService : IUserService {
        private readonly IRepository<User> m_userRepository;

        public UserService(IRepository<User> userRepository) {
            this.m_userRepository = userRepository;
        }

        public async Task DeleteUserAsync(long id) {
            User user = await this.GetUserAsync(id);
            await this.m_userRepository.DeleteAsync(user);
        }

        public async Task<User> GetUserAsync(long id) {
            return await this.m_userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync() {
            return await this.m_userRepository.GetAllAsync();
        }

        public async Task InsertUserAsync(User user) {
            await this.m_userRepository.InsertAsync(user);
        }

        public async Task UpdateUserAsync(User user) {
            await this.m_userRepository.UpdateAsync(user);
        }
    }
}

