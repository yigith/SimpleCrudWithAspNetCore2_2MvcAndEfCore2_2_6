using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FirstName = "Angelina", LastName = "Jolie" },
                new Student { Id = 2, FirstName = "Brad", LastName = "Pitt" });
        }

        public DbSet<Student> Students { get; set; }
    }
}
