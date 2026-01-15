using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SitePageVersionDbContextModelCreatingExtensions
{
    public static void ConfigureSitePageVersion(this ModelBuilder builder)
    {
        builder.Entity<SitePageVersion>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SitePageVersions", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<SitePage>()
                .WithMany(p => p.SitePageVersions)
                .HasForeignKey(x => x.SitePageId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
