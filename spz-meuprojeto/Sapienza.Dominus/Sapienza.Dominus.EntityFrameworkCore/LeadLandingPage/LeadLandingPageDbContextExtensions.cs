using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadLandingPageDbContextModelCreatingExtensions
{
    public static void ConfigureLeadLandingPage(this ModelBuilder builder)
    {
        builder.Entity<LeadLandingPage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadLandingPages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();
            b.Property(x => x.Slug).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadWorkflow>()
                .WithMany(p => p.LeadLandingPages)
                .HasForeignKey(x => x.LeadWorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
