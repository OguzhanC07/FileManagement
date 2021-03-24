using AutoMapper;
using FileManagement.API.CustomFilters;
using FileManagement.API.Models;
using FileManagement.Business.DTOs.FolderDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using HeyRed.Mime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FolderController : BaseController
    {
        public readonly IFolderService _folderService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public readonly IStringLocalizer<FolderController> _localizer;

        public FolderController(IStringLocalizer<FolderController> localizer, IFileService fileService, IUserService userService, IMapper mapper, IFolderService folderService, IWebHostEnvironment webHostEnvironment) : base(fileService,webHostEnvironment)
        {
            _localizer = localizer;
            _folderService = folderService;
            _userService = userService;
            _fileService = fileService;
            _mapper = mapper;
            _webHostEnviroment = webHostEnvironment;
        }

        [HttpGet("[action]/{id}")]
        [ServiceFilter(typeof(ValidId<User>))]
        [UserHasAccessFolder(CheckUserId = true)]
        public async Task<IActionResult> GetFoldersByAppUserId(int id)
        {
            return Ok(new MultipleDataResponseMessageModel<FolderListDto>
            {
                Result = true,
                Message = _localizer["FolderSendSuccess"],
                Data = _mapper.Map<List<FolderListDto>>(await _folderService.GetFoldersByUserId(id))
            });
        }

        [HttpGet("[action]/{id}")]
        [ServiceFilter(typeof(ValidId<Folder>))]
        [UserHasAccessFolder(CheckUserId = false)]
        public async Task<IActionResult> GetSubFoldersByFolderId(int id)
        {
            return Ok(new MultipleDataResponseMessageModel<FolderListDto>
            {
                Result = true,
                Message = _localizer["FolderSendSuccess"],
                Data = _mapper.Map<List<FolderListDto>>(await _folderService.GetSubFoldersByFolderId(id))
            });
        }

        [HttpPost("{id?}")]
        [UserHasAccessFolder(CheckUserId = false)]
        public async Task<IActionResult> AddSubOrMainFolder(int? id, AddFolderDto dto)
        {
            if (id != null && id != 0)
            {
                if (await _folderService.FindFolderById(Convert.ToInt32(id)) == null)
                {
                    return BadRequest(new SingleResponseMessageModel<int>
                    {
                        Result = false,
                        Message = _localizer["AddSubFolderToDoesNotExistFolder"],
                    });
                }
                else
                    dto.ParentFolderId = id;
            }


            dto.Size = 0;
            dto.CreatedAt = DateTime.Now;
            dto.FileGuid = Guid.NewGuid();
            dto.FolderName.Trim();
            dto.AppUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _folderService.AddAsync(_mapper.Map<Folder>(dto));
            var user = await _userService.GetById(dto.AppUserId);
            string userDirectory = Directory.GetCurrentDirectory() + $"/wwwroot/users/{user.Username}/{dto.FileGuid}";
            Directory.CreateDirectory(userDirectory);

            return Created("", new SingleResponseMessageModel<AddFolderDto>
            {
                Result = true,
                Message = _localizer["FolderCreateSuccess"],
                Data = dto
            });
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidId<Folder>))]
        [UserHasAccessFolder(CheckUserId = false)]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            var folder = await _folderService.FindFolderById(id);
            folder.IsDeleted = true;
            await _folderService.UpdateAsync(folder);

            return Ok(new SingleResponseMessageModel<string>
            {
                Result = true,
                Message = _localizer["FolderDeleteSuccess"]
            });
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidId<Folder>))]
        [UserHasAccessFolder(CheckUserId = false)]
        public async Task<IActionResult> EditFolder(int id, FolderEditDto folderEditDto)
        {
            if (id != folderEditDto.Id)
            {
                return BadRequest(new SingleResponseMessageModel<string> { Result = false, Message = _localizer["IdsAreNotMatch"] });
            }

            var folder = await _folderService.GetById(id);

            folder.FolderName = folderEditDto.FolderName.Trim();
            await _folderService.UpdateAsync(folder);
            return Ok(new SingleResponseMessageModel<string> { Result = true, Message = _localizer["FolderEditSuccess"] });
        }

        [HttpGet("[action]/{id}")]
        [ServiceFilter(typeof(ValidId<Folder>))]
        [UserHasAccessFolder(CheckUserId = false)]
        public async Task<IActionResult> DownloadFolder(int id)
        {
            //zipname and zip path.
            var zipName = Guid.NewGuid().ToString() + ".zip";
            var tempOutput = Path.Combine(Path.Combine(_webHostEnviroment.WebRootPath, @"TempFiles\"), zipName);

            if (!Directory.Exists(Path.Combine(_webHostEnviroment.WebRootPath, @"TempFiles\")))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnviroment.WebRootPath, @"TempFiles\"));
            }

            //check main folder and files also look new path for folders
            var folder = await _folderService.FindFolderById(id);
            var user = await _userService.GetById(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var files = await _fileService.GetFilesByFolderId(id);
            var mainPath = Path.Combine(_webHostEnviroment.WebRootPath, "users", user.Username);

            //create zip
            var zip = ZipFile.Open(tempOutput, ZipArchiveMode.Create);

            //look for files in main folder and add to zip
            foreach (var file in files)
            {
                string[] zipPaths = { folder.FolderName, file.FileName };
                var source = Path.Combine(mainPath, folder.FileGuid.ToString(), file.FileGuid);
                var zipSource = Path.Combine(zipPaths);
                zip.CreateEntryFromFile(source, zipSource, CompressionLevel.Fastest);
            }
            zip.Dispose();

            var subfolders = await _folderService.GetAllSubFolders(id);
            if (subfolders.Count > 0)
            {
                await AddSubFoldersToZip(mainPath, tempOutput, subfolders);
            }

            string mimetype = MimeTypesMap.GetMimeType(zipName);
            return PhysicalFile(Path.Combine(_webHostEnviroment.WebRootPath, @"TempFiles\", zipName), mimetype, zipName);
        }
    }
}
