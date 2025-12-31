using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Pro.EntityFrameworkCore;

public class MyProjectHttpApiHostMigrationsDbContext : AbpDbContext<MyProjectHttpApiHostMigrationsDbContext>
{
    public MyProjectHttpApiHostMigrationsDbContext(DbContextOptions<MyProjectHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureCmsKit();
        modelBuilder.ConfigureCmsKitPro();
    }
}
