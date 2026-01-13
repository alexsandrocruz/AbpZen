using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProfissionaisEstadosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProfissionaisEstados(this ModelBuilder builder)
    {
        builder.Entity<advProfissionaisEstados>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProfissionaisEstadoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
