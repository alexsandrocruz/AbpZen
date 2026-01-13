using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabPermissoesTiposDbContextModelCreatingExtensions
{
    public static void ConfigurefabPermissoesTipos(this ModelBuilder builder)
    {
        builder.Entity<fabPermissoesTipos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabPermissoesTiposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
