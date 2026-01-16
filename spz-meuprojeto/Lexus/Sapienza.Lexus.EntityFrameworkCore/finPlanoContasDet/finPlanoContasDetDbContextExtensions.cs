using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finPlanoContasDetDbContextModelCreatingExtensions
{
    public static void ConfigurefinPlanoContasDet(this ModelBuilder builder)
    {
        builder.Entity<finPlanoContasDet>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finPlanoContasDets", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
