using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vaccine.Models;
using Vaccine.Models.f;

namespace Vaccine
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRoles> UserInRoles { get; set; }
        public DbSet<File> Files { get; set; }

        public DbSet<Org> Orgs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=34.122.9.109;Port=5432;Database=vaccine-release-db;Username=mobnius;Password=XfOgQt");
        }
    }
}
