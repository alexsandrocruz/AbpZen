using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabRegioesDbContextModelCreatingExtensions
{
    public static void ConfigurefabRegioes(this ModelBuilder builder)
    {
        builder.Entity<fabRegioes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabRegioeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
