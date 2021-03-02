using FileManagement.Entities.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagement.DataAccess
{
    public partial class Folder : ITable
    {
        public Folder()
        {
            Files = new HashSet<File>();
            InverseSubFolder = new HashSet<Folder>();
        }

        public int Id { get; set; }
        public string FolderName { get; set; }
        public int Size { get; set; }
        public int? SubFolderId { get; set; }
        public int AppUserId { get; set; }
        public bool IsActive { get; set; }

        public virtual User AppUser { get; set; }
        public virtual Folder SubFolder { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Folder> InverseSubFolder { get; set; }
    }
}
