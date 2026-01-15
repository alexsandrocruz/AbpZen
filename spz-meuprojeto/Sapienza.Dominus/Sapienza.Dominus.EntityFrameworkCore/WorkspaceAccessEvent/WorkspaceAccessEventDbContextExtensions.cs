using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class WorkspaceAccessEventDbContextModelCreatingExtensions
{
    public static void ConfigureWorkspaceAccessEvent(this ModelBuilder builder)
    {
        builder.Entity<WorkspaceAccessEvent>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "WorkspaceAccessEvents", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.EventType).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
