using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public interface IFileService
    {
        Task<bool> RemoveFile(string username, string password, int id);
        Task<bool> UploadFile(string username, string password, int folderId, string filePath);
    }
}
