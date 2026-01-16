using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finLancamentosDbContextModelCreatingExtensions
{
    public static void ConfigurefinLancamentos(this ModelBuilder builder)
    {
        builder.Entity<finLancamentos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finLancamentoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<finPrestacaoContas>()
                .WithMany(p => p.finPrestacaoContases)
                .HasForeignKey(x => x.finPrestacaoContasId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
