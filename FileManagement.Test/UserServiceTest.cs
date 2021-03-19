using FileManagement.Business.Concrete;
using FileManagement.Business.DTOs.UserDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using FileManagement.DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task CheckUserByEmailOrUsername_ShouldReturnUser_IfUserExists()
        {
            //Arrange
            string email = "test@test1.com";
            var expected = users.Where(I => I.Email == email).SingleOrDefault();
            _userMockRepo.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((Expression<Func<User,bool>> exp)=> 
            {
                return users.Where(exp.Compile()).SingleOrDefault();
            });

            //Act
            var actual = await _sut.CheckEmailorUsernameAsync(email);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CheckUserNameAndPassword_ShouldReturnUser_IfUserExists()
        {
            //Arrange
            string username = "tester";
            string password = "1234";
            var dto = new UserLoginDto { UserName = username, Password = password};
            var expected = users.Where(I => I.Username == dto.UserName && I.Password == dto.Password).SingleOrDefault();
            _userMockRepo.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((Expression<Func<User, bool>> exp) =>
               {
                   return users.Where(exp.Compile()).SingleOrDefault();
               });

            //Act
            var actual = await _sut.CheckUserNameOrPasswordAsync(dto);
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CheckUserNameAndPassword_ShouldReturnNull_IfUserDontExists()
        {
            //Arrange
            string username = "tester21";
            string password = "1234";
            var dto = new UserLoginDto { UserName = username, Password = password };
            var expected = users.Where(I => I.Username == dto.UserName && I.Password == dto.Password).SingleOrDefault();
            _userMockRepo.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(()=>null);

            //Act
            var actual = await _sut.CheckUserNameOrPasswordAsync(dto);
            //Assert
            Assert.Null(actual);
            Assert.Equal(expected, actual);
        }



        private List<User> users = new List<User>
        {
            new User { Email = "test@test1.com", Id = 1, Password = "1234", isActive = true, Username = "tester" },
            new User { Email = "test@test2.com", Id = 2, Password = "1234", isActive = true, Username = "tester1" },
            new User { Email = "test@test3.com", Id = 3, Password = "1234", isActive = true, Username = "tester2" },
            new User { Email = "test@test4.com", Id = 4, Password = "1234", isActive = true, Username = "tester3" },
        };
    }
}
