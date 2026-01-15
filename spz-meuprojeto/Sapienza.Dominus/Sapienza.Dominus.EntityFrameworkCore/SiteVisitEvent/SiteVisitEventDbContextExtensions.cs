using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SiteVisitEventDbContextModelCreatingExtensions
{
    public static void ConfigureSiteVisitEvent(this ModelBuilder builder)
    {
        builder.Entity<SiteVisitEvent>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SiteVisitEvents", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Site>()
                .WithMany(p => p.SiteVisitEvents)
                .HasForeignKey(x => x.SiteId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
