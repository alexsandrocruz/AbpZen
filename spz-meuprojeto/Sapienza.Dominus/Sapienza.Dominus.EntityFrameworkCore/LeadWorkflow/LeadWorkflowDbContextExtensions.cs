using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadWorkflowDbContextModelCreatingExtensions
{
    public static void ConfigureLeadWorkflow(this ModelBuilder builder)
    {
        builder.Entity<LeadWorkflow>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadWorkflows", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
