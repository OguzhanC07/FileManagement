using AutoMapper;
using FileManagement.API.Models;
using FileManagement.Business.DTOs.FileDto;
using FileManagement.Business.Interfaces;
using HeyRed.Mime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileController : BaseController
    {

        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public FileController(IFolderService folderService, IFileService fileService, IUserService userService, IWebHostEnvironment webHostEnvironment) : base(fileService)
        {
            _folderService = folderService;
            _fileService = fileService;
            _userService = userService;
            _webHostEnviroment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleFile(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            var folder = await _folderService.FindFolderById(file.FolderId);
            var user = await _userService.GetById(folder.AppUserId);
            if (string.IsNullOrEmpty(file.FileName))
            {
                return NotFound(new SingleResponseMessageModel<string> { Result = false, Message = "File not found." });
            }


            string[] paths = { _webHostEnviroment.WebRootPath, @"users/", user.Username, folder.FileGuid.ToString(), file.FileGuid };
            //_webHostEnviroment.WebRootPath, $"/users/{user.Username}/{folder.FileGuid}/{file.FileGuid}".TrimStart(new char[] { '\\', '/' }) old version
            var path = Path.Combine(paths);
            string mimetype = MimeTypesMap.GetMimeType(file.FileGuid.Split(".").Last());
            return PhysicalFile(path, mimetype);
        }

        [HttpPost("[action]/{folderId}")]
        public async Task<IActionResult> UploadFile(int folderId, [FromForm] IFormFileCollection formFiles)
        {
            var folder = await _folderService.FindFolderById(folderId);
            var user = await _userService.GetById(folder.AppUserId);
            int folderSize = 0;
            foreach (var file in formFiles)
            {
                var newName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var result = await UploadFile(file, folder.FileGuid.ToString(), user.Username, newName);
                if (result == true)
                {
                    await _fileService.AddAsync(new DataAccess.File
                    {
                        FileName = file.FileName,
                        FileGuid = newName,
                        FolderId = folder.Id,
                        IsActive = true,
                        Size = Convert.ToInt32(file.Length),
                        UploadedAt = DateTime.Now,
                    });

                    folderSize += Convert.ToInt32(file.Length);
                }
                else
                {
                    return BadRequest(new SingleResponseMessageModel<string> { Result= false, Message="File(s) not uploaded"});
                }
            }

            folder.Size += folderSize;
            await _folderService.UpdateAsync(folder);
            return Created("", new SingleResponseMessageModel<string> { Result=true, Message="File(s) uploaded successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditFile(int id, FileEditDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new SingleResponseMessageModel<string> { Result =false, Message="Id's are not match"});
            }
            var file = await _fileService.GetFileByIdAsync(id);
            file.FileName = dto.FileName;
            await _fileService.UpdateAsync(file);
            return Ok(new SingleResponseMessageModel<string> { Result=true, Message="File name edited successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            file.IsActive = false;
            await _fileService.UpdateAsync(file);

            return Ok(new SingleResponseMessageModel<string> { Result=true, Message="File deleted successfully." });
        }
    }
}
