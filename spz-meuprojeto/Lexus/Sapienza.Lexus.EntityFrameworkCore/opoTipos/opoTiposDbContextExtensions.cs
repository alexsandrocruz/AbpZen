using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class opoTiposDbContextModelCreatingExtensions
{
    public static void ConfigureopoTipos(this ModelBuilder builder)
    {
        builder.Entity<opoTipos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "opoTiposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
