using FileManagement.Entities.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace FileManagement.DataAccess
{
    public partial class User : ITable
    {
        public User()
        {
            Folders = new HashSet<Folder>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Folder> Folders { get; set; }
    }
}
