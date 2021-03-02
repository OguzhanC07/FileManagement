using Microsoft.EntityFrameworkCore;
using FileManagement.Entities;

namespace FileManagement.DataAccess.Concrete.EntityFrameworkCore.Context
{
    public partial class FileManagementContext : DbContext
    {
        public FileManagementContext()
        {
        }

        public FileManagementContext(DbContextOptions<FileManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-FERHVEV\\SQLEXPRESS;Database=FileManagement;uid=sa;pwd=1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UploadedAt).HasColumnType("date");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Files__FolderId__2A4B4B5E");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Folders__AppUser__276EDEB3");

                entity.HasOne(d => d.SubFolder)
                    .WithMany(p => p.InverseSubFolder)
                    .HasForeignKey(d => d.SubFolderId)
                    .HasConstraintName("FK__Folders__SubFold__267ABA7A");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
