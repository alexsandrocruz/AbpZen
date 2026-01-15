using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class TaskDbContextModelCreatingExtensions
{
    public static void ConfigureTask(this ModelBuilder builder)
    {
        builder.Entity<Task>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Tasks", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Project>()
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
