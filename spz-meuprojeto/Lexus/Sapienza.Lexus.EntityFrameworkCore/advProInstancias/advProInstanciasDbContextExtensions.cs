using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProInstanciasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProInstancias(this ModelBuilder builder)
    {
        builder.Entity<advProInstancias>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProInstanciases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
