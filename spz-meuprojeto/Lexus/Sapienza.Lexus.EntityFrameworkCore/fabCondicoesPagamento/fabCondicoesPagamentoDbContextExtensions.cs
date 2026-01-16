using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabCondicoesPagamentoDbContextModelCreatingExtensions
{
    public static void ConfigurefabCondicoesPagamento(this ModelBuilder builder)
    {
        builder.Entity<fabCondicoesPagamento>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabCondicoesPagamentos", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<opoOrcamentos>()
                .WithMany(p => p.opoOrcamentoses)
                .HasForeignKey(x => x.opoOrcamentosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
