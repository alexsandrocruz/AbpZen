using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProFasesDbContextModelCreatingExtensions
{
    public static void ConfigureadvProFases(this ModelBuilder builder)
    {
        builder.Entity<advProFases>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProFaseses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
