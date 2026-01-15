using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadAutomationDbContextModelCreatingExtensions
{
    public static void ConfigureLeadAutomation(this ModelBuilder builder)
    {
        builder.Entity<LeadAutomation>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadAutomations", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadWorkflow>()
                .WithMany(p => p.LeadAutomations)
                .HasForeignKey(x => x.LeadWorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
