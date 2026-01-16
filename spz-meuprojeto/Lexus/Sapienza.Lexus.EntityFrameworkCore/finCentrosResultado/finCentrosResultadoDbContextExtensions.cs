using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finCentrosResultadoDbContextModelCreatingExtensions
{
    public static void ConfigurefinCentrosResultado(this ModelBuilder builder)
    {
        builder.Entity<finCentrosResultado>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finCentrosResultados", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
