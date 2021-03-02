using FileManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.JwtTool
{
    public interface IJwtService
    {
        public JwtToken GenerateToken(User user);
    }
}
