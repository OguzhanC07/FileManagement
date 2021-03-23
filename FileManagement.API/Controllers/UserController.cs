using FileManagement.API.Models;
using FileManagement.Business.DTOs.UserDto;
using FileManagement.Business.Interfaces;
using FileManagement.Business.JwtTool;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public UserController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _userService.CheckUserNameOrPasswordAsync(userLoginDto);
            if (user != null)
            {
                var token = _jwtService.GenerateToken(user);
                string userDirectory = Directory.GetCurrentDirectory() + $"/wwwroot/users/{userLoginDto.UserName}";
                if (!Directory.Exists(userDirectory))
                {
                    Directory.CreateDirectory(userDirectory);
                }

                return Created("", new SingleResponseMessageModel<object>
                {
                    Result = true,
                    Message = "Giriş Başarılı",
                    Data = new
                    {
                        token.Token,
                        user.Username,
                        user.Id,
                        ExpirationDate = new DateTimeOffset(DateTime.UtcNow.AddMinutes(JwtConstant.ExpiresIn)).ToUnixTimeMilliseconds()
                    }
                });
            }

            return NotFound(new SingleResponseMessageModel<JwtToken>
            {
                Result = false,
                Message = "Kullanıcı adı veya şifre yanlış."
            });
        }

        //[HttpGet]
        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetActiveUser()
        //{
        //    var user = await _userService.CheckEmailorUsernameAsync(User.Identity.Name);

        //    return Ok(new SingleResponseMessageModel<object> { Result = true, Message = "Başarılı", Data = new { user.Username, user.Id, user.Email } });
        //}

    }
}
