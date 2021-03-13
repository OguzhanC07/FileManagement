using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.DTOs.FolderDto
{
    public class AddFolderDto
    {
        public string FolderName { get; set; }
        public int? ParentFolderId { get; set; }
        public Guid FileGuid { get; set; }
        public int Size { get; set; }
        public int AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
