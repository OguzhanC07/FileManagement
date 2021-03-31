using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FileManagement.DataAccess
{
    public partial class File
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FileName { get; set; }
        public int Size { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadedAt { get; set; }
        public int FolderId { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [StringLength(50)]
        public string FileGuid { get; set; }

        [ForeignKey(nameof(FolderId))]
        [InverseProperty("Files")]
        public virtual Folder Folder { get; set; }
    }
}