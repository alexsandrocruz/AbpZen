using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProProbabilidadesDbContextModelCreatingExtensions
{
    public static void ConfigureadvProProbabilidades(this ModelBuilder builder)
    {
        builder.Entity<advProProbabilidades>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProProbabilidadeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
