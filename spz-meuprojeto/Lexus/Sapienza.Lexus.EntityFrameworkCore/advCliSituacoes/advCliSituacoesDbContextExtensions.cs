using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliSituacoesDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliSituacoes(this ModelBuilder builder)
    {
        builder.Entity<advCliSituacoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliSituacoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
