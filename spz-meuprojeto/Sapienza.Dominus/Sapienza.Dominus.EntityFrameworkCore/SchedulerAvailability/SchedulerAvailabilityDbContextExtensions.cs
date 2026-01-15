using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SchedulerAvailabilityDbContextModelCreatingExtensions
{
    public static void ConfigureSchedulerAvailability(this ModelBuilder builder)
    {
        builder.Entity<SchedulerAvailability>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SchedulerAvailabilities", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.StartTime).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<SchedulerType>()
                .WithMany(p => p.SchedulerAvailabilities)
                .HasForeignKey(x => x.SchedulerTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
