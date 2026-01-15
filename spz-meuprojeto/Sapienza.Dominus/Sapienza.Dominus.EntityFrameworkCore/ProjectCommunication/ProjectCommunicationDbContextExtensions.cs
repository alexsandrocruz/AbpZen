using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProjectCommunicationDbContextModelCreatingExtensions
{
    public static void ConfigureProjectCommunication(this ModelBuilder builder)
    {
        builder.Entity<ProjectCommunication>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ProjectCommunications", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Channel).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Project>()
                .WithMany(p => p.ProjectCommunications)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
