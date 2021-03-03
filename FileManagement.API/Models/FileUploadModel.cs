using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.Models
{
    public class FileUploadModel
    {
        public string Name { get; set; }
        public IFormFile UploadedFile{ get; set; }
        public string FileName { get; set; }
        public string FileGuid { get; set; }
        public int Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public int FolderId { get; set; }
    }
}
