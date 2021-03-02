using FileManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.DataAccess.Concrete.EntityFrameworkCore.Repositories
{
    public class FolderRepository : GenericRepository<Folder>, IFolderDal
    {
    }
}
