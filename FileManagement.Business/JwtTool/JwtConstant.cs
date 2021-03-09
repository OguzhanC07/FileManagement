using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.JwtTool
{
    public class JwtConstant
    {
        public const string Issuer = "http://localhost";
        public const string Audience= "http://localhost";
        public const string SecretKey= "my-super-uber-secret-jwt-key";
        public const int ExpiresIn = 60;
    }
}
