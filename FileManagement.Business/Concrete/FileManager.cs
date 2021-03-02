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
        public FileManager(IGenericDal<File> genericDal) : base(genericDal)
        {
        }
    }
}
