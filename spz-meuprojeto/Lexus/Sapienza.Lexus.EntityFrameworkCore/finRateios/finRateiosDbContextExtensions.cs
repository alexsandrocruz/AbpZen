using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finRateiosDbContextModelCreatingExtensions
{
    public static void ConfigurefinRateios(this ModelBuilder builder)
    {
        builder.Entity<finRateios>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finRateioses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
