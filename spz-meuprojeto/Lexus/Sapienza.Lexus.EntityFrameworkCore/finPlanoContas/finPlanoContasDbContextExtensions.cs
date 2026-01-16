using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finPlanoContasDbContextModelCreatingExtensions
{
    public static void ConfigurefinPlanoContas(this ModelBuilder builder)
    {
        builder.Entity<finPlanoContas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finPlanoContases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<finLancamentos>()
                .WithMany(p => p.finLancamentoses)
                .HasForeignKey(x => x.finLancamentosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
