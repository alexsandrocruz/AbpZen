using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finPlanoContasDbContextModelCreatingExtensions
{
    public static void ConfigurefinPlanoContas(this ModelBuilder builder)
    {
        builder.Entity<finPlanoContas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finPlanoContases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
