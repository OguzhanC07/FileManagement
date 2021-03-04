using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using FileManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.Concrete
{
    public class FolderManager : GenericManager<Folder>, IFolderService
    {
        private readonly IGenericDal<Folder> _genericDal;
        private readonly IFolderDal _folderDal;
        public FolderManager(IFolderDal folderDal,IGenericDal<Folder> genericDal) : base(genericDal)
        {
            _genericDal = genericDal;
            _folderDal = folderDal;
        }

        public async Task<List<Folder>> GetFoldersByUserId(int id)
        {
            return await _genericDal.GetAllByFilter(I => I.AppUserId == id && I.IsDeleted == false && I.ParentFolderId==null);
        }

        public async Task<List<Folder>> GetSubFoldersByFolderId(int id)
        {
            return await _genericDal.GetAllByFilter(I => I.ParentFolderId == id && I.IsDeleted == false);
        }

        public async Task<Folder> FindFolderById(int id)
        {
            return await _genericDal.GetByFilter(I => I.Id == id && I.IsDeleted == false);
        }

        public async Task<List<Folder>> GetAllSubFolders(int folderId)
        {
           return await _folderDal.GetAllSubFolders(folderId);
        }
    }
}
