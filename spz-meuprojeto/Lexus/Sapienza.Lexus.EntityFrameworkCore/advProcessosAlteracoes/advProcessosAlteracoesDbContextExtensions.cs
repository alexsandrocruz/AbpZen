using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosAlteracoesDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosAlteracoes(this ModelBuilder builder)
    {
        builder.Entity<advProcessosAlteracoes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProcessosAlteracoeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
