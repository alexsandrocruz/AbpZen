using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabDatasEFeriadosDbContextModelCreatingExtensions
{
    public static void ConfigurefabDatasEFeriados(this ModelBuilder builder)
    {
        builder.Entity<fabDatasEFeriados>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabDatasEFeriadoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
