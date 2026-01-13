using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finCentrosResultadoDbContextModelCreatingExtensions
{
    public static void ConfigurefinCentrosResultado(this ModelBuilder builder)
    {
        builder.Entity<finCentrosResultado>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finCentrosResultados", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
