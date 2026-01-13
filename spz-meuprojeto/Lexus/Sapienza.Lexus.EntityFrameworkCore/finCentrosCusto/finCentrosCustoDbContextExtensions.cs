using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finCentrosCustoDbContextModelCreatingExtensions
{
    public static void ConfigurefinCentrosCusto(this ModelBuilder builder)
    {
        builder.Entity<finCentrosCusto>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finCentrosCustos", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
