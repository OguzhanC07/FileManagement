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
        public string Name { get; set; }
        public int Size { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
