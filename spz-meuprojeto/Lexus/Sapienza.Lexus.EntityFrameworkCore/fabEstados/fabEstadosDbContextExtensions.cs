using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabEstadosDbContextModelCreatingExtensions
{
    public static void ConfigurefabEstados(this ModelBuilder builder)
    {
        builder.Entity<fabEstados>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabEstadoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
