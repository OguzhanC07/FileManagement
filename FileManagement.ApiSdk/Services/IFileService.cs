using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public interface IFileService
    {
        Task<bool> RemoveFile(int id);
        Task<bool> UploadFile(int folderId, string filePath);
    }
}
