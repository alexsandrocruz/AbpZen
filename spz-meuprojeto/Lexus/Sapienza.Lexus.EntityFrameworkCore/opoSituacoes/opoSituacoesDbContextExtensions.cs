using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class opoSituacoesDbContextModelCreatingExtensions
{
    public static void ConfigureopoSituacoes(this ModelBuilder builder)
    {
        builder.Entity<opoSituacoes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "opoSituacoeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<opoOportunidades>()
                .WithMany(p => p.opoOportunidadeses)
                .HasForeignKey(x => x.opoOportunidadesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
