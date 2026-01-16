using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProTiposDbContextModelCreatingExtensions
{
    public static void ConfigureadvProTipos(this ModelBuilder builder)
    {
        builder.Entity<advProTipos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProTiposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
