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
    public class FileManager : GenericManager<File>, IFileService
    {
        private readonly IGenericDal<File> _genericDal;
        public FileManager(IGenericDal<File> genericDal) : base(genericDal)
        {
            _genericDal = genericDal;
        }

        public async Task<List<File>> GetFilesByFolderId(int id)
        {
            return await _genericDal.GetAllByFilter(I => I.Id == id && I.IsActive==false);
        }

        public async Task<File> GetFileByIdAsync(int id)
        {
            return await _genericDal.GetByFilter(I => I.Id == id && I.IsActive == false);
        } 
    }
}
