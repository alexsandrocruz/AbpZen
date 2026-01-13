using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finUnidadesDbContextModelCreatingExtensions
{
    public static void ConfigurefinUnidades(this ModelBuilder builder)
    {
        builder.Entity<finUnidades>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finUnidadeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
