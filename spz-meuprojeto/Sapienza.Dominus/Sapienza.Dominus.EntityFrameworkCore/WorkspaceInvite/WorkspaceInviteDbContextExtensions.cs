using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class WorkspaceInviteDbContextModelCreatingExtensions
{
    public static void ConfigureWorkspaceInvite(this ModelBuilder builder)
    {
        builder.Entity<WorkspaceInvite>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "WorkspaceInvites", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Email).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
