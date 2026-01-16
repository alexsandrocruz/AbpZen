using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPautaObsDbContextModelCreatingExtensions
{
    public static void ConfigureadvPautaObs(this ModelBuilder builder)
    {
        builder.Entity<advPautaObs>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPautaObses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
