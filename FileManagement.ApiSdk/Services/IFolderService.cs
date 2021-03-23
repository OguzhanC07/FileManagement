using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public interface IFolderService
    {
        Task<bool> RemoveAsync(int id);
        Task<bool> AddAsync(string folderName);
    }
}
