using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FileManagement.DataAccess
{
    public partial class User
    {
        public User()
        {
            Folders = new HashSet<Folder>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        public bool isActive { get; set; }

        [InverseProperty(nameof(Folder.AppUser))]
        public virtual ICollection<Folder> Folders { get; set; }
    }
}