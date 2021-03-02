using FileManagement.Business.DTOs.UserDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using FileManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.Concrete
{
    public class UserManager : GenericManager<User>, IUserService
    {
        private readonly IGenericDal<User> _genericDal;
        public UserManager(IGenericDal<User> genericDal) : base(genericDal)
        {
            _genericDal = genericDal;
        }

        public async Task<User> CheckEmailorUsernameAsync(string emailorUsername)
        {
            var result = await _genericDal.GetByFilter(I => I.Username == emailorUsername || I.Email == emailorUsername);
            return result;
        }

        public async Task<User> CheckUserNameOrPasswordAsync(UserLoginDto dto)
        {
            var result = await _genericDal.GetByFilter(I => I.Password == dto.Password && I.Username == dto.UserName);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
