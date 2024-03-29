﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FileManagement.DataAccess
{
    public partial class Folder
    {
        public Folder()
        {
            Files = new HashSet<File>();
            InverseParentFolder = new HashSet<Folder>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FolderName { get; set; }
        public Guid FileGuid { get; set; }
        public int Size { get; set; }
        public int? ParentFolderId { get; set; }
        public int AppUserId { get; set; }
        public bool IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AppUserId))]
        [InverseProperty(nameof(User.Folders))]
        public virtual User AppUser { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        [InverseProperty(nameof(Folder.InverseParentFolder))]
        public virtual Folder ParentFolder { get; set; }
        [InverseProperty(nameof(File.Folder))]
        public virtual ICollection<File> Files { get; set; }
        [InverseProperty(nameof(Folder.ParentFolder))]
        public virtual ICollection<Folder> InverseParentFolder { get; set; }
    }
}