using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finExtratoDbContextModelCreatingExtensions
{
    public static void ConfigurefinExtrato(this ModelBuilder builder)
    {
        builder.Entity<finExtrato>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finExtratos", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
