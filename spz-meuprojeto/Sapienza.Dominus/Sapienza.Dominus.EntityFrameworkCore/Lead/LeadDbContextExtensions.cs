using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadDbContextModelCreatingExtensions
{
    public static void ConfigureLead(this ModelBuilder builder)
    {
        builder.Entity<Lead>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Leads", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadWorkflow>()
                .WithMany(p => p.Leads)
                .HasForeignKey(x => x.LeadWorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<LeadWorkflowStage>()
                .WithMany(p => p.Leads)
                .HasForeignKey(x => x.LeadWorkflowStageId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
