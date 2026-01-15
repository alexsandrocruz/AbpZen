using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class TaskCommentDbContextModelCreatingExtensions
{
    public static void ConfigureTaskComment(this ModelBuilder builder)
    {
        builder.Entity<TaskComment>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "TaskComments", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Content).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Task>()
                .WithMany(p => p.TaskComments)
                .HasForeignKey(x => x.TaskId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
