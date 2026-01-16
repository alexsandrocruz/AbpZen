using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuDistanciasDbContextModelCreatingExtensions
{
    public static void ConfigureusuDistancias(this ModelBuilder builder)
    {
        builder.Entity<usuDistancias>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "usuDistanciases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
