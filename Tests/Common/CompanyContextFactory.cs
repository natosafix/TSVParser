using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Tests.Common
{
    public class CompanyContextFactory
    {
        public static CompanyDbContext Create()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("DbConnection");
            var options = new DbContextOptionsBuilder<CompanyDbContext>()
                .UseNpgsql(configuration)
                .Options;
            var context = new CompanyDbContext(options);
            return context;
        }

        public static void Destroy(CompanyDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}