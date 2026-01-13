using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finRateiosPadraoDbContextModelCreatingExtensions
{
    public static void ConfigurefinRateiosPadrao(this ModelBuilder builder)
    {
        builder.Entity<finRateiosPadrao>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finRateiosPadraos", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
