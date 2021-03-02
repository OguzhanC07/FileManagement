using FileManagement.Entities.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagement.DataAccess
{
    public partial class File : ITable
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public int FolderId { get; set; }
        public bool IsActive { get; set; }

        public virtual Folder Folder { get; set; }
    }
}
