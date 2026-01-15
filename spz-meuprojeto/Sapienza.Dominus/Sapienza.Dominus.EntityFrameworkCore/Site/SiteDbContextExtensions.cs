using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SiteDbContextModelCreatingExtensions
{
    public static void ConfigureSite(this ModelBuilder builder)
    {
        builder.Entity<Site>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Sites", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Slug).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
