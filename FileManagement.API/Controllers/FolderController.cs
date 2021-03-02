using AutoMapper;
using FileManagement.Business.DTOs.FolderDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
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
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public FolderController(IUserService userService, IMapper mapper, IFolderService folderService)
        {
            _folderService = folderService;
            _userService = userService;
            _mapper = mapper;
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetFoldersByAppUserId(int id)
        {
            return Ok(_mapper.Map<List<FolderListDto>>(await _folderService.GetFoldersByUserId(id)));
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetSubFoldersByFolderId(int id)
        {
            return Ok(_mapper.Map<List<FolderListDto>>(await _folderService.GetSubFoldersByFolderId(id)));
        }

        [HttpPost]
        //[FileNameCheck]
        public async Task<IActionResult> AddFolder(AddFolderDto dto)
        {
            dto.Size = 0;
            dto.CreatedAt = DateTime.Now;
            dto.SubFolderId = null;
            dto.FileGuid = Guid.NewGuid();

            await _folderService.AddAsync(_mapper.Map<Folder>(dto));
            var user = await _userService.GetById(dto.AppUserId);
            string userDirectory = Directory.GetCurrentDirectory() + $"/wwwroot/users/{user.Username}/{dto.FileGuid}";
            Directory.CreateDirectory(userDirectory);

            return Created("", new { Message = "Başarıyla klasör oluşturuldu.", Code = "CREATED_SUCCESSFULLY" });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddSubFolder(int id, AddFolderDto dto)
        {
            var file = await _folderService.FindFolderById(id);
            if (file!=null)
            {
                dto.Size = 0;
                dto.CreatedAt = DateTime.Now;
                dto.SubFolderId = id;
                dto.FileGuid = Guid.NewGuid();

                await _folderService.AddAsync(_mapper.Map<Folder>(dto));
                var user = await _userService.GetById(dto.AppUserId);
                string userDirectory = Directory.GetCurrentDirectory() + $"/wwwroot/users/{user.Username}/{dto.FileGuid}";
                Directory.CreateDirectory(userDirectory);
                return Created("", new { Message = "Başarıyla klasör oluşturuldu.", Code = "CREATED_SUCCESSFULLY" });
            }
            else
            {
                return BadRequest(new { Message = "Ana klasör olmadığından oluşturamazsınız.", Code = "PARENT_FOLDER_NOT_EXIST" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            var folder = await _folderService.FindFolderById(id);
            folder.IsDeleted = true;
            await _folderService.UpdateAsync(folder);

            return Ok(new { Message = "Başarıyla silindi", Code = "DELETED_SUCCESSFULLY" });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditFolder(int id, FolderEditDto folderEditDto)
        {
            if (id != folderEditDto.Id)
            {
                return BadRequest(new { Error = "Id'ler uyuşmuyor", Code = "ID_IS_NOT_MATCHED" });
            }
            var folder = await _folderService.GetById(id);
            if (folder!=null)
            {
                folder.FolderName = folderEditDto.FolderName;

                await _folderService.UpdateAsync(folder);
                return Ok(new { Message = "Başarıyla güncellendi", Code = "UPDATED_SUCCESSFULLY" });
            }
            return NotFound("Böyle bir klasör bulunamadı.");
        }

    }
}
