using FileManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.Interfaces
{
    public interface IFolderService : IGenericService<Folder>
    {
        Task<List<Folder>> GetAllSubFolders(int folderId);

        Task<List<Folder>> GetFoldersByUserId(int id);
        Task<List<Folder>> GetSubFoldersByFolderId(int id);
        Task<Folder> FindFolderById(int id);
    }
}
