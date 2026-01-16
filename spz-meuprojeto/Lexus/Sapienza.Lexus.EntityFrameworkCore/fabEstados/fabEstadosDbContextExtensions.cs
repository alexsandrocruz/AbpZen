using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabEstadosDbContextModelCreatingExtensions
{
    public static void ConfigurefabEstados(this ModelBuilder builder)
    {
        builder.Entity<fabEstados>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabEstadoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<fabCidades>()
                .WithMany(p => p.fabCidadeses)
                .HasForeignKey(x => x.fabCidadesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
