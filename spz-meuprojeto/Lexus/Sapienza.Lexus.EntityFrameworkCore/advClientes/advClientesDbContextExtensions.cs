using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientes(this ModelBuilder builder)
    {
        builder.Entity<advClientes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClienteses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientesArquivos>()
                .WithMany(p => p.advClientesArquivoses)
                .HasForeignKey(x => x.advClientesArquivosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advClientesAtualizacoes>()
                .WithMany(p => p.advClientesAtualizacoeses)
                .HasForeignKey(x => x.advClientesAtualizacoesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advClientesChecklist>()
                .WithMany(p => p.advClientesChecklists)
                .HasForeignKey(x => x.advClientesChecklistId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advProcessos>()
                .WithMany(p => p.advProcessoses)
                .HasForeignKey(x => x.advProcessosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advProcessosClientes>()
                .WithMany(p => p.advProcessosClienteses)
                .HasForeignKey(x => x.advProcessosClientesId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne<advClientesHistoricos>()
                .WithMany(p => p.advClientesHistoricoses)
                .HasForeignKey(x => x.advClientesHistoricosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<opoOportunidades>()
                .WithMany(p => p.opoOportunidadeses)
                .HasForeignKey(x => x.opoOportunidadesId)
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
