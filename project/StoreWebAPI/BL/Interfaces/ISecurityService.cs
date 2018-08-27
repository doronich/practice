using System.Threading.Tasks;
using ClothingStore.Service.Models;
using ClothingStore.Service.Models.User;

namespace ClothingStore.Service.Interfaces {
    public interface ISecurityService {
        Task<string> GetTokenAsync(LoginUserDTO model, bool reg = false);

        Task<string> EncryptPasswordAsync(string pass);

        Task ChangePasswordAsync(ChangePasswordDTO model);
    }
}
