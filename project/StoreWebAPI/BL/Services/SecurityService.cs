using System.Text;
using BL.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BL.Services {
    public class SecurityService {
        public SecurityService(IOptions<AuthSettings> options) {
            this.Settings = options.Value;
        }

        public AuthSettings Settings { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Settings.Key));
        }
    }
}
