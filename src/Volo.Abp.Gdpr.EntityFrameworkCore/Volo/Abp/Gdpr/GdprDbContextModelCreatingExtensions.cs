using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Gdpr;

public static class GdprDbContextModelCreatingExtensions
{
    public static void ConfigureGdpr(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<GdprRequest>(b =>
        {
            b.ToTable(GdprDbProperties.DbTablePrefix + "Requests", GdprDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.UserId).IsRequired().HasColumnName(nameof(GdprRequest.UserId));
            b.Property(x => x.ReadyTime).IsRequired().HasColumnName(nameof(GdprRequest.ReadyTime));

            b.HasMany(x => x.Infos).WithOne().HasForeignKey(x => x.RequestId).IsRequired();
            
            b.HasIndex(x => x.UserId);
        });
        
        builder.Entity<GdprInfo>(b =>
        {
            b.ToTable(GdprDbProperties.DbTablePrefix + "Info", GdprDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Provider).IsRequired().HasColumnName(nameof(GdprInfo.Provider)).HasMaxLength(GdprInfoConsts.MaxProviderLength);
            b.Property(x => x.Data).IsRequired().HasColumnName(nameof(GdprInfo.Data));

            b.HasIndex(x => x.RequestId);
        });
    }
}