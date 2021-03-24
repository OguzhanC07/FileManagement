using FileManagement.ApiSdk.RequestClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public interface IFileService
    {
        Task<bool> RemoveFile(int id);
        Task<string> UploadFile(int folderId, string filePath);
        Task<string> EditFile(string name, int id);
        Task<byte[]> GetSingleFile(int id);
        Task<List<FileList>> GetFiles(int folderId);
    }
}
