using FileManagement.DataAccess.Concrete.EntityFrameworkCore.Context;
using FileManagement.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.DataAccess.Concrete.EntityFrameworkCore.Repositories
{
    public class FolderRepository : GenericRepository<Folder>, IFolderDal
    {
        public async Task<List<Folder>> GetAllSubFolders(int folderId)
        {
            List<Folder> result = new List<Folder>();
            await GetFolders(folderId, result);

            return result;
        }


        //Recursive function that take all subfolders inside of one folder.
        private async Task GetFolders(int folderId, ICollection<Folder> result)
        {
            using var context = new FilemanagementContext();

            var folders =await context.Folders.Where(I => I.ParentFolderId== folderId && I.IsDeleted==false).ToListAsync();


            if (folders.Count >0)
            {
                foreach (var folder in folders)
                {
                    if (folder.InverseParentFolder==null)
                        folder.InverseParentFolder = new List<Folder>();

                    await GetFolders(folder.Id, folder.InverseParentFolder);

                    if (!result.Contains(folder))
                    {
                        result.Add(folder);
                    }
                }
            }
        }
    }
}
