using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProNaturezasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProNaturezas(this ModelBuilder builder)
    {
        builder.Entity<advProNaturezas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProNaturezases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
