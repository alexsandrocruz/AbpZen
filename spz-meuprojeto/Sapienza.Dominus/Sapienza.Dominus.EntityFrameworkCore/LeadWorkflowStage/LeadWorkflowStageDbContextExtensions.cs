using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadWorkflowStageDbContextModelCreatingExtensions
{
    public static void ConfigureLeadWorkflowStage(this ModelBuilder builder)
    {
        builder.Entity<LeadWorkflowStage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadWorkflowStages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadWorkflow>()
                .WithMany(p => p.LeadWorkflowStages)
                .HasForeignKey(x => x.LeadWorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
