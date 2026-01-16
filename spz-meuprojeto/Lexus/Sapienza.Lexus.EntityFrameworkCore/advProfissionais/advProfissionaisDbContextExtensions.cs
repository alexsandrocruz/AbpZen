using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProfissionaisDbContextModelCreatingExtensions
{
    public static void ConfigureadvProfissionais(this ModelBuilder builder)
    {
        builder.Entity<advProfissionais>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProfissionaises", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advProfissionaisEstados>()
                .WithMany(p => p.advProfissionaisEstadoses)
                .HasForeignKey(x => x.advProfissionaisEstadosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advProfissionaisNaturezas>()
                .WithMany(p => p.advProfissionaisNaturezases)
                .HasForeignKey(x => x.advProfissionaisNaturezasId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advVerbas>()
                .WithMany(p => p.advVerbases)
                .HasForeignKey(x => x.advVerbasId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
