using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.Forms.EntityFrameworkCore;

public class FormsHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<MyProjectHttpApiHostMigrationsDbContext>
{
    public MyProjectHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<MyProjectHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Forms"));

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
