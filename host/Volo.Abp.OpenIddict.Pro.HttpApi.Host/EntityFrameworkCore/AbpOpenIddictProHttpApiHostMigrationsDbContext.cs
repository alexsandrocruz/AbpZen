using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Pro.EntityFrameworkCore;

public class AbpOpenIddictProHttpApiHostMigrationsDbContext : AbpDbContext<AbpOpenIddictProHttpApiHostMigrationsDbContext>
{
    public AbpOpenIddictProHttpApiHostMigrationsDbContext(DbContextOptions<AbpOpenIddictProHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.ConfigureIdentityPro();
        // modelBuilder.ConfigureOpenIddictPro();
        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
    }
}
