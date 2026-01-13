using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advVerTiposDbContextModelCreatingExtensions
{
    public static void ConfigureadvVerTipos(this ModelBuilder builder)
    {
        builder.Entity<advVerTipos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advVerTiposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
