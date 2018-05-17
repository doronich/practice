using System.Threading.Tasks;
using BL.ViewModels;

namespace BL.Interfaces {
    public interface ISecurityService {
        Task<string> GetTokenAsync(LoginUserViewModel model, bool reg = false);

        Task<string> EncryptPasswordAsync(string pass);
    }
}
