using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.CmsKit.Pro.EntityFrameworkCore;

public class ProHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<MyProjectHttpApiHostMigrationsDbContext>
{
    public MyProjectHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<MyProjectHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Pro"));

        return new MyProjectHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
