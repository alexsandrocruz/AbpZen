using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class LeadDbContextModelCreatingExtensions
{
    public static void ConfigureLead(this ModelBuilder builder)
    {
        builder.Entity<Lead>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Leads", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
