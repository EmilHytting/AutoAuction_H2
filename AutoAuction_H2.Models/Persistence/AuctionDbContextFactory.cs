using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AutoAuction_H2.Models.Persistence
{
    public class AuctionDbContextFactory : IDesignTimeDbContextFactory<AuctionDbContext>
    {
        public AuctionDbContext CreateDbContext(string[] args)
        {
            // Load configuration from API project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AutoAuction_H2.API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("SqlServer");

            var optionsBuilder = new DbContextOptionsBuilder<AuctionDbContext>();

            // 👇 Important: point migrations to Models project
            optionsBuilder.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(AuctionDbContext).Assembly.FullName) // ensures tables only
            );

            return new AuctionDbContext(optionsBuilder.Options);
        }
    }
}
