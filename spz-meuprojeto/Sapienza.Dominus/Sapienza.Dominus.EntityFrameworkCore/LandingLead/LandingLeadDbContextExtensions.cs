using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LandingLeadDbContextModelCreatingExtensions
{
    public static void ConfigureLandingLead(this ModelBuilder builder)
    {
        builder.Entity<LandingLead>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LandingLeads", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Email).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
