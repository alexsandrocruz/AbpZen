using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabLembretesDbContextModelCreatingExtensions
{
    public static void ConfigurefabLembretes(this ModelBuilder builder)
    {
        builder.Entity<fabLembretes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabLembreteses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
