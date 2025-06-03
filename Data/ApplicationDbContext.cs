using Microsoft.EntityFrameworkCore;
using SycamoreCommercial.Models;

namespace SycamoreCommercial.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, CategoryName="Table", Order= 1 },
                new Category { Id= 2, CategoryName="Security", Order= 2},
                new Category { Id = 3, CategoryName = "Abdullah", Order = 3 },
                new Category { Id = 4, CategoryName = "BhaadeDada", Order = 4 }
                );
        }
    }
}
