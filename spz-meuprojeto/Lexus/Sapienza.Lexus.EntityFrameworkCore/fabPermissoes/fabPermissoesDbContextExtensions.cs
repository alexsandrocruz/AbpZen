using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabPermissoesDbContextModelCreatingExtensions
{
    public static void ConfigurefabPermissoes(this ModelBuilder builder)
    {
        builder.Entity<fabPermissoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabPermissoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
