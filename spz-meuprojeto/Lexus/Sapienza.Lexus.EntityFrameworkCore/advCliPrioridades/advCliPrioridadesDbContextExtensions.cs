using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliPrioridadesDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliPrioridades(this ModelBuilder builder)
    {
        builder.Entity<advCliPrioridades>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliPrioridadeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
