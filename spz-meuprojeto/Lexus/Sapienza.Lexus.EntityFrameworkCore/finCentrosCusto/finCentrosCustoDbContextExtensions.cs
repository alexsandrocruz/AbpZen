using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finCentrosCustoDbContextModelCreatingExtensions
{
    public static void ConfigurefinCentrosCusto(this ModelBuilder builder)
    {
        builder.Entity<finCentrosCusto>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finCentrosCustos", LexusConsts.DbSchema);
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
