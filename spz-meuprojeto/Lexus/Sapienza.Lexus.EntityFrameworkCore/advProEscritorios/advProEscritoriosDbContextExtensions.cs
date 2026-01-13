using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProEscritoriosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProEscritorios(this ModelBuilder builder)
    {
        builder.Entity<advProEscritorios>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProEscritorioses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
