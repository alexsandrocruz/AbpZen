using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliGruposDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliGrupos(this ModelBuilder builder)
    {
        builder.Entity<advCliGrupos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliGruposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
