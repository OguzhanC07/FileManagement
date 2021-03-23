using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public interface IFolderService
    {
        Task<bool> RemoveAsync(string userName, string password, int id);
        Task<bool> AddAsync(string userName, string password, string folderName);
    }
}
