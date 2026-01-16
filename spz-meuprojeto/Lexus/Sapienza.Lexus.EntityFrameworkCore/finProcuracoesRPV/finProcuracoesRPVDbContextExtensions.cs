using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finProcuracoesRPVDbContextModelCreatingExtensions
{
    public static void ConfigurefinProcuracoesRPV(this ModelBuilder builder)
    {
        builder.Entity<finProcuracoesRPV>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finProcuracoesRPVs", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
