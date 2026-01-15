using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProjectResponsibleDbContextModelCreatingExtensions
{
    public static void ConfigureProjectResponsible(this ModelBuilder builder)
    {
        builder.Entity<ProjectResponsible>(b =>
        {
            b.ToTable(Sapienza.DominusConsts.DbTablePrefix + "ProjectResponsibles", Sapienza.DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Project>()
                .WithMany(p => p.ProjectResponsibles)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
