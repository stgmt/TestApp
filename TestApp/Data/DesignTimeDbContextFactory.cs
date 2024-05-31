using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TestApp.Config;

namespace TestApp.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookCatalogContext>
    {
        public BookCatalogContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<BookCatalogContext>();
            var connectionString = DatabaseConfig.ConnectionString;
            builder.UseNpgsql(connectionString);

            return new BookCatalogContext(builder.Options);
        }
    }
}
