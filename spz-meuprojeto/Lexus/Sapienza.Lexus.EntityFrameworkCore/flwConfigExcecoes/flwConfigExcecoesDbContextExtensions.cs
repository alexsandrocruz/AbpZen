using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwConfigExcecoesDbContextModelCreatingExtensions
{
    public static void ConfigureflwConfigExcecoes(this ModelBuilder builder)
    {
        builder.Entity<flwConfigExcecoes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "flwConfigExcecoeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
