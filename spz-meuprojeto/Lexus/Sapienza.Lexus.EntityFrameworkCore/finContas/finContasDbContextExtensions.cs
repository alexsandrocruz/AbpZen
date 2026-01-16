using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finContasDbContextModelCreatingExtensions
{
    public static void ConfigurefinContas(this ModelBuilder builder)
    {
        builder.Entity<finContas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finContases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<finExtrato>()
                .WithMany(p => p.finExtratos)
                .HasForeignKey(x => x.finExtratoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<finLancamentos>()
                .WithMany(p => p.finLancamentoses)
                .HasForeignKey(x => x.finLancamentosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
