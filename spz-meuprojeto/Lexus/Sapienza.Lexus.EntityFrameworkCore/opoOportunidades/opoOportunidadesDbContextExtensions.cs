using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class opoOportunidadesDbContextModelCreatingExtensions
{
    public static void ConfigureopoOportunidades(this ModelBuilder builder)
    {
        builder.Entity<opoOportunidades>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "opoOportunidadeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
