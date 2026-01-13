using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finPrestacaoContasDbContextModelCreatingExtensions
{
    public static void ConfigurefinPrestacaoContas(this ModelBuilder builder)
    {
        builder.Entity<finPrestacaoContas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finPrestacaoContases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
