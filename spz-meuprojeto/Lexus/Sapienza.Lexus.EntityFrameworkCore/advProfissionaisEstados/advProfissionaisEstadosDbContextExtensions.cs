using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProfissionaisEstadosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProfissionaisEstados(this ModelBuilder builder)
    {
        builder.Entity<advProfissionaisEstados>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProfissionaisEstadoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
