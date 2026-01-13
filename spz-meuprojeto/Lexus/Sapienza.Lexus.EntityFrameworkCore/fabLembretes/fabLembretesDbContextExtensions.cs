using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabLembretesDbContextModelCreatingExtensions
{
    public static void ConfigurefabLembretes(this ModelBuilder builder)
    {
        builder.Entity<fabLembretes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabLembreteses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
