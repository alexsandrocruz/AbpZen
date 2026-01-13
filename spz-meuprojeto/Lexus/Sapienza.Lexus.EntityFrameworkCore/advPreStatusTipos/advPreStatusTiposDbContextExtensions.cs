using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreStatusTiposDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreStatusTipos(this ModelBuilder builder)
    {
        builder.Entity<advPreStatusTipos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPreStatusTiposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
