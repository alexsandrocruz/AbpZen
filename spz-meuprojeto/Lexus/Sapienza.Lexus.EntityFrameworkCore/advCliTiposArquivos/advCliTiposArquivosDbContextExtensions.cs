using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliTiposArquivosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliTiposArquivos(this ModelBuilder builder)
    {
        builder.Entity<advCliTiposArquivos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliTiposArquivoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientesArquivos>()
                .WithMany(p => p.advClientesArquivoses)
                .HasForeignKey(x => x.advClientesArquivosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<advClientesChecklist>()
                .WithMany(p => p.advClientesChecklists)
                .HasForeignKey(x => x.advClientesChecklistId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
