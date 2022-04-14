using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<AddPage> AddPages { get; set; }
        public DbSet<AddRole> AddRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<ImagesPaths> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AddRole>()
                .HasIndex(u => u.RoleName)
                .IsUnique();

            builder.Entity<Users>()
               .HasIndex(u => u.Email)
               .IsUnique();

            builder.Entity<AddPage>()
               .HasIndex(u => u.PageName)
               .IsUnique();
        }

    }
}
