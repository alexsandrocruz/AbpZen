using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SiteVisitDailyStatDbContextModelCreatingExtensions
{
    public static void ConfigureSiteVisitDailyStat(this ModelBuilder builder)
    {
        builder.Entity<SiteVisitDailyStat>(b =>
        {
            b.ToTable(Sapienza.DominusConsts.DbTablePrefix + "SiteVisitDailyStats", Sapienza.DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Site>()
                .WithMany(p => p.SiteVisitDailyStats)
                .HasForeignKey(x => x.SiteId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
