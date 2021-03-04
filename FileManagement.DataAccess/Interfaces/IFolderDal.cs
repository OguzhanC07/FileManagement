using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.DataAccess.Interfaces
{
    public interface IFolderDal : IGenericDal<Folder>
    {
        Task<List<Folder>> GetAllSubFolders(int folderId);
    }
}
