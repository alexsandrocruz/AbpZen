using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProjectFollowerDbContextModelCreatingExtensions
{
    public static void ConfigureProjectFollower(this ModelBuilder builder)
    {
        builder.Entity<ProjectFollower>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ProjectFollowers", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Project>()
                .WithMany(p => p.ProjectFollowers)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
