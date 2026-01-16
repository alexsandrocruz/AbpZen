using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabFormasPagamentoDbContextModelCreatingExtensions
{
    public static void ConfigurefabFormasPagamento(this ModelBuilder builder)
    {
        builder.Entity<fabFormasPagamento>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabFormasPagamentos", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
