using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finPlanoContasGruposDbContextModelCreatingExtensions
{
    public static void ConfigurefinPlanoContasGrupos(this ModelBuilder builder)
    {
        builder.Entity<finPlanoContasGrupos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finPlanoContasGruposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
