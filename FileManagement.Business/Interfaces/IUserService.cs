using FileManagement.Business.DTOs.UserDto;
using FileManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.Interfaces
{
    public interface IUserService :IGenericService<User>
    {
        public Task<User> CheckUserNameOrPasswordAsync(UserLoginDto dto);
        Task<User> CheckEmailorUsernameAsync(string emailorUsername);
    }
}
