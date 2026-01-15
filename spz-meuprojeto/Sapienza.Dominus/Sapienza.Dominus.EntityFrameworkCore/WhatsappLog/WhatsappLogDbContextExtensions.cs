using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class WhatsappLogDbContextModelCreatingExtensions
{
    public static void ConfigureWhatsappLog(this ModelBuilder builder)
    {
        builder.Entity<WhatsappLog>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "WhatsappLogs", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ToPhone).IsRequired();
            b.Property(x => x.Status).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
