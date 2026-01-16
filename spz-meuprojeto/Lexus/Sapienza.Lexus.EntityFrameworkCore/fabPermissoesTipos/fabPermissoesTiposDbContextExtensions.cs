using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabPermissoesTiposDbContextModelCreatingExtensions
{
    public static void ConfigurefabPermissoesTipos(this ModelBuilder builder)
    {
        builder.Entity<fabPermissoesTipos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabPermissoesTiposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<fabPermissoes>()
                .WithMany(p => p.fabPermissoeses)
                .HasForeignKey(x => x.fabPermissoesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
