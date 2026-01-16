using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabPaisesDbContextModelCreatingExtensions
{
    public static void ConfigurefabPaises(this ModelBuilder builder)
    {
        builder.Entity<fabPaises>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabPaiseses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<fabEstados>()
                .WithMany(p => p.fabEstadoses)
                .HasForeignKey(x => x.fabEstadosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
