using AutoMapper;
using FileManagement.Business.DTOs.FolderDto;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnviroment;
     
        public FolderController(IFileService fileService, IUserService userService, IMapper mapper, IFolderService folderService, IWebHostEnvironment webHostEnvironment)
        {
            _folderService = folderService;
            _userService = userService;
            _fileService = fileService;
            _mapper = mapper;
            _webHostEnviroment = webHostEnvironment;
          
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
        public async Task<IActionResult> AddFolder(AddFolderDto dto)
        {
            dto.Size = 0;
            dto.CreatedAt = DateTime.Now;
            dto.ParentFolderId = null;
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
                dto.ParentFolderId = id;
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



        //[HttpGet("[action]/{id}")]
        //public async Task<IActionResult> DownloadFolder(int id)
        //{
        //    var mainfolder = await _folderService.FindFolderById(id);
        //    var user = await _userService.GetById(mainfolder.AppUserId);
        //    var fileName = string.Format("{0}_files.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_1");
        //    var temppath = _webHostEnviroment.WebRootPath + "/TempFiles/";
        //    if (!Directory.Exists(temppath))
        //    {
        //        Directory.CreateDirectory(temppath);
        //    }

        //    var tempOutPutPath = Path.Combine(temppath, fileName);

        //    var subfolders = await _folderService.GetAllSubFolders(id);
        //    using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
        //    {
        //        s.SetLevel(9);
        //        byte[] buffer = new byte[4096];
        //        var filepathList = new List<string>();
        //        foreach (var subfolder in subfolders)
        //        {
        //            string currentfilepath = Path.Combine(_webHostEnviroment.WebRootPath + $"/users/{user.Username}/{subfolder.FileGuid}".TrimStart(new char[] { '\\', '/' }));
        //            ZipEntry entry = new ZipEntry(currentfilepath)
        //            {
        //                DateTime = DateTime.Now,
        //                IsUnicodeText = true,
        //            };

        //            s.PutNextEntry(entry);

        //            using FileStream fs = System.IO.File.OpenRead(currentfilepath);
        //            int sourceBytes;
        //            do
        //            {
        //                sourceBytes = fs.Read(buffer, 0, buffer.Length);
        //                s.Write(buffer, 0, sourceBytes);
        //            } while (sourceBytes > 0);
        //        }

        //        s.Finish();
        //        s.Flush();
        //        s.Close();
        //    }

        //    byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
        //    if (System.IO.File.Exists(tempOutPutPath))
        //        System.IO.File.Delete(tempOutPutPath);

        //    if (finalResult== null)
        //    {
        //        throw new Exception(string.Format("No Files"));
        //    }

        //    return File(finalResult, "application/zip");
        //}


        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult Demo()
        {
            var webRoot = _webHostEnviroment.WebRootPath;
            var fileName = "MyZip.zip";
            var tempOutput = webRoot + "/TempFiles/" + fileName;
            var sourcefile = "connectionstring.txt";
            var sourcefile2 = "ne.txt";

            var source = Path.Combine(webRoot, sourcefile2);

            var zip = ZipFile.Open(tempOutput, ZipArchiveMode.Create);
            zip.CreateEntryFromFile(source,$"Folder1/{sourcefile}",CompressionLevel.Fastest);
            zip.CreateEntryFromFile(source,$"Folder1/{sourcefile2}",CompressionLevel.Fastest);

            zip.Dispose();

            return Ok();
        }
    }
}
