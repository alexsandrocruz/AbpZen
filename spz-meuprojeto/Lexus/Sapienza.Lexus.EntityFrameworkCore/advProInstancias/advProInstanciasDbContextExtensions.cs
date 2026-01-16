using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProInstanciasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProInstancias(this ModelBuilder builder)
    {
        builder.Entity<advProInstancias>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProInstanciases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
