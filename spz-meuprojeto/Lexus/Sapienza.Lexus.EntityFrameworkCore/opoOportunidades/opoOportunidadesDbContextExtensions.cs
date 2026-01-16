using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class opoOportunidadesDbContextModelCreatingExtensions
{
    public static void ConfigureopoOportunidades(this ModelBuilder builder)
    {
        builder.Entity<opoOportunidades>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "opoOportunidadeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<opoOrcamentos>()
                .WithMany(p => p.opoOrcamentoses)
                .HasForeignKey(x => x.opoOrcamentosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<flwFollows>()
                .WithMany(p => p.flwFollowses)
                .HasForeignKey(x => x.flwFollowsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
