using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.OpenIddict.Pro.EntityFrameworkCore;

public class AbpOpenIddictProHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AbpOpenIddictProHttpApiHostMigrationsDbContext>
{
    public AbpOpenIddictProHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AbpOpenIddictProHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default")); //OpenIddictPro

        return new AbpOpenIddictProHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
