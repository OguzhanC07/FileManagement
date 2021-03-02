using FileManagement.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API
{
    public static class IdentityInitializer
    {
        public static async Task Seed(IUserService userService)
        {
            var seedUser = await userService.CheckEmailorUsernameAsync("test");

            if (seedUser == null)
            {
                await userService.AddAsync(new DataAccess.User
                {
                    Email = "test@test.com",
                    Password = "1234",
                    Username = "test",
                    isActive = true
                });
            }
        }

        internal static object Seed(object userService)
        {
            throw new NotImplementedException();
        }
    }
}
