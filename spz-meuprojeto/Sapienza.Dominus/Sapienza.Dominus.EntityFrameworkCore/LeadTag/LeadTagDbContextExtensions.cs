using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadTagDbContextModelCreatingExtensions
{
    public static void ConfigureLeadTag(this ModelBuilder builder)
    {
        builder.Entity<LeadTag>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadTags", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
