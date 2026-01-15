using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SchedulerExceptionDbContextModelCreatingExtensions
{
    public static void ConfigureSchedulerException(this ModelBuilder builder)
    {
        builder.Entity<SchedulerException>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SchedulerExceptions", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<SchedulerType>()
                .WithMany(p => p.SchedulerExceptions)
                .HasForeignKey(x => x.SchedulerTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
