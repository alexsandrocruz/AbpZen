using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabCidadesDbContextModelCreatingExtensions
{
    public static void ConfigurefabCidades(this ModelBuilder builder)
    {
        builder.Entity<fabCidades>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabCidadeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
