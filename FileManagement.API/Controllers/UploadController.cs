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
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class UploadController : ControllerBase
    {
        internal async Task<bool> UploadFile(IFormFile file, string foldername,string username,string guidName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/users/{username}/{foldername}/{guidName}");
                var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
