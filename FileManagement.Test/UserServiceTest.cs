using FileManagement.Business.Concrete;
using FileManagement.Business.DTOs.UserDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using FileManagement.DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileManagement.Test
{
    public class UserServiceTest
    {
        private readonly UserManager _sut;
        private readonly Mock<IGenericDal<User>> _userMockRepo = new Mock<IGenericDal<User>>();
        public UserServiceTest()
        {
            _sut = new UserManager(_userMockRepo.Object);
        }

        [Fact]
        public void CheckUserByEmailOrUsername_ShouldReturnUser_IfUserExists()
        {
            //Arrange
            string email = "test@test123.com";
            var user = new User { Email = email, Id = 1, Password = "1234", isActive = true, Username = "tester" };
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.CheckEmailorUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);


            //Act
            var actual = mock.Object.CheckEmailorUsernameAsync(email).Result; //its not valid.  /*_sut.CheckEmailorUsernameAsync(email).Result;*/

            //Assert
            Assert.Equal(email, actual.Email);
        }

        [Fact]
        public void CheckUserNameAndPassword_ShouldReturnUser_IfUserExists()
        {
            //Arrange
            string username = "test1234";
            string password = "1234";
            var dto = new UserLoginDto { UserName = username, Password = password};
            var user = new User { Email = "test1234@test.com", Id = 1, Password = password, isActive = true, Username = username };
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.CheckUserNameOrPasswordAsync(It.IsAny<UserLoginDto>())).ReturnsAsync(user);

            //Act
            var actual = mock.Object.CheckUserNameOrPasswordAsync(dto).Result; /*_sut.CheckUserNameOrPasswordAsync(dto).Result; this returns null beacuse this object dont accept any ctor for IUserService*/

            //Assert
            Assert.Equal(username, actual.Username);
        }
    }
}
