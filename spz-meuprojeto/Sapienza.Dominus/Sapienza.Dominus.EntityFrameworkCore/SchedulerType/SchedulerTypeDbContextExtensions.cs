using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SchedulerTypeDbContextModelCreatingExtensions
{
    public static void ConfigureSchedulerType(this ModelBuilder builder)
    {
        builder.Entity<SchedulerType>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SchedulerTypes", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
