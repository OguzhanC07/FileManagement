using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.DTOs.FolderDto
{
    public class FolderListDto
    {
        public int Id { get; set; }
        public string FolderName { get; set; }
        public int? ParentFolderId { get; set; }
        public int Size { get; set; }
        public int AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
