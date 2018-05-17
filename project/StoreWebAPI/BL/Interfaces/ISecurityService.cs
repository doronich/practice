using System.Threading.Tasks;
using BL.Options;
using BL.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace BL.Interfaces {
    public interface ISecurityService {
        SymmetricSecurityKey GetSymmetricSecurityKey();
        Task<string> GetTokenAsync(LoginUserViewModel model, bool reg=false);

        string EncryptPassword(string pass);
    }
}
