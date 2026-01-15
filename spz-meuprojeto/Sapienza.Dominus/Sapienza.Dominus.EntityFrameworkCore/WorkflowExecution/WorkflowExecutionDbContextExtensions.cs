using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class WorkflowExecutionDbContextModelCreatingExtensions
{
    public static void ConfigureWorkflowExecution(this ModelBuilder builder)
    {
        builder.Entity<WorkflowExecution>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "WorkflowExecutions", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Status).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Workflow>()
                .WithMany(p => p.WorkflowExecutions)
                .HasForeignKey(x => x.WorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
