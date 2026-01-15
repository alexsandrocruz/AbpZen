using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SitePageDbContextModelCreatingExtensions
{
    public static void ConfigureSitePage(this ModelBuilder builder)
    {
        builder.Entity<SitePage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SitePages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();
            b.Property(x => x.Slug).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Site>()
                .WithMany(p => p.SitePages)
                .HasForeignKey(x => x.SiteId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
