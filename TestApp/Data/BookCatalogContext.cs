using Microsoft.EntityFrameworkCore;
using TestApp.Config;
using TestApp.Data.Models;

namespace TestApp.Data
{
    public class BookCatalogContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookCatalogContext(DbContextOptions<BookCatalogContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(DatabaseConfig.ConnectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ISBN).HasMaxLength(20);
                entity.Property(e => e.Genre).HasMaxLength(100);
            });
        }
    }
}
