using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliPrioridadesDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliPrioridades(this ModelBuilder builder)
    {
        builder.Entity<advCliPrioridades>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliPrioridadeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
