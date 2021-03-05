using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        private readonly IFileService _fileService;
        public BaseController(IFileService fileService)
        {
            _fileService = fileService;
        }

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

        internal async Task AddSubFoldersToZip(string mainPath, string zipPath, List<Folder> subfolders)
        {
            foreach (var subfolder in subfolders)
            {
                if (subfolder.InverseParentFolder.Count == 0)
                {
                    var files = await _fileService.GetFilesByFolderId(subfolder.Id);
                    if (files.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        using FileStream zipToOpen = new FileStream(zipPath, FileMode.Open);
                        using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);

                        foreach (var file in files)
                        {
                            archive.CreateEntryFromFile(Path.Combine(mainPath, subfolder.FileGuid.ToString(), file.FileGuid), Path.Combine(subfolder.FolderName, file.FileName));
                        }
                    }

                }
                else
                {
                    var files = await _fileService.GetFilesByFolderId(subfolder.Id);
                    if (files.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        using FileStream zipToOpen = new FileStream(zipPath, FileMode.Open);
                        using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
                        foreach (var file in files)
                        {
                            archive.CreateEntryFromFile(Path.Combine(mainPath, subfolder.FileGuid.ToString(), file.FileGuid), Path.Combine(subfolder.FolderName, file.FileName));
                        }
                        archive.Dispose();
                    }
                    await AddSubFoldersToZip(mainPath, zipPath, subfolder.InverseParentFolder.ToList());
                }
            }
        }
    }
}
