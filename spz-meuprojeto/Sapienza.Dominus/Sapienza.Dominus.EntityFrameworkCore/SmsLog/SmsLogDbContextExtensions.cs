using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class SmsLogDbContextModelCreatingExtensions
{
    public static void ConfigureSmsLog(this ModelBuilder builder)
    {
        builder.Entity<SmsLog>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "SmsLogs", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ToPhone).IsRequired();
            b.Property(x => x.Status).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
