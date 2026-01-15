using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class WorkflowDbContextModelCreatingExtensions
{
    public static void ConfigureWorkflow(this ModelBuilder builder)
    {
        builder.Entity<Workflow>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Workflows", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.TriggerType).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
