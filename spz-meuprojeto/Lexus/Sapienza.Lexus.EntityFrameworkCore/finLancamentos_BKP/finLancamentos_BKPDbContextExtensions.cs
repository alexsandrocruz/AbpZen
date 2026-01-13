using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finLancamentos_BKPDbContextModelCreatingExtensions
{
    public static void ConfigurefinLancamentos_BKP(this ModelBuilder builder)
    {
        builder.Entity<finLancamentos_BKP>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finLancamentos_BKPs", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
