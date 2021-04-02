using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.SeleniumXunit.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }
    }
}
