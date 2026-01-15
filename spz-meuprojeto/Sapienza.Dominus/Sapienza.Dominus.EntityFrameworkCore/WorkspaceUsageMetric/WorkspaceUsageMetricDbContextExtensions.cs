using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class WorkspaceUsageMetricDbContextModelCreatingExtensions
{
    public static void ConfigureWorkspaceUsageMetric(this ModelBuilder builder)
    {
        builder.Entity<WorkspaceUsageMetric>(b =>
        {
            b.ToTable(Sapienza.DominusConsts.DbTablePrefix + "WorkspaceUsageMetrics", Sapienza.DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
