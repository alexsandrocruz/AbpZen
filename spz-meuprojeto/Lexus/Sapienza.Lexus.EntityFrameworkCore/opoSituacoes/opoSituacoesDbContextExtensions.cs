using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class opoSituacoesDbContextModelCreatingExtensions
{
    public static void ConfigureopoSituacoes(this ModelBuilder builder)
    {
        builder.Entity<opoSituacoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "opoSituacoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
