using AutoMapper;
using FileManagement.API.Models;
using FileManagement.Business.Interfaces;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileController : UploadController
    {

        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public FileController(IFolderService folderService, IFileService fileService, IUserService userService)
        {
            _folderService = folderService;
            _fileService = fileService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowableFileById(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            var folder = await _folderService.FindFolderById(file.FolderId);
            var user = await _userService.GetById(id);
            if (string.IsNullOrEmpty(file.FileName))
            {
                return NotFound(new { Message = "Dosya bulunamadı", Code = "FILE_NOT_UPLOADED" });
            }
            return Ok();
        }

        //[HttpGet("[action]/{filename}")]
        //public async Task<IActionResult> DownloadFilesWithZip(int[] fileIds)
        //{
        //    foreach (var item in fileIds)
        //    {
                
        //    }
        //    return Ok();
        //}

        

        [HttpPost("[action]/{folderId}")]
        public async Task<IActionResult> UploadFile(int folderId, [FromForm]IFormFileCollection formFiles)
        {
            var folder = await _folderService.FindFolderById(folderId);
            var user = await _userService.GetById(folder.AppUserId);
            foreach ( var file in formFiles)
            {
                var newName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var result = await UploadFile(file, folder.FileGuid.ToString(), user.Username, newName);
                if (result==true)
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
                }
                else
                {
                    return BadRequest(new { Message = "Dosyalar yüklenirken bir sorun oluştu", Code = "FILE_NOT_UPLOADED" });
                }
            }

            return Created("", new { Message = "Dosyalar başarıyla yüklendi.", Code = "FILE_UPLOADED_SUCCESSFULLY" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            file.IsActive = false;
            await _fileService.UpdateAsync(file);

            return Ok(new { Message = "Dosya başarıyla silindi.", Code = "FILE_DELETED_SUCCESSFULLY" });
        }
    }
}
