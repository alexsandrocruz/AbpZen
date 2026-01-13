using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabHistoricoTiposDbContextModelCreatingExtensions
{
    public static void ConfigurefabHistoricoTipos(this ModelBuilder builder)
    {
        builder.Entity<fabHistoricoTipos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabHistoricoTiposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
