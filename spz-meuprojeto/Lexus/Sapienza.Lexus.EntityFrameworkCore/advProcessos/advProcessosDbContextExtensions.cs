using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessos(this ModelBuilder builder)
    {
        builder.Entity<advProcessos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProcessoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advProcessosClientes>()
                .WithMany(p => p.advProcessosClienteses)
                .HasForeignKey(x => x.advProcessosClientesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advProcessosHonorarios>()
                .WithMany(p => p.advProcessosHonorarioses)
                .HasForeignKey(x => x.advProcessosHonorariosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advProcessosMeritos>()
                .WithMany(p => p.advProcessosMeritoses)
                .HasForeignKey(x => x.advProcessosMeritosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advCompromissos>()
                .WithMany(p => p.advCompromissoses)
                .HasForeignKey(x => x.advCompromissosId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne<advProcessosAlteracoes>()
                .WithMany(p => p.advProcessosAlteracoeses)
                .HasForeignKey(x => x.advProcessosAlteracoesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advProcessosDadosHerdeiros>()
                .WithMany(p => p.advProcessosDadosHerdeiroses)
                .HasForeignKey(x => x.advProcessosDadosHerdeirosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advTarefas>()
                .WithMany(p => p.advTarefases)
                .HasForeignKey(x => x.advTarefasId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne<advVerbas>()
                .WithMany(p => p.advVerbases)
                .HasForeignKey(x => x.advVerbasId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
