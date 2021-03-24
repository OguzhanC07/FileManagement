using FileManagement.ApiSdk.RequestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public interface IFolderService
    {
        Task<List<FolderList>> GetFoldersByUserId();
        Task<List<FolderList>> GetSubFoldersByFolderId(int folderId);
        Task<byte[]> GetFolder(int id);
        Task<string> EditAsync(string folderName, int id);
        Task<bool> RemoveAsync(int id);
        Task<string> AddAsync(int? id, string folderName);
    }
}
