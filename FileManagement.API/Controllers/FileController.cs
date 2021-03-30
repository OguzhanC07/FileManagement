using AutoMapper;
using FileManagement.API.CustomFilters;
using FileManagement.API.Models;
using FileManagement.Business.DTOs.FileDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using HeyRed.Mime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        public readonly IFolderService _folderService;
        public readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IMapper _mapper;
        public readonly IStringLocalizer<FileController> _localizer;

        public FileController(IStringLocalizer<FileController> localizer, IMapper mapper, IFolderService folderService, IFileService fileService, IUserService userService, IWebHostEnvironment webHostEnvironment) : base(fileService, webHostEnvironment)
        {
            _localizer = localizer;
            _folderService = folderService;
            _fileService = fileService;
            _userService = userService;
            _mapper = mapper;
            _webHostEnviroment = webHostEnvironment;
        }

        [HttpGet("[action]/{id}")]
        [ServiceFilter(typeof(ValidId<Folder>))]
        [UserHasAccessFile(HaveFolderId = true)]
        public async Task<IActionResult> GetFiles(int id)
        {
            return Ok(new ResponseMessageModel<List<FileListDto>>
            {
                Result = true,
                Message = _localizer["FileSendSuccess"],
                Data = _mapper.Map<List<FileListDto>>(await _fileService.GetFilesByFolderId(id))
            });
        }


        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidId<DataAccess.File>))]
        [UserHasAccessFile(HaveFolderId = false)]
        public async Task<IActionResult> GetSingleFile(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            var folder = await _folderService.FindFolderById(file.FolderId);
            
            if (folder == null)
            {
                return NotFound(new ResponseMessageModel<string> { Result = false, Message = _localizer["FolderNotFound"] });
            }

            var user = await _userService.GetById(folder.AppUserId);

            string[] paths = { _webHostEnviroment.WebRootPath, "users", user.Username, folder.FileGuid.ToString(), file.FileGuid };
            var path = Path.Combine(paths);
            var fileResult = System.IO.File.ReadAllBytes(path);
            string mimetype = MimeTypesMap.GetMimeType(file.FileGuid.Split(".").Last());
            return File(fileResult, mimetype);
        }

        [HttpPost("[action]/{id}")]
        [ServiceFilter(typeof(ValidId<Folder>))]
        [UserHasAccessFile(HaveFolderId = true)]
        //action id is folderId.
        public async Task<IActionResult> UploadFile(int id, [FromForm] List<IFormFile> formFiles)
        {
            var folder = await _folderService.FindFolderById(id);
            var user = await _userService.GetById(folder.AppUserId);
            int folderSize = 0;

            foreach (var file in formFiles)
            {
                var newName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var result = await UploadFile(file, folder.FileGuid.ToString(), user.Username, newName);
                if (result == true)
                {
                    await _fileService.AddAsync(new DataAccess.File
                    {
                        FileName = file.FileName.Length>50 ? file.FileName.Substring(0,50) : file.FileName,/*rgx.Replace(file.FileName, "a").Trim(),*/
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
                    return BadRequest(new ResponseMessageModel<string> { Result = false, Message = _localizer["FileUploadError"] });
                }
            }

            folder.Size += folderSize;
            if (folder.ParentFolderId != null)
            {
                var mainfolder = await _folderService.FindFolderById(Convert.ToInt32(folder.ParentFolderId));
                mainfolder.Size += folderSize;
                await _folderService.UpdateAsync(mainfolder);
            }
            await _folderService.UpdateAsync(folder);
            return Created("", new ResponseMessageModel<string> { Result = true, Message = _localizer["FileUploadSuccess"] });
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidId<DataAccess.File>))]
        [UserHasAccessFile(HaveFolderId = false)]
        //id is fileId
        public async Task<IActionResult> EditFile(int id, FileEditDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ResponseMessageModel<string> { Result = false, Message = "Id's are not match" });
            }
            var file = await _fileService.GetFileByIdAsync(id);
            file.FileName = file.FileName.Length>50 ? file.FileName.Substring(0,50) : file.FileName + "." + file.FileGuid.Split(".").Last();
            await _fileService.UpdateAsync(file);
            return Ok(new ResponseMessageModel<string> { Result = true, Message = _localizer["FileNameEdit"] });
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidId<DataAccess.File>))]
        [UserHasAccessFile(HaveFolderId = false)]
        //id is fileId
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            file.IsActive = false;
            await _fileService.UpdateAsync(file);

            var folder = await _folderService.FindFolderById(file.FolderId);
            folder.Size -= file.Size;
            await _folderService.UpdateAsync(folder);
            return Ok(new ResponseMessageModel<string> { Result = true, Message = _localizer["FileDelete"] });
        }
    }
}
