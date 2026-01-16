using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliGruposDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliGrupos(this ModelBuilder builder)
    {
        builder.Entity<advCliGrupos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliGruposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientes>()
                .WithMany(p => p.advClienteses)
                .HasForeignKey(x => x.advClientesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
