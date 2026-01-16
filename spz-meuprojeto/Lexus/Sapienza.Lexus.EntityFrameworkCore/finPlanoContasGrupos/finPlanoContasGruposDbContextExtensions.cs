using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finPlanoContasGruposDbContextModelCreatingExtensions
{
    public static void ConfigurefinPlanoContasGrupos(this ModelBuilder builder)
    {
        builder.Entity<finPlanoContasGrupos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finPlanoContasGruposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<finPlanoContas>()
                .WithMany(p => p.finPlanoContases)
                .HasForeignKey(x => x.finPlanoContasId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
