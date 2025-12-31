using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;

namespace Volo.Saas.EntityFrameworkCore;

public static class SaasDbContextModelCreatingExtensions
{
    public static void ConfigureSaas(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<Tenant>(b =>
        {
            b.ToTable(SaasDbProperties.DbTablePrefix + "Tenants", SaasDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength).HasColumnName(nameof(Tenant.Name));
            b.Property(x => x.NormalizedName).IsRequired().HasMaxLength(TenantConsts.MaxNameLength).HasColumnName(nameof(Tenant.NormalizedName));

            b.HasMany(x => x.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

            b.HasIndex(x => x.Name);
            b.HasIndex(x => x.NormalizedName);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Edition>(b =>
        {
            b.ToTable(SaasDbProperties.DbTablePrefix + "Editions", SaasDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.DisplayName).IsRequired().HasMaxLength(EditionConsts.MaxDisplayNameLength).HasColumnName(nameof(Edition.DisplayName));

            b.HasIndex(x => x.DisplayName);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<TenantConnectionString>(b =>
        {
            b.ToTable(SaasDbProperties.DbTablePrefix + "TenantConnectionStrings", SaasDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.TenantId, x.Name });

            b.Property(x => x.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength).HasColumnName(nameof(TenantConnectionString.Name));
            b.Property(x => x.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength).HasColumnName(nameof(TenantConnectionString.Value));

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<SaasDbContext>();
    }
}
