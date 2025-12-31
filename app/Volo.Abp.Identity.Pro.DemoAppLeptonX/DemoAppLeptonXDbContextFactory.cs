using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Identity.Pro.DemoAppLeptonX;

public class DemoAppLeptonXDbContextFactory : IDesignTimeDbContextFactory<DemoAppLeptonXDbContext>
{
    public DemoAppLeptonXDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<DemoAppLeptonXDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new DemoAppLeptonXDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
