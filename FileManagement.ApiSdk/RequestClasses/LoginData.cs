using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.RequestClasses
{
    public class LoginData
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public int Id { get; set; }
        public long ExpirationDate { get; set; }
    }
}
