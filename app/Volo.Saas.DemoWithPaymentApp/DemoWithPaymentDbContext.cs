using Microsoft.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Payment.EntityFrameworkCore;

namespace Volo.Saas.DemoWithPaymentApp;

public class DemoWithPaymentDbContext : AbpDbContext<DemoWithPaymentDbContext>
{
    public DemoWithPaymentDbContext(DbContextOptions<DemoWithPaymentDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigureIdentity();
        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigureTenantManagement();
        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureSaas();
        modelBuilder.ConfigurePayment();
    }
}
