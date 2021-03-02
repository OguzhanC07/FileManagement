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
        public FolderManager(IGenericDal<Folder> genericDal) : base(genericDal)
        {
            _genericDal = genericDal;
        }

        public async Task<List<Folder>> GetFoldersByUserId(int id)
        {
            return await _genericDal.GetAllByFilter(I => I.AppUserId == id && I.IsDeleted == false && I.SubFolderId==null);
        }

        public async Task<List<Folder>> GetSubFoldersByFolderId(int id)
        {
            return await _genericDal.GetAllByFilter(I => I.SubFolderId == id && I.IsDeleted == false);
        }

        public async Task<Folder> FindFolderById(int id)
        {
            return await _genericDal.GetByFilter(I => I.Id == id && I.IsDeleted == false);
        }
    }
}
