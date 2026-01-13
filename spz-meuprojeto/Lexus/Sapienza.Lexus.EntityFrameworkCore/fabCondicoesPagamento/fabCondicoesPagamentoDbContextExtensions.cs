using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabCondicoesPagamentoDbContextModelCreatingExtensions
{
    public static void ConfigurefabCondicoesPagamento(this ModelBuilder builder)
    {
        builder.Entity<fabCondicoesPagamento>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabCondicoesPagamentos", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
