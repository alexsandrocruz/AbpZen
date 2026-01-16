using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabHistoricoTiposDbContextModelCreatingExtensions
{
    public static void ConfigurefabHistoricoTipos(this ModelBuilder builder)
    {
        builder.Entity<fabHistoricoTipos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabHistoricoTiposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<flwConfigExcecoes>()
                .WithMany(p => p.flwConfigExcecoeses)
                .HasForeignKey(x => x.flwConfigExcecoesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<flwGradeHorarios>()
                .WithMany(p => p.flwGradeHorarioses)
                .HasForeignKey(x => x.flwGradeHorariosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<flwFollows>()
                .WithMany(p => p.flwFollowses)
                .HasForeignKey(x => x.flwFollowsId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
