using FileManagement.Business.DTOs.UserDto;
using FileManagement.Business.Interfaces;
using FileManagement.Business.JwtTool;
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
            if (user!=null)
            {
                var token =_jwtService.GenerateToken(user);
                string userDirectory = Directory.GetCurrentDirectory() + $"/wwwroot/users/{userLoginDto.UserName}";
                if (!Directory.Exists(userDirectory))
                {
                    Directory.CreateDirectory(userDirectory);
                }

                return Created("",token);
            }

            return NotFound(new {Error="Kullanıcı adı veya şifre yanlış.", Code="USERNAME_OR_PASSWORD_WRONG" });
        }
    }
}
