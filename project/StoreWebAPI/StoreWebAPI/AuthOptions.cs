using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreWebAPI
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer";//издатель токена
        public const string AUDIENCE = "http://localhost:52016/";//потребитель токена
        const string KEY = "secret_key_da123"; //ключ для шифрации
        public const int LIFETIME = 20; //время жизни токена
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
