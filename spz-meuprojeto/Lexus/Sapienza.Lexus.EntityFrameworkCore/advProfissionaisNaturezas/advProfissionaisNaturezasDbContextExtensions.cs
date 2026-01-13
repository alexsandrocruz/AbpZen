using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProfissionaisNaturezasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProfissionaisNaturezas(this ModelBuilder builder)
    {
        builder.Entity<advProfissionaisNaturezas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProfissionaisNaturezases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
