using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProEscritoriosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProEscritorios(this ModelBuilder builder)
    {
        builder.Entity<advProEscritorios>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProEscritorioses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
