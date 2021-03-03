using FileManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.Interfaces
{
    public interface IFileService : IGenericService<File>
    {
        Task<List<File>> GetFilesByFolderId(int id);
        Task<File> GetFileByIdAsync(int id);
    }
}
